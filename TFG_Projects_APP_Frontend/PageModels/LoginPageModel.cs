using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Security.Cryptography;
using System.Text;
using TFG_Projects_APP_Frontend.Entities.Dtos.Users;
using TFG_Projects_APP_Frontend.Properties;
using TFG_Projects_APP_Frontend.Rest;
using TFG_Projects_APP_Frontend.Services.UsersService;

namespace TFG_Projects_APP_Frontend.PageModels;

public partial class LoginPageModel : ObservableObject
{
    private readonly IUsersService usersService;
    private readonly UserSession userSession;
    private readonly RestClient restClient;

    private bool ApiSaved;

    [ObservableProperty]
    private string _email;

    [ObservableProperty]
    private string _username;

    [ObservableProperty]
    private string _password;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _showTestButton = true;

    [ObservableProperty]
    private bool _isTesting;

    [ObservableProperty]
    private string _route;

    [ObservableProperty]
    private string _port;

    [ObservableProperty]
    private bool _apiFound = false;

    [ObservableProperty]
    private bool _rememberMe = false;

    public LoginPageModel(IUsersService usersService, UserSession userSession, RestClient restClient)
    {
        this.usersService = usersService;
        this.userSession = userSession;
        this.restClient = restClient;
    }

    /*When a user navigates to the page, resets the relevant properties. Additionally, sets remember me to false, as navigating to this page is akin to a log out*/
    public async Task OnNavigatedTo()
    {
        ApiSaved = false;
        ApiFound = true;
        Email = string.Empty;
        Username = string.Empty;
        Password = string.Empty;
        Route = string.Empty;
        Port = string.Empty;

        Preferences.Set("RememberMe", false);
    }

    /*Debug method to skip login TODO: Remove*/
    [RelayCommand]
    public async Task DirectLogin()
    {
        Application.Current.Dispatcher.Dispatch(async () =>
        {
            await Task.Delay(50);
            await Shell.Current.GoToAsync("//MainPage");
        });
    }

    /*Checks if the user is allowed to log in, then calls the authenticate method in the UserService, and if it gets a sucecesful response, it takes the user to the main page.
     * Additionally, if the user sets remember me, updates the Preferences to allow for a direct login next time the user opens the app.
     */
    [RelayCommand]
    public async Task Login()
    {
        if (ApiSaved == false)
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.ConnectionTestMessage, Resources.ConfirmButton);
            return;
        }
        IsLoading = true;
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Username))
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.EnterAllDataMessage, Resources.ConfirmButton);
            IsLoading = false;
            
        } else
        {
            var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(Password);
            var hash = sha256.ComputeHash(bytes);
            var password = Convert.ToHexString(hash);

            var userAuth = new UserAuthenticate
            {
                Email = Email,
                Username = Username,
                Password = password
            };
            var result = await usersService.AuthenticateUser(userAuth);

            if (result != null)
            {
                userSession.User = result;
                if (RememberMe)
                {
                    Preferences.Set("RememberMe", true);
                    Preferences.Set("UserId", result.Id);
                    Preferences.Set("Username", result.Username);
                    Preferences.Set("Email", result.Email);
                    Preferences.Set("Token", userSession.Token);
                }
                else
                {
                    Preferences.Set("RememberMe", false);
                }
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.InvalidCredentialsMessage, Resources.ConfirmButton);
            }
        }
        IsLoading = false;
    }

    /*Checks if the user is allowed to register, then creates a new user, then takes the user to the main page like a normal log in*/
    [RelayCommand]
    public async Task Register()
    {
        if (ApiSaved == false)
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.ConnectionTestMessage, Resources.ConfirmButton);
            return;
        }
        IsLoading = true;
        var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(Password);
        var hash = sha256.ComputeHash(bytes);
        var password = Convert.ToHexString(hash);

        var user = new UserCreate
        {
            Email = Email,
            Username = Username,
            Password = password
        };
        var result = await usersService.Post(user);
        if (result != null)
        {
            userSession.User = result;
            if (RememberMe)
            {
                Preferences.Set("RememberMe", true);
                Preferences.Set("UserId", result.Id);
                Preferences.Set("Username", result.Username);
                Preferences.Set("Email", result.Email);
                Preferences.Set("Token", userSession.Token);
            }
            else
            {
                Preferences.Set("RememberMe", false);
            }
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.InvalidCredentialsMessage, Resources.ConfirmButton);
        }
        IsLoading = false;
    }

    /*Tests the connection to the API to determine whether the user should be allowed to log in or register*/
    [RelayCommand]
    public async Task TestConnection()
    {
        ShowTestButton = false;
        IsTesting = true;
        ApiFound = false;
        if (string.IsNullOrWhiteSpace(Route) || string.IsNullOrWhiteSpace(Port))
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.APIValidationMessage, Resources.ConfirmButton);
        } else
        {
            var testUrl = $"http://{Route}:{Port}/connection/test";
            var result = await restClient.TestConnection(testUrl);
            if (result == null)
            {
                await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.APINotFoundMessage, Resources.ConfirmButton);
            } else
            {
                if (result.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert(Resources.SuccessMessageTitle, Resources.APIFoundMessage, Resources.ConfirmButton);
                    ApiFound = true;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.CouldNotConnectMessage, Resources.ConfirmButton);
                }
            }
        }   
        IsTesting = false;
        ShowTestButton = true;
    }

    /*Saves the API Url so that it can be used by the RestClient*/
    [RelayCommand]
    public async Task ChangeRoute()
    {
        if (ApiFound)
        {
            Preferences.Set("APIurl", $"http://{Route}:{Port}");
            ApiSaved = true;
            await Application.Current.MainPage.DisplayAlert(Resources.SuccessMessageTitle, Resources.APIUpdatedMessage, Resources.ConfirmButton);
        } else
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.APINotFoundTestConnectionMessage, Resources.ConfirmButton);
        }
    }
}
