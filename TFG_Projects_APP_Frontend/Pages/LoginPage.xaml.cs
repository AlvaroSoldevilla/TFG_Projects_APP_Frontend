using TFG_Projects_APP_Frontend.PageModels;

namespace TFG_Projects_APP_Frontend.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPageModel ViewModel { get; set; }
    public LoginPage(LoginPageModel loginPageModel)
	{
		InitializeComponent();
        BindingContext = loginPageModel;
        ViewModel = loginPageModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        ViewModel?.OnNavigatedTo();
    }
}