using TFG_Projects_APP_Frontend.PageModels.Concepts;

namespace TFG_Projects_APP_Frontend.Pages.Concepts;

public partial class ConceptBoardPage : ContentPage
{
	public ConceptBoardPage(ConceptBoardPageModel conceptBoardPageModel)
	{
		InitializeComponent();
        BindingContext = conceptBoardPageModel;
    }
}