// See https://aka.ms/new-console-template for more information

using ConsoleDemo.Settings;

Console.Clear();

Console.WriteLine("Starting App...");

Console.WriteLine("=== Settings demo ===");

SettingsManager.Initialize();

var app = SettingsManager.App.Settings;
var creds = SettingsManager.Credentials.Settings;

Console.WriteLine($"App theme: {app.Theme}");
Console.WriteLine($"Launch count: {app.LaunchCount}");
Console.WriteLine($"Stored API key (decrypted): {creds.ApiKey}");

app.LaunchCount++;
app.Theme = app.Theme == "Light" ? "Dark" : "Light";

Console.Write("Enter new API key (or leave empty to keep): ");
var input = Console.ReadLine();
if (!string.IsNullOrWhiteSpace(input))
    creds.ApiKey = input;

Console.WriteLine("Settings changed. Auto-save triggered via PropertyChanged.");
Thread.Sleep(500);
Console.WriteLine("Restart the app to see persisted values.");

Console.WriteLine("Press any key to exit...");
Console.ReadKey();
