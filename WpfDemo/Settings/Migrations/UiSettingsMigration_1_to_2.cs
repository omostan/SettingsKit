#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <                  SettingsKit                  |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: WpfDemo                                                           *
*            Filename: UiSettingsMigration_1_to_2.cs                                     *
*              Author: Stanley Omoregie                                                  *
*        Created Date: 08.02.2026                                                        *
*       Modified Date: 08.02.2026                                                        *
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
using WpfDemo.Models;

namespace WpfDemo.Settings.Migrations;

public class UiSettingsMigration_1_to_2 : ISettingsMigration<UiModel>
{
    public int FromVersion => 1;
    public int ToVersion => 2;

    public UiModel Migrate(UiModel old)
    {
        if (old.WindowHeight <= 0)
            old.WindowHeight = 600;

        old.Version = 2;
        return old;
    }
}
