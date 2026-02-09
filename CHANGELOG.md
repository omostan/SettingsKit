# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2026-02-09

### Added
- Core `SettingsService<T>` for strongly-typed settings management
- `ObservableObject` base class implementing `INotifyPropertyChanged` for MVVM scenarios
- `ISettingsMigration` interface for version-aware schema migrations
- Automatic JSON persistence to disk
- Auto-save on property change functionality
- `[Encrypted]` attribute for DPAPI-based encryption of sensitive data
- `EncryptionHelper` utility for encryption/decryption operations
- Automatic backup and restore functionality
- Multiple `SettingsService<T>` instances support for different settings groups
- Console demo application showing basic usage
- WPF demo application demonstrating MVVM integration
- Support for .NET 6.0 and later

### Features
- Strongly-typed settings models inheriting from `SettingsBase`
- MVVM-friendly with built-in `INotifyPropertyChanged` support
- Zero external dependencies (pure .NET)
- Cross-platform support (Windows, Linux, macOS)
- Custom file paths and directories configuration
- Error handling and logging capabilities
- Unit test friendly with mockable `SettingsService<T>`
- Performance optimized for large settings models
- Extensible architecture for custom implementations

### Documentation
- Comprehensive README with quick start guide
- Advanced usage documentation
- Contributing guidelines
- License (MIT)

### Examples
- Console application demonstrating core functionality
- WPF application showcasing MVVM integration with UI settings persistence

---

## Planned for Future Releases

### [1.1.0] - Planned
- Async I/O operations for improved performance with large datasets
- Settings validation framework
- Support for settings templates and presets
- Enhanced logging and diagnostics
- Additional encryption algorithms (AES, RSA)

### [1.2.0] - Planned
- Cloud sync capabilities
- Settings diff and merge utilities
- Performance profiling and optimization
- Extended platform support (MAUI, Avalonia)

---

## [0.1.0] - Initial Development
- Project structure established
- Core framework development
- Initial examples and demos

---

For more details about a specific version, see the [GitHub releases](https://github.com/SettingsKit/SettingsKit/releases).

