# SettingsKit Solution

[![NuGet](https://img.shields.io/nuget/v/Omostan.SettingsKit.svg)](https://www.nuget.org/packages/Omostan.SettingsKit/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Omostan.SettingsKit.svg)](https://www.nuget.org/packages/Omostan.SettingsKit/)
[![.NET](https://img.shields.io/badge/.NET-10.0-blue.svg)](https://dotnet.microsoft.com/download)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

**SettingsKit** is a lightweight, MVVM‑friendly, version‑tolerant JSON settings library for .NET applications.  
It provides automatic persistence, schema migration, backup/restore, and optional encryption for sensitive fields — all with a clean, strongly‑typed API.

Perfect for WPF, WinUI, Avalonia, MAUI, console apps, and any .NET project that needs reliable user or app settings.

---

## 📦 Installation

Install via NuGet:

```bash
Install-Package Omostan.SettingsKit
```
or .NET CLI:

```bash
dotnet add package Omostan.SettingsKit
```

---

## Repository Structure

```
SettingsKit/
│
├── SettingsKit/
│   ├── Core/
│   │   ├── ObservableObject.cs
│   │   ├── SettingsService.cs
│   │   ├── ISettingsMigration.cs
│   │
│   ├── Security/
│   │   ├── EncryptedAttribute.cs
│   │   ├── EncryptionHelper.cs
│   │
│   ├── SettingsKit.csproj
│
├── ConsoleDemo/
│       ├── Program.cs
│       ├── Settings/
│       │   ├── AppSettings.cs
│       │   ├── AppSettingsMigration_1_to_2.cs
│       │   ├── CredentialSettings.cs
│       │   └── SettingsManager.cs
│       │   
│       ├── ConsoleDemo.csproj
│       
├── WpfDemo/
│       ├── App.xaml
│       ├── App.xaml.cs
│       ├── MainWindow.xaml
│       ├── MainWindow.xaml.cs
│       ├── Commands/
│       │   └── RelayCommand.cs
│       ├── Helpers/
│       │   └── PasswordBoxHelper.cs
│       ├── Models/
│       │   └── CredentialModel.cs
│       │   └── UiModel.cs
│       │   └── WinPosModel.cs
│       ├── Settings/
│       │   ├── migrations/
│       │   │   └── UiSettingsMigration_1_to_2.cs
│       │   └── SettingsManager.cs
│       ├── ViewModels/
│       │   └── BaseViewModel.cs
│       │   └── MainViewModel.cs
│       │   
│       ├── WpfDemo.csproj
│
├── .github/
│   ├── workflows/
│   │   └── publish.yml
│   └── ISSUE_TEMPLATE.md
│
├── README.md
├── CHANGELOG.md
├── CONTRIBUTING.md
├── LICENSE
└── Directory.Build.props

```

---

## ✨ Features

- **Strongly‑typed settings models**
- **MVVM‑friendly** (`INotifyPropertyChanged` built in)
- **Automatic JSON persistence**
- **Auto‑save on property change**
- **Version‑aware schema migration**
- **Automatic backup & restore**
- **Optional DPAPI encryption** for sensitive fields
- **Generic SettingsService<T>** for unlimited settings groups
- **Zero dependencies** (pure .NET)

---

## 🚀 Quick Start

### 1. Create a settings model

```csharp
public class UiSettings : SettingsBase
{
    public double WindowWidth { get; set; } = 800;
    public double WindowHeight { get; set; } = 600;
}
```

### 2. Initialize the service and bind directly in MVVM

```csharp
public class SomeViewModel : ViewModelBase
{
    var uiSettings = new SettingsService<UiSettings>(
        filePath: "ui.json",
        currentVersion: 1
    );
    
    public UiSettings Ui => uiSettings.Settings;
    
    --- other properties and logic ---
    --- whereby ViewModelBase implements INotifyPropertyChanged and raises notifications when UiSettings properties change ---
}
```

### 3. Use in XAML

```xaml
<Window ...>
    <StackPanel>
        <TextBox Text="{Binding Ui.WindowWidth, UpdateSourceTrigger=PropertyChanged}" />
        <TextBox Text="{Binding Ui.WindowHeight, UpdateSourceTrigger=PropertyChanged}" />
    </StackPanel>
</Window>
```

### 4. Auto‑save happens automatically

Whenever `WindowWidth` or `WindowHeight` changes, the settings are automatically saved to `ui.json` with the new values.
No need to call save() methods manually - SettingsKit listens for property changes and handles persistence seamlessly.

## 🛠️ Advanced Usage
- **Migrations**: Implement `ISettingsMigration` to handle schema changes across versions.
- **Encryption**: Use `[Encrypted]` attribute on properties to automatically encrypt/decrypt sensitive data using DPAPI.
- **Multiple Settings Groups**: Create multiple `SettingsService<T>` instances for different settings categories (e.g. `UserSettings`, `AppSettings`).
- **Custom Paths**: Configure custom file paths and directories for settings storage.
- **Backup & Restore**: Use built‑in backup and restore methods to safeguard user settings.
- **Error Handling**: Customize error handling and logging for settings operations.
- **Cross‑Platform**: Works seamlessly on Windows, Linux, and macOS with .NET Core/5+/6+.
- **Unit Testing**: Easily mock `SettingsService<T>` for unit testing your view models and services.
- **Performance**: Optimized for fast load/save operations even with large settings models.
- **Extensibility**: Extend core functionality by inheriting from `SettingsBase` and implementing custom logic as needed.

🔄 Schema Migration

Add migration steps when your settings evolve:

```csharp
public class UiSettingsMigration_1_to_2 : ISettingsMigration<UiSettings>
{
    public int FromVersion => 1;
    public int ToVersion => 2;

    public UiSettings Migrate(UiSettings old)
    {
        old.WindowHeight = 600;
        old.Version = 2;
        return old;
    }
}
```

### Register it:

```csharp
uiSettings.AddMigration(new UiSettingsMigration_1_to_2());
```

🔐 Encrypt Sensitive Settings

```csharp
public class Credentials : SettingsBase
{
    [Encrypted]
    public string ApiKey { get; set; }
}
```
SettingsKit automatically encrypts on save and decrypts on load.

🛟 Backup & Restore

SettingsKit automatically creates a .bak file before saving.
If the main file becomes corrupted, it restores from backup.

## Build, Package and Publish to NuGet

1. Build the project in Release mode.

```bash
cd /d/Tutorials/SettingsKit && dotnet build SettingsKit/SettingsKit.csproj -c Release
```

2. Create the NuGet package:

```bash
cd /d/Tutorials/SettingsKit && dotnet clean SettingsKit/SettingsKit.csproj && dotnet pack SettingsKit/SettingsKit.csproj -c Release --output ./nupkg
```
3. Publish to NuGet:

```bash
cd /d/Tutorials/SettingsKit && dotnet nuget push ./nupkg/Omostan.SettingsKit.1.0.x.nupkg -k YOUR_NUGET_API_KEY -s https://api.nuget.org/v3/index.json
```

### 📦 **NuGet Package Description (Guide)**

#### **Short Description (NuGet summary field)**

Strongly‑typed, MVVM‑friendly JSON settings library with auto‑save, migration, backup, and encryption.

#### **Long Description (NuGet description field)**

SettingsKit is a lightweight, strongly‑typed JSON settings library for .NET applications.
It provides automatic persistence, MVVM‑friendly change tracking, version‑aware schema migration,
backup/restore, and optional DPAPI encryption for sensitive fields.

Ideal for WPF, WinUI, Avalonia, MAUI, and console applications that need reliable user or app settings.

Key features include:
- Strongly‑typed settings models
- Auto‑save on property change
- Version‑aware schema migration
- Automatic backup & restore
- Optional encryption for sensitive data
- Zero dependencies (pure .NET)

## 🤝 Contributing

Contributions and pull requests are welcome!
If you have ideas for improvements or new features, feel free to open an issue.