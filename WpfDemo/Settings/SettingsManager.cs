#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <                  SettingsKit                  |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: WpfDemo                                                           *
*            Filename: SettingsManager.cs                                                *
*              Author: Stanley Omoregie                                                  *
*        Created Date: 08.02.2026                                                        *
*       Modified Date: 08.02.2026                                                        *
*          Created By: Stanley Omoregie                                                  *
*    Last Modified By: Stanley Omoregie                                                  *
*           CopyRight: copyright © 2026 Novomatic AG.                                    *
*                  .oooO  Oooo.                                                          *
*                  (   )  (   )                                                          *
* ------------------\ (----) /---------------------------------------------------------- *
*                    \_)  (_/                                                            *
*****************************************************************************************/

#endregion copyright

using System.IO;
using SettingsKit.Core;
using WpfDemo.Models;
using WpfDemo.Settings.Migrations;

namespace WpfDemo.Settings;

public static class SettingsManager
{
    public static SettingsService<UiModel> UiSet { get; private set; } = null!;
    public static SettingsService<WinPosModel> UiPos { get; private set; } = null!;
    public static SettingsService<CredentialModel> Credentials { get; private set; } = null!;

    public static void Initialize()
    {
        var baseFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "WpfDemo");
        
        UiSet = new SettingsService<UiModel>(
            Path.Combine(baseFolder, "ui.json"),
            currentVersion: 2);
        
        UiPos = new SettingsService<WinPosModel>(
            Path.Combine(baseFolder, "winPos.json"),
            currentVersion: 2);

        UiSet.AddMigration(new UiSettingsMigration_1_to_2());

        Credentials = new SettingsService<CredentialModel>(
            Path.Combine(baseFolder, "credentials.json"),
            currentVersion: 1);
    }
}
