#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <                  SettingsKit                  |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: WpfDemo                                                           *
*            Filename: WinPosModel.cs                                                    *
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

namespace WpfDemo.Models;

public class WinPosModel : ObservableObject
{
    private double _windowTop;
    public double WindowTop
    {
        get => _windowTop;
        set => SetProperty(ref _windowTop, value, nameof(WindowTop));
    }

    private double _windowLeft;
    public double WindowLeft
    {
        get => _windowLeft;
        set => SetProperty(ref _windowLeft, value, nameof(WindowLeft));
    }
}