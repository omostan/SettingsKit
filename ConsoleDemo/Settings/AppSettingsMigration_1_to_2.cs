#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <                  SettingsKit                  |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: ConsoleDemo                                                       *
*            Filename: AppSettingsMigration_1_to_2.cs                                    *
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

public class AppSettingsMigration_1_to_2 : ISettingsMigration<AppSettings>
{
    public int FromVersion => 1;
    public int ToVersion => 2;
    
    public AppSettings Migrate(AppSettings old)
    {
        if (old.LaunchCount < 0)
            old.LaunchCount = 0;
        
        if (string.IsNullOrWhiteSpace(old.Theme))
            old.Theme = "Light";
        
        old.Version = 2;
        
        return old;
    }
}