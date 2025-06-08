using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Security.Cryptography;
using System.Text;
using TFG_Projects_APP_Frontend.Entities.Dtos.Users;
using TFG_Projects_APP_Frontend.Properties;
using TFG_Projects_APP_Frontend.Services.UsersService;

namespace TFG_Projects_APP_Frontend.PageModels;

public partial class UserSettingsPageModel : ObservableObject
{
    private readonly IUsersService usersService;
    private readonly UserSession userSession;

    [ObservableProperty]
    private string _email;

    [ObservableProperty]
    private string _username;

    [ObservableProperty]
    private string _password;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    public bool _isLoggedIn;

    [ObservableProperty]
    private string _newPassword;

    [ObservableProperty]
    private string _newPasswordConfirm;

    [ObservableProperty]
    private string _newUsername;

    [ObservableProperty]
    private string _newEmail;

    public UserSettingsPageModel(IUsersService usersService, UserSession userSession)
    {
        this.usersService = usersService;
        this.userSession = userSession;
    }

    [RelayCommand]
    public async Task Login()
    {
        IsLoading = true;
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Username))
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.EnterAllDataMessage, Resources.ConfirmButton);
            IsLoading = false;
        }
        else
        {
            var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(Password);
            var hash = sha256.ComputeHash(bytes);
            var password = Convert.ToHexString(hash);

            if (Email != userSession.User.Email || Username != userSession.User.Username)
            {
                await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.InvalidCredentialsMessage, Resources.ConfirmButton);
            }
            else
            {
                var userAuth = new UserAuthenticate
                {
                    Email = Email,
                    Username = Username,
                    Password = password
                };
                var result = await usersService.AuthenticateUser(userAuth);

                if (result != null)
                {
                    IsLoggedIn = true;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.InvalidCredentialsMessage, Resources.ConfirmButton);
                }
            }


        }

        Username = string.Empty;
        Password = string.Empty;
        Email = string.Empty;

        IsLoading = false;
    }

    [RelayCommand]
    private async Task ChangePassword()
    {
        if (NewPassword != null && NewPasswordConfirm != null)
        {
            if (NewPassword == NewPasswordConfirm)
            {
                var sha256 = SHA256.Create();
                var bytes = Encoding.UTF8.GetBytes(NewPassword);
                var hash = sha256.ComputeHash(bytes);
                var password = Convert.ToHexString(hash);

                var userUpdate = new UserUpdate
                {
                    Username = userSession.User.Username,
                    Email = userSession.User.Email,
                    Password = password
                };

                var result = await usersService.Patch(userSession.User.Id, userUpdate);

                if (result != null)
                {
                    NewPassword = string.Empty;
                    NewPasswordConfirm = string.Empty;
                    await Application.Current.MainPage.DisplayAlert(Resources.SuccessMessageTitle, Resources.PasswordUpdatedMessage, Resources.ConfirmButton);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.PasswordNotUpdatedMessage, Resources.ConfirmButton);
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.PasswordsMustMatchMessage, Resources.ConfirmButton);
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.PasswordNotIntroducedMessage, Resources.ConfirmButton);
        }
    }

    [RelayCommand]
    private async Task ChangeEmail()
    {
        if (NewEmail != null)
        {
            var userUpdate = new UserUpdate
            {
                Username = userSession.User.Username,
                Email = NewEmail,
                Password = null
            };

            var result = await usersService.Patch(userSession.User.Id, userUpdate);

            if (result != null)
            {
                NewEmail = string.Empty;
                await Application.Current.MainPage.DisplayAlert(Resources.SuccessMessageTitle, Resources.EmailUpdatedMessage, Resources.ConfirmButton);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.EmailNotUpdatedMessage, Resources.ConfirmButton);
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.EmailNotIntroducedMessage, Resources.ConfirmButton);
        }
    }

    [RelayCommand]
    private async Task ChangeUsername()
    {
        if (NewUsername != null)
        {
            var userUpdate = new UserUpdate
            {
                Username = NewUsername,
                Email = userSession.User.Email,
                Password = null
            };

            var result = await usersService.Patch(userSession.User.Id, userUpdate);

            if (result != null)
            {
                NewUsername = string.Empty;
                await Application.Current.MainPage.DisplayAlert(Resources.SuccessMessageTitle, Resources.UsernameUpdatedMessage, Resources.ConfirmButton);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.UsernameNotUpdatedMessage, Resources.ConfirmButton);
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.UsernameNotIntroducedMessage, Resources.ConfirmButton);
        }
    }
}
