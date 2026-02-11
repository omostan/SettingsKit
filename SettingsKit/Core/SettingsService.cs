#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <                  SettingsKit                  |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: SettingsKit                                                       *
*            Filename: SettingsService.cs                                                *
*              Author: Stanley Omoregie                                                  *
*        Created Date: 07.02.2026                                                        *
*       Modified Date: 11.02.2026                                                        *
*          Created By: Stanley Omoregie                                                  *
*    Last Modified By: Stanley Omoregie                                                  *
*           CopyRight: copyright © 2026 Novomatic AG.                                    *
*                  .oooO  Oooo.                                                          *
*                  (   )  (   )                                                          *
* ------------------\ (----) /---------------------------------------------------------- *
*                    \_)  (_/                                                            *
*****************************************************************************************/

#endregion copyright

using System.ComponentModel;
using System.Text.Json;
using SettingsKit.Security;

namespace SettingsKit.Core;

/// <summary>
/// Provides loading, saving, migration, backup, and optional encryption
/// for strongly typed settings models.
/// </summary>
/// <typeparam name="T">A settings model derived from <see cref="ObservableObject"/>.</typeparam>
public class SettingsService<T> where T : ObservableObject, new()
{
    private readonly string _filePath;
    private readonly int _currentVersion;
    private readonly List<ISettingsMigration<T>> _migrations = [];
    private readonly SemaphoreSlim _saveGate = new(1, 1);
    private CancellationTokenSource? _saveCts;
    private readonly TimeSpan _saveDebounce = TimeSpan.FromMilliseconds(200);
    private volatile bool _isSaving;
    private readonly SynchronizationContext? _synchronizationContext;
    private readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

    /// <summary>
    /// Gets the active settings instance.
    /// </summary>
    public T Settings { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsService{T}"/> class.
    /// </summary>
    /// <param name="filePath">The full path to the JSON settings file.</param>
    /// <param name="currentVersion">The current schema version.</param>
    public SettingsService(string filePath, int currentVersion)
    {
        _filePath = filePath;
        _currentVersion = currentVersion;
        _synchronizationContext = SynchronizationContext.Current;

        // Load settings from file, applying migrations and decryption as needed
        Settings = Load();
        
        // Subscribe to property changes to trigger saves
        Settings.PropertyChanged += OnSettingsChanged;
    }

    /// <summary>
    /// Registers a migration step for upgrading settings.
    /// </summary>
    public void AddMigration(ISettingsMigration<T> migration) => _migrations.Add(migration);

    /// <summary>
    /// Loads the settings from the file, applies migrations if needed, and decrypts encrypted properties.
    /// </summary>
    /// <returns>A settings instance loaded from file, restored from backup, or a new default instance.</returns>
    private T Load()
    {
        // Ensure the directory exists before attempting to read or create the file
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath) ?? string.Empty);

        // If the file doesn't exist, attempt to restore from backup or create default settings
        if (!File.Exists(_filePath))
            return TryRestoreBackup() ?? CreateDefault();

        try
        {
            // Load settings from file
            var json = File.ReadAllText(_filePath);
            var settings = JsonSerializer.Deserialize<T>(json);

            // If deserialization fails, create default settings
            if (settings == null)
                return CreateDefault();

            // If the settings version is older than the current version, apply migrations
            if (settings.Version < _currentVersion)
                settings = Migrate(settings);

            // After loading and migrating, decrypt any encrypted properties for in-memory use
            DecryptProperties(settings);

            // Return the loaded settings instance
            return settings;
        }
        catch
        {
            return TryRestoreBackup() ?? CreateDefault();
        }
    }

    /// <summary>
    /// Creates a default settings instance with the current version and persists it to disk.
    /// </summary>
    /// <returns>A new default instance of <typeparamref name="T"/> with the current schema version.</returns>
    private T CreateDefault()
    {
        // Create a new instance with the current version set,
        // so that migrations can properly identify the version
        var defaultSettings = new T {
            Version = _currentVersion
        };
        
        // Save the default settings to create the file and set correct permissions
        var json = JsonSerializer.Serialize(defaultSettings, SerializerOptions);
        File.WriteAllText(_filePath, json);
        
        // No need to decrypt since we just created it, but ensure
        // any encrypted properties are properly initialized
        return defaultSettings;
    }

    /// <summary>
    /// Applies all applicable migrations to upgrade settings from their current version to the target version.
    /// </summary>
    /// <param name="settings">The settings instance to migrate.</param>
    /// <returns>The migrated settings instance with the version updated to <see cref="_currentVersion"/>.</returns>
    private T Migrate(T settings)
    {
        bool migrated;

        do
        {
            migrated = false;

            foreach (var m in _migrations
                         .Where(m => m.FromVersion == settings.Version))
            {
                settings.Version = m.ToVersion; // update version before migration
                settings = m.Migrate(settings);
                migrated = true;
                break;
            }
        } while (migrated && settings.Version < _currentVersion);

        return settings;
    }

    /// <summary>
    /// Handles changes to the settings properties and schedules a save operation.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The property change event arguments.</param>
    private void OnSettingsChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (_isSaving)
            return;

        ScheduleSave();
    }

    /// <summary>
    /// Schedules a debounced save operation by canceling any pending save and creating a new delayed task.
    /// </summary>
    private void ScheduleSave()
    {
        _saveCts?.Cancel();
        var cts = new CancellationTokenSource();
        _saveCts = cts;
        var token = cts.Token;

        _ = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(_saveDebounce, token).ConfigureAwait(false);
                await SaveAsync(token).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                // Superseded by a newer change.
            }
        }, token);
    }

    /// <summary>
    /// Asynchronously saves the settings to disk with concurrency control and synchronization context marshaling.
    /// </summary>
    /// <param name="token">A cancellation token to cancel the save operation.</param>
    /// <remarks>
    /// This method acquires a semaphore gate to ensure only one save operation occurs at a time.
    /// If a UI synchronization context exists, the actual save operation is marshaled to that context.
    /// The method sets the <see cref="_isSaving"/> flag to prevent recursive saves during property changes.
    /// </remarks>
    private async Task SaveAsync(CancellationToken token)
    {
        await _saveGate.WaitAsync(token).ConfigureAwait(false);
        try
        {
            _isSaving = true;
            
            // If we have a UI synchronization context, marshal the save to that thread
            if (_synchronizationContext != null)
            {
                var tcs = new TaskCompletionSource<bool>();
                _synchronizationContext.Post(_ =>
                {
                    try
                    {
                        SaveCore();
                        tcs.SetResult(true);
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                }, null);
                
                await tcs.Task.ConfigureAwait(false);
            }
            else
            {
                SaveCore();
            }
        }
        finally
        {
            _isSaving = false;
            _saveGate.Release();
        }
    }

    /// <summary>
    /// Saves the current settings to disk with backup, encryption, and atomic replace semantics.
    /// </summary>
    /// <remarks>
    /// This method backs up the existing file, encrypts annotated properties for persistence,
    /// writes to a temporary file, and replaces the original file to reduce corruption risk.
    /// It then decrypts the in-memory settings for continued use.
    /// </remarks>
    private void SaveCore()
    {
        try
        {
            Backup();
            EncryptProperties(Settings);

            Settings.Version = _currentVersion;

            var json = JsonSerializer.Serialize(Settings, SerializerOptions);

            // Write to temporary file first for atomic operation
            var tempFile = _filePath + ".tmp";
            File.WriteAllText(tempFile, json);
            
            // Replace original file with temp file
            File.Move(tempFile, _filePath, overwrite: true);

            DecryptProperties(Settings); // keep in-memory object usable
        }
        catch
        {
            // Ensure properties are decrypted even if save fails
            try
            {
                DecryptProperties(Settings);
            }
            catch
            {
                // Swallow secondary errors to keep the app running.
            }
        }
    }

    /// <summary>
    /// Creates a backup of the current settings file before saving.
    /// </summary>
    /// <remarks>
    /// If the settings file exists, it is copied to a `.bak` file.
    /// Any existing backup is deleted first to ensure only the latest backup is retained.
    /// If the backup operation fails, the exception is silently caught to prevent blocking the save operation.
    /// </remarks>
    private void Backup()
    {
        if (!File.Exists(_filePath))
            return;

        try
        {
            var backupPath = _filePath + ".bak";
            
            // Delete old backup if it exists
            if (File.Exists(backupPath))
            {
                File.Delete(backupPath);
            }
            
            // Create new backup
            File.Copy(_filePath, backupPath, overwrite: false);
        }
        catch
        {
            // If backup fails, continue anyway - better to save than to fail completely
        }
    }

    /// <summary>
    /// Attempts to restore settings from a backup file if the primary settings file is missing or corrupted.
    /// </summary>
    /// <returns>A deserialized settings instance from the backup file, or <c>null</c> if the backup does not exist or cannot be read.</returns>
    /// <remarks>
    /// This method reads from a `.bak` file and attempts to restore it as the primary settings file.
    /// If any error occurs during reading or deserialization, the method silently returns <c>null</c> to allow fallback to default settings.
    /// </remarks>
    private T? TryRestoreBackup()
    {
        var backup = _filePath + ".bak";

        if (!File.Exists(backup))
            return null;

        try
        {
            var json = File.ReadAllText(backup);
            File.Copy(backup, _filePath, overwrite: false);
            return JsonSerializer.Deserialize<T>(json);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Encrypts string properties on the settings instance that are marked with <see cref="EncryptedAttribute"/>.
    /// </summary>
    /// <param name="settings">The settings object whose annotated properties will be encrypted.</param>
    private static void EncryptProperties(T settings)
    {
        try
        {
            var props = typeof(T).GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(EncryptedAttribute)));

            foreach (var p in props)
            {
                var value = p.GetValue(settings) as string;
                if (!string.IsNullOrEmpty(value))
                    p.SetValue(settings, EncryptionHelper.Encrypt(value));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    /// <summary>
    /// Decrypts string properties on the settings instance that are marked with <see cref="EncryptedAttribute"/>.
    /// </summary>
    /// <param name="settings">The settings object whose annotated properties will be decrypted.</param>
    private static void DecryptProperties(T settings)
    {
        try
        {
            var props = typeof(T).GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(EncryptedAttribute)));

            foreach (var p in props)
            {
                var value = p.GetValue(settings) as string;
                if (!string.IsNullOrEmpty(value))
                    p.SetValue(settings, EncryptionHelper.TryDecrypt(value));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
