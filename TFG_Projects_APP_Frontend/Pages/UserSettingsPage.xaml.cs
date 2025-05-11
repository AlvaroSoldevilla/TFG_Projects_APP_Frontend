using TFG_Projects_APP_Frontend.PageModels;

namespace TFG_Projects_APP_Frontend.Pages;

public partial class UserSettingsPage : ContentPage
{
	public UserSettingsPage(UserSettingsPageModel userSettingsPageModel)
	{
		InitializeComponent();
        BindingContext = userSettingsPageModel;
    }
}