#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <                  SettingsKit                  |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: ConsoleDemo                                                       *
*            Filename: SettingsManager.cs                                                *
*              Author: Stanley Omoregie                                                  *
*        Created Date: 09.02.2026                                                        *
*       Modified Date: 09.02.2026                                                        *
*          Created By: Stanley Omoregie                                                  *
*    Last Modified By: Stanley Omoregie                                                  *
*           CopyRight: copyright © 2026 Omotech Digital Solutions                        *
*                  .oooO  Oooo.                                                          *
*                  (   )  (   )                                                          *
* ------------------\ (----) /---------------------------------------------------------- *
*                    \_)  (_/                                                            *
*****************************************************************************************/

#endregion copyright

using SettingsKit.Core;

namespace ConsoleDemo.Settings;

public class SettingsManager
{
    public static SettingsService<AppSettings> App { get; private set; } = null!;
    public static SettingsService<CredentialsSettings> Credentials { get; private set; } = null!;

    public static void Initialize()
    {
        var baseFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "DemoSettings");
        
        App = new SettingsService<AppSettings>(
            Path.Combine(baseFolder, "app.json"),
            currentVersion: 2);
        
        App.AddMigration(new AppSettingsMigration_1_to_2());

        Credentials = new SettingsService<CredentialsSettings>(
            Path.Combine(baseFolder, "credentials.json"),
            currentVersion: 1);
    }
}