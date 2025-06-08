using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Globalization;
using TFG_Projects_APP_Frontend.Properties;
using TFG_Projects_APP_Frontend.Services.UsersService;
using TFG_Projects_APP_Frontend.Utils;

namespace TFG_Projects_APP_Frontend.PageModels;

public partial class AppSettingsPageModel : ObservableObject
{
    private readonly UserSession userSession;

    [ObservableProperty]
    Language _selectedLanguage;

    [ObservableProperty]
    string _aPIUrl;

    [ObservableProperty]
    string _aPIPort;

    [ObservableProperty]
    string _savedMessage = "";

    [ObservableProperty]
    ObservableCollection<Language> languages = new ObservableCollection<Language>
    {
        new() {DisplayName = "English", LanguageCode = "en"},
        new() {DisplayName = "Español", LanguageCode = "es"}
    };

    public AppSettingsPageModel(UserSession userSession)
    {

        this.userSession = userSession;
    }


    [RelayCommand]
    private async Task LanguageSelected()
    {
        var culture = new CultureInfo(SelectedLanguage.LanguageCode);
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

        Preferences.Set("AppLanguage", SelectedLanguage.LanguageCode);

        await Application.Current.MainPage.DisplayAlert("Error", Resources.RestartMessage, "OK");
    }
}
