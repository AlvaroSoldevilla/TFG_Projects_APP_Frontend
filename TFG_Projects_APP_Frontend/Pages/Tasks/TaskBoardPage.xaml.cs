using TFG_Projects_APP_Frontend.PageModels.Tasks;

namespace TFG_Projects_APP_Frontend.Pages.Tasks;

public partial class TaskBoardPage : ContentPage
{
	public TaskBoardPage(TaskBoardPageModel taskBoardPageModel)
	{
		InitializeComponent();
        BindingContext = taskBoardPageModel;
    }
}