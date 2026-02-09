#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <                  SettingsKit                  |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: SettingsKit                                                       *
*            Filename: ISettingsMigration.cs                                             *
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

namespace SettingsKit.Core;

/// <summary>
/// Defines a migration step for upgrading settings between schema versions.
/// </summary>
public interface ISettingsMigration<T> where T : ObservableObject
{
    int FromVersion { get; }
    
    int ToVersion { get; }
    
    /// <summary>
    /// Performs the migration from <see cref="FromVersion"/> to <see cref="ToVersion"/>.
    /// </summary>
    T Migrate(T oldSettings);
}
