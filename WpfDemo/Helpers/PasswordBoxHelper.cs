#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <                  SettingsKit                  |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: WpfDemo                                                           *
*            Filename: PasswordBoxHelper.cs                                              *
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

using System.Windows;
using System.Windows.Controls;

namespace WpfDemo.Helpers;

public static class PasswordBoxHelper
{
    public static readonly DependencyProperty BoundPasswordProperty =
        DependencyProperty.RegisterAttached("BoundPassword",
            typeof(string),
            typeof(PasswordBoxHelper),
            new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

    public static readonly DependencyProperty BindPasswordProperty =
        DependencyProperty.RegisterAttached("BindPassword",
            typeof(bool),
            typeof(PasswordBoxHelper),
            new PropertyMetadata(false, OnBindPasswordChanged));

    private static readonly DependencyProperty UpdatingPasswordProperty =
        DependencyProperty.RegisterAttached("UpdatingPassword",
            typeof(bool),
            typeof(PasswordBoxHelper),
            new PropertyMetadata(false));

    public static string GetBoundPassword(DependencyObject d)
    {
        return (string)d.GetValue(BoundPasswordProperty);
    }

    public static void SetBoundPassword(DependencyObject d, string value)
    {
        d.SetValue(BoundPasswordProperty, value);
    }

    public static bool GetBindPassword(DependencyObject d)
    {
        return (bool)d.GetValue(BindPasswordProperty);
    }

    public static void SetBindPassword(DependencyObject d, bool value)
    {
        d.SetValue(BindPasswordProperty, value);
    }

    private static bool GetUpdatingPassword(DependencyObject d)
    {
        return (bool)d.GetValue(UpdatingPasswordProperty);
    }

    private static void SetUpdatingPassword(DependencyObject d, bool value)
    {
        d.SetValue(UpdatingPasswordProperty, value);
    }

    private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PasswordBox passwordBox)
            return;

        passwordBox.PasswordChanged -= PasswordChanged;

        if (!GetUpdatingPassword(passwordBox))
        {
            passwordBox.Password = (string)e.NewValue;
        }

        passwordBox.PasswordChanged += PasswordChanged;
    }

    private static void OnBindPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PasswordBox passwordBox)
            return;

        var wasBound = (bool)e.OldValue;
        var needToBind = (bool)e.NewValue;

        if (wasBound)
        {
            passwordBox.PasswordChanged -= PasswordChanged;
        }

        if (needToBind)
        {
            passwordBox.PasswordChanged += PasswordChanged;
        }
    }

    private static void PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (sender is not PasswordBox passwordBox)
            return;

        SetUpdatingPassword(passwordBox, true);
        SetBoundPassword(passwordBox, passwordBox.Password);
        SetUpdatingPassword(passwordBox, false);
    }
}

