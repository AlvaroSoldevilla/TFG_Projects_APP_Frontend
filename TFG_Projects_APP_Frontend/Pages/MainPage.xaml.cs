using TFG_Projects_APP_Frontend.PageModels;

namespace TFG_Projects_APP_Frontend.Pages;

public partial class MainPage : ContentPage
{
    public MainPageModel ViewModel { get; set; }
    public MainPage(MainPageModel mainPageModel)
    {
        InitializeComponent();
        BindingContext = mainPageModel;
        ViewModel = mainPageModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        ViewModel?.OnNavigatedTo();
    }
}
