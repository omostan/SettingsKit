# Contributing to SettingsKit

Thank you for your interest in contributing to SettingsKit! We welcome contributions from the community and appreciate your help in making this project better.

## Code of Conduct

Please be respectful and constructive in all interactions. We're committed to providing a welcoming and inclusive environment for all contributors.

## Getting Started

### Prerequisites
- .NET 6.0 SDK or later
- Visual Studio, Visual Studio Code, or JetBrains Rider
- Git

### Setting Up Your Development Environment

1. **Fork the repository**
   ```bash
   git clone https://github.com/your-username/SettingsKit.git
   cd SettingsKit
   ```

2. **Create a feature branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

4. **Run tests**
   ```bash
   dotnet test
   ```

## How to Contribute

### Reporting Bugs

Before creating a bug report, please check the issue tracker to see if the problem has already been reported. If you find a duplicate, you can add a comment to the existing issue.

When filing a bug report, include:
- A clear, descriptive title
- A detailed description of the problem
- Steps to reproduce the issue
- Expected behavior vs. actual behavior
- Your environment (OS, .NET version, IDE)
- Any relevant code samples or error messages

### Suggesting Enhancements

We welcome suggestions for new features and improvements! Please include:
- A clear, descriptive title
- A detailed description of the suggested feature
- Why you believe this enhancement would be useful
- Possible alternative implementations (if applicable)

### Submitting Pull Requests

1. **Fork and branch**: Create a feature branch from `main`
   ```bash
   git checkout -b feature/descriptive-name
   ```

2. **Make your changes**: Follow the code style and conventions below

3. **Test your changes**: Ensure all tests pass and add new tests for new functionality
   ```bash
   dotnet test
   ```

4. **Commit with clear messages**
   ```bash
   git commit -m "Add feature: clear description of changes"
   ```

5. **Push to your fork**
   ```bash
   git push origin feature/descriptive-name
   ```

6. **Open a Pull Request**: Provide a clear description of your changes and reference any related issues

## Code Style and Conventions

- **Language**: C#
- **Framework**: .NET 6.0+
- **Naming**: Follow Microsoft C# coding conventions
- **Formatting**: Use consistent indentation (4 spaces)
- **Null Safety**: Enable nullable reference types and handle nullability appropriately
- **Documentation**: Add XML documentation comments to public APIs
- **Tests**: Write unit tests for new features using xUnit, NUnit, or MSTest

### Example Code Style

```csharp
public class MyClass
{
    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Performs an action.
    /// </summary>
    /// <param name="input">The input value.</param>
    /// <returns>The result.</returns>
    public string DoSomething(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentException("Input cannot be null or empty.", nameof(input));
        }

        return input.ToUpperInvariant();
    }
}
```

## Development Workflow

1. Keep your fork synchronized with the main repository
   ```bash
   git fetch upstream
   git rebase upstream/main
   ```

2. Make focused commits for related changes
   - One feature/fix per commit when possible
   - Write clear commit messages explaining the "why" behind changes

3. Ensure your code builds without warnings
   ```bash
   dotnet build
   ```

4. Run the full test suite before submitting a PR
   ```bash
   dotnet test
   ```

## Project Structure

```
SettingsKit/
├── SettingsKit/              # Main library
│   ├── Core/                 # Core functionality (SettingsService, ObservableObject, etc.)
│   └── Security/             # Encryption and security features
├── ConsoleDemo/              # Console application example
├── WpfDemo/                  # WPF desktop application example
└── [configuration files]     # Build, documentation, and CI/CD configs
```

## Documentation

- **Code Comments**: Use clear, concise comments to explain complex logic
- **Commit Messages**: Use descriptive messages that explain the "what" and "why"
- **Pull Request Descriptions**: Provide context and reasoning for your changes
- **README Updates**: Update relevant documentation when adding features

## Release Process

Releases are managed by the project maintainers. When a release is planned:
1. Update `Directory.Build.props` with the new version number
2. Update `CHANGELOG.md` with release notes
3. Create a GitHub release with documentation
4. Push to NuGet package registry

## Questions or Need Help?

- Check the existing documentation and examples
- Review closed issues and discussions for similar questions
- Open a GitHub Discussion for general questions
- Open an Issue for bugs or feature requests

## License

By contributing to SettingsKit, you agree that your contributions will be licensed under the MIT License.

---

Thank you for contributing to SettingsKit! Your efforts help make this project better for everyone. 🎉

