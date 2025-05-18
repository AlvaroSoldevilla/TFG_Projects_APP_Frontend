using TFG_Projects_APP_Frontend.PageModels.Concepts;

namespace TFG_Projects_APP_Frontend.Pages.Concepts;

public partial class ConceptBoardPage : ContentPage
{
    public ConceptBoardPageModel ViewModel { get; set; }

    public ConceptBoardPage(ConceptBoardPageModel conceptBoardPageModel)
	{
		InitializeComponent();
        BindingContext = conceptBoardPageModel;
        ViewModel = conceptBoardPageModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        ViewModel?.OnNavigatedTo();
    }
}