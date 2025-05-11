using TFG_Projects_APP_Frontend.PageModels;

namespace TFG_Projects_APP_Frontend.Pages;

public partial class ProjectManagementPage : ContentPage
{
	public ProjectManagementPage(ProjectManagementPageModel projectManagementPageModel)
	{
		InitializeComponent();
        BindingContext = projectManagementPageModel;
    }
}