using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Security.Cryptography;
using System.Text;
using TFG_Projects_APP_Frontend.Entities.Dtos.Users;
using TFG_Projects_APP_Frontend.Rest;
using TFG_Projects_APP_Frontend.Services.UsersService;
using static System.Net.WebRequestMethods;

namespace TFG_Projects_APP_Frontend.PageModels;

public partial class LoginPageModel : ObservableObject
{
    private readonly IUsersService usersService;
    private readonly UserSession userSession;
    private readonly RestClient restClient;

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

    public LoginPageModel(IUsersService usersService, UserSession userSession, RestClient restClient)
    {
        this.usersService = usersService;
        this.userSession = userSession;
        this.restClient = restClient;
    }

    [RelayCommand]
    public async Task DebugLogin()
    {
        await Shell.Current.GoToAsync("//MainPage");
    }


    [RelayCommand]
    public async Task Login()
    {
        if (ApiFound == false)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Please test the connection first", "OK");
            return;
        }
        IsLoading = true;
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Username))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Please enter all data", "OK");
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
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid credentials", "OK");
            }
        }
        IsLoading = false;
    }

    [RelayCommand]
    public async Task Register()
    {
        if (ApiFound == false)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Please test the connection first", "OK");
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
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Invalid credentials", "OK");
        }
        IsLoading = false;
    }

    [RelayCommand]
    public async Task TestConnection()
    {
        ShowTestButton = false;
        IsTesting = true;
        ApiFound = false;
        if (string.IsNullOrWhiteSpace(Route) || string.IsNullOrWhiteSpace(Port))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid route and port", "OK");
        } else
        {
            var testUrl = $"http://{Route}:{Port}/connection/test";
            var result = await restClient.TestConnection(testUrl);
            if (result == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "API not found", "OK");
            } else
            {
                if (result.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "API found", "OK");
                    ApiFound = true;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Could not connect", "OK");
                }
            }
        }   
        IsTesting = false;
        ShowTestButton = true;
    }

    [RelayCommand]
    public async Task ChangeRoute()
    {
        if (ApiFound)
        {
            Preferences.Set("APIurl", $"http://{Route}:{Port}");
            await Application.Current.MainPage.DisplayAlert("Success", "API URL updated successfully", "OK");
        } else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "API not found, please test the connection first", "OK");
        }
    }
}
