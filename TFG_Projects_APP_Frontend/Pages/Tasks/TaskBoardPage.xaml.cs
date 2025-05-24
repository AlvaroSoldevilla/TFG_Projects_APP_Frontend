using TFG_Projects_APP_Frontend.PageModels.Tasks;

namespace TFG_Projects_APP_Frontend.Pages.Tasks;

public partial class TaskBoardPage : ContentPage
{
	public TaskBoardPageModel ViewModel { get; set; }

    public TaskBoardPage(TaskBoardPageModel taskBoardPageModel)
	{
		InitializeComponent();
        BindingContext = taskBoardPageModel;
        ViewModel = taskBoardPageModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        ViewModel?.OnNavigatedTo();
    }

}