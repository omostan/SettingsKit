# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.6] - 2026-02-13

### CopyRight Correction
- Version bump release with no functionality changes but with copyright correction
- All features and fixes from v1.0.3 remain intact

---

## [1.0.5] - 2026-02-13

### CopyRight Correction
- Version bump release with no functionality changes but with copyright correction
- All features and fixes from v1.0.3 remain intact

---

## [1.0.4] - 2026-02-11

### No Code Changes
- Version bump release with no functionality changes
- All features and fixes from v1.0.3 remain intact

---

## [1.0.3] - 2026-02-11

### Fixed
- Improved null reference handling in edge cases during migration
- Fixed potential resource leak in file backup operations
- Enhanced error recovery for incomplete JSON files
- Resolved timing issues in rapid consecutive save operations
- Fixed incorrect exception type thrown in validation scenarios
- Improved handling of special characters in file paths

### Improved
- Memory usage in large settings objects
- Allocations in property change detection
- Synchronization context detection performance
- Backup file cleanup efficiency
- XML documentation comments for better IntelliSense
- Exception messages with actionable guidance
- Debugging experience with more descriptive log messages
- Validation error messages for common mistakes

### Security
- Updated encryption validation to be more strict
- Improved DPAPI key caching mechanisms
- Better handling of encryption context across AppDomains

### Documentation
- Enhanced README with troubleshooting section improvements
- Added more code examples for edge cases
- Improved API documentation with additional remarks
- Better guidance on async/await patterns with settings

### Compatibility
- Verified compatibility with .NET 6.0, 7.0, 8.0, and 9.0
- Improved compatibility with various WPF framework versions
- Enhanced support for non-Windows platforms
- Better compatibility with different file system configurations

---

## [1.0.2] - 2026-02-10

### Fixed
- **Critical: Cross-thread access violation in WPF applications** - Added SynchronizationContext support for automatic UI thread marshalling during save operations
- File encoding issues when saving settings on non-UTF8 systems
- Thread-safety concerns in concurrent access scenarios
- Error handling for corrupted JSON files with automatic recovery
- Settings not loading properly on first initialization
- Migration path validation errors in edge cases

### Improved
- JSON deserialization performance for faster startup times
- Memory allocations during settings updates
- File I/O efficiency with better buffering strategies
- Backup mechanism to reduce disk space usage
- Exception messages for easier troubleshooting
- Logging output for debugging settings issues

### Security
- Enhanced DPAPI encryption key handling across different user contexts
- Better validation of encrypted data to prevent tampering
- Improved file permission checks for settings files

### Documentation
- Fixed typos and clarifications in README
- Enhanced API documentation
- Added troubleshooting section for common issues

---

## [1.0.1] - 2026-02-10

### Fixed
- Minor bug fixes from initial release
- Documentation improvements
- Example code enhancements

---

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

