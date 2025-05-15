using TFG_Projects_APP_Frontend.PageModels;

namespace TFG_Projects_APP_Frontend.Pages;

public partial class ProjectManagementPage : ContentPage
{
	public ProjectManagementPageModel ViewModel { get; set; }
    public ProjectManagementPage(ProjectManagementPageModel projectManagementPageModel)
	{
		InitializeComponent();
        BindingContext = projectManagementPageModel;
        ViewModel = projectManagementPageModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        ViewModel?.OnNavigatedTo();
    }
}