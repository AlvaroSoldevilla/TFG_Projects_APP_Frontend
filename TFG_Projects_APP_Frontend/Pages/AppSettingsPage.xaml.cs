using TFG_Projects_APP_Frontend.PageModels;

namespace TFG_Projects_APP_Frontend.Pages;

public partial class AppSettingsPage : ContentPage
{
	public AppSettingsPage(AppSettingsPageModel appSettingsPageModel)
	{
		InitializeComponent();
        BindingContext = appSettingsPageModel;
    }
}