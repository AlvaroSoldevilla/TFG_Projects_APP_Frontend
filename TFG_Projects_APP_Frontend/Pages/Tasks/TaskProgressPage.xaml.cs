using TFG_Projects_APP_Frontend.PageModels.Tasks;

namespace TFG_Projects_APP_Frontend.Pages.Tasks;

public partial class TaskProgressPage : ContentPage
{
	public TaskProgressPage(TaskProgressPageModel taskProgressPageModel)
	{
		InitializeComponent();
        BindingContext = taskProgressPageModel;
    }
}