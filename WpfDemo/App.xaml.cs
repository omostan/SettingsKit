using System.Windows;
using WpfDemo.Settings;

namespace WpfDemo;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        SettingsManager.Initialize();
    }
}