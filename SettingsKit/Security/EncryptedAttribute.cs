#region copyright
 
/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <                  SettingsKit                  |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: SettingsKit                                                       *
*            Filename: EncryptedAttribute.cs                                             *
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

namespace SettingsKit.Security;

/// <summary>
/// Marks a property as requiring encryption in settings storage.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class EncryptedAttribute : Attribute;