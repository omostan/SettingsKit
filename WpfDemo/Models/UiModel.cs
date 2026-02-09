#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <                  SettingsKit                  |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: WpfDemo                                                           *
*            Filename: UiModel.cs                                                        *
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

using SettingsKit.Core;
using SettingsKit.Security;

namespace WpfDemo.Models;

public class UiModel : ObservableObject
{
    private double _windowWidth = 800;
    public double WindowWidth
    {
        get => _windowWidth;
        set => SetProperty(ref _windowWidth, value, nameof(WindowWidth));
    }

    private double _windowHeight = 600;
    public double WindowHeight
    {
        get => _windowHeight;
        set => SetProperty(ref _windowHeight, value, nameof(WindowHeight));
    }

    private string _theme = "Light";
    
    [Encrypted]
    public string Theme
    {
        get => _theme;
        set => SetProperty(ref _theme, value, nameof(Theme));
    }
}
