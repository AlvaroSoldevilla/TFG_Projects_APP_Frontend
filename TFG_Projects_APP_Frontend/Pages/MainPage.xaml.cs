using TFG_Projects_APP_Frontend.PageModels;

namespace TFG_Projects_APP_Frontend.Pages;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageModel mainPageModel)
    {
        InitializeComponent();
        BindingContext = mainPageModel;
    }
}
