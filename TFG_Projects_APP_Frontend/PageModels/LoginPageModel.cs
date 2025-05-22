using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Security.Cryptography;
using System.Text;
using TFG_Projects_APP_Frontend.Entities.Dtos.Users;
using TFG_Projects_APP_Frontend.Services.UsersService;

namespace TFG_Projects_APP_Frontend.PageModels;

public partial class LoginPageModel : ObservableObject
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
    private string _route;

    [ObservableProperty]
    private string _port;

    public LoginPageModel(IUsersService usersService, UserSession userSession)
    {
        this.usersService = usersService;
        this.userSession = userSession;
    }

    [RelayCommand]
    public async Task Login()
    {
        IsLoading = true;

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
        } else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Invalid credentials", "OK");
        }
        IsLoading = false;
    }

    [RelayCommand]
    public async Task Register()
    {
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
        //TODO: Implement test connection
    }

    [RelayCommand]
    public async Task ChangeRoute()
    {
        if (string.IsNullOrEmpty(Route) || string.IsNullOrEmpty(Port))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid route and port", "OK");
            return;
        }
        Preferences.Set("APIurl", $"{Route}:{Port}");
        await Application.Current.MainPage.DisplayAlert("Success", "API URL updated successfully", "OK");
    }
}
