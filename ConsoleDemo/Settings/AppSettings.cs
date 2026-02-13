#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <                  SettingsKit                  |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: ConsoleDemo                                                       *
*            Filename: AppSettings.cs                                                    *
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

public class AppSettings : ObservableObject
{
    private string _theme = "Light";

    public string Theme
    {
        get => _theme;
        set => SetProperty(ref _theme, value, nameof(Theme));
    }
    
    private int _launchCount;

    public int LaunchCount
    {
        get => _launchCount;
        set => SetProperty(ref _launchCount, value, nameof(LaunchCount));
    }
}