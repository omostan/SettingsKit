#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <                  SettingsKit                  |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: WpfDemo                                                           *
*            Filename: MainViewModel.cs                                                  *
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

using System.Windows.Input;
using SettingsKit.Security;
using WpfDemo.Commands;
using WpfDemo.Models;
using WpfDemo.Settings;

namespace WpfDemo.ViewModels;

public sealed class MainViewModel : BaseViewModel
{
    #region Fields

    private string _apiKeyPassword = EncryptionHelper.TryDecrypt(Credentials.ApiKey);
    private bool _isPasswordVisible;

    #endregion Fields

    #region Commands

    public ICommand SaveApiKeyCommand => new RelayCommand(_ => SaveSettings());
    public ICommand TogglePasswordVisibilityCommand => new RelayCommand(_ => TogglePasswordVisibility());

    #endregion Commands

    #region Properties

    public string ApiKeyPassword
    {
        get => _apiKeyPassword;
        set => SetProperty(ref _apiKeyPassword, value, nameof(ApiKeyPassword));
    }

    public bool IsPasswordVisible
    {
        get => _isPasswordVisible;
        set => SetProperty(ref _isPasswordVisible, value, nameof(IsPasswordVisible));
    }
    
    public UiModel UiSet => SettingsManager.UiSet.Settings;
    public WinPosModel UiPos => SettingsManager.UiPos.Settings;
    private static CredentialModel Credentials => SettingsManager.Credentials.Settings;

    #endregion Properties

    #region SaveSettings

    private void SaveSettings()
    {
        Credentials.ApiKey = ApiKeyPassword;
    }

    #endregion SaveSettings

    #region TogglePasswordVisibility

    private void TogglePasswordVisibility()
    {
        IsPasswordVisible = !IsPasswordVisible;
    }

    #endregion TogglePasswordVisibility
}