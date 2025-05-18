using TFG_Projects_APP_Frontend.PageModels.Tasks;

namespace TFG_Projects_APP_Frontend.Pages.Tasks;

public partial class TaskProgressPage : ContentPage
{
	public TaskProgressPageModel ViewModel { get; set; }

    public TaskProgressPage(TaskProgressPageModel taskProgressPageModel)
	{
		InitializeComponent();
        BindingContext = taskProgressPageModel;
        ViewModel = taskProgressPageModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        ViewModel?.OnNavigatedTo();
    }
}