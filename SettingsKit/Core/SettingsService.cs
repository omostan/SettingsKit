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
*       Modified Date: 07.02.2026                                                        *
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

        Settings = Load();
        Settings.PropertyChanged += OnSettingsChanged;
    }

    /// <summary>
    /// Registers a migration step for upgrading settings.
    /// </summary>
    public void AddMigration(ISettingsMigration<T> migration)
        => _migrations.Add(migration);

    /// <summary>
    /// Loads the settings from the file, applies migrations if needed, and decrypts encrypted properties.
    /// </summary>
    /// <returns>A settings instance loaded from file, restored from backup, or a new default instance.</returns>
    private T Load()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath) ?? string.Empty);

        if (!File.Exists(_filePath))
            return CreateDefault();

        try
        {
            var json = File.ReadAllText(_filePath);
            var settings = JsonSerializer.Deserialize<T>(json);

            if (settings == null)
                return CreateDefault();

            if (settings.Version < _currentVersion)
                settings = Migrate(settings);

            DecryptProperties(settings);

            return settings;
        }
        catch
        {
            return TryRestoreBackup() ?? CreateDefault();
        }
    }

    private T CreateDefault()
    {
        return new T
        {
            Version = _currentVersion
        };
    }

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

    private void OnSettingsChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (_isSaving)
            return;

        ScheduleSave();
    }

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

    private async Task SaveAsync(CancellationToken token)
    {
        await _saveGate.WaitAsync(token).ConfigureAwait(false);
        try
        {
            _isSaving = true;
            SaveCore();
        }
        finally
        {
            _isSaving = false;
            _saveGate.Release();
        }
    }

    /// <summary>
    /// Saves the settings to disk.
    /// </summary>
    private void SaveCore()
    {
        try
        {
            Backup();
            EncryptProperties(Settings);

            Settings.Version = _currentVersion;

            var json = JsonSerializer.Serialize(Settings, new JsonSerializerOptions
            {
                WriteIndented = true
            });

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

    private T? TryRestoreBackup()
    {
        var backup = _filePath + ".bak";

        if (!File.Exists(backup))
            return null;

        try
        {
            var json = File.ReadAllText(backup);
            return JsonSerializer.Deserialize<T>(json);
        }
        catch
        {
            return null;
        }
    }

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
