#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <                  SettingsKit                  |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: SettingsKit                                                       *
*            Filename: ObservableObject.cs                                               *
*              Author: Stanley Omoregie                                                  *
*        Created Date: 07.02.2026                                                        *
*       Modified Date: 07.02.2026                                                        *
*          Created By: Stanley Omoregie                                                  *
*    Last Modified By: Stanley Omoregie                                                  *
*           CopyRight: copyright © 2026 Novomatic AG.                                    *
*                  .oooO  Oooo.                                                          *
*                  (   )  (   )                                                          *
* ------------------\ (----) /---------------------------------------------------------- *
*                    \_)  (_/                                                            *
*****************************************************************************************/

#endregion copyright

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SettingsKit.Core;

public class ObservableObject: INotifyPropertyChanged
{
    #region Properties

    private int? _version = 1;

    /// <summary> /// Gets or sets the schema version of the settings file.
    /// Used for automatic migration.
    /// </summary>
    public int? Version
    {
        get => _version;
        set => SetProperty(ref _version, value, nameof(Version));
    }

    #endregion Properties
    
    #region SetProperty
    
    protected virtual bool SetProperty<T>(
        ref T backingStore, T value,
        [CallerMemberName] string propertyName = "",
        Action? onChanged = null,
        Func<T, T, bool>? validateValue = null)
    {
        //if value didn't change
        if (EqualityComparer<T>.Default.Equals(backingStore, value))
            return false;

        //if value changed but didn't validate
        if (validateValue != null && !validateValue(backingStore, value))
            return false;

        backingStore = value;
        onChanged?.Invoke();
        OnPropertyChanged(propertyName);
        return true;
    }

    #endregion

    #region PropertyChanged

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    /// <remarks>
    /// This event is part of the <see cref="INotifyPropertyChanged"/> interface and is used by 
    /// WPF data binding to detect changes in property values and update the UI accordingly.
    /// </remarks>
    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

    #region OnPropertyChanged

    /// <summary>
    /// Raises the property changed event.
    /// </summary>
    /// <param name="propertyName">Property name.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    #endregion
}