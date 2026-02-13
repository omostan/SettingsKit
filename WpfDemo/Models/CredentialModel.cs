#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <                  SettingsKit                  |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: WpfDemo                                                           *
*            Filename: CredentialModel.cs                                                *
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
using SettingsKit.Security;

namespace WpfDemo.Models;

public class CredentialModel : ObservableObject
{
    private string _apiKey = string.Empty;

    [Encrypted]
    public string ApiKey
    {
        get => _apiKey;
        set => SetProperty(ref _apiKey, value, nameof(ApiKey));
    }
}
