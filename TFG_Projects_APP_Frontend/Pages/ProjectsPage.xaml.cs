using TFG_Projects_APP_Frontend.PageModels;

namespace TFG_Projects_APP_Frontend.Pages;

public partial class ProjectsPage : ContentPage
{
	public ProjectsPage(ProjectsPageModel projectsPageModel)
	{
		InitializeComponent();
        BindingContext = projectsPageModel;
    }
}