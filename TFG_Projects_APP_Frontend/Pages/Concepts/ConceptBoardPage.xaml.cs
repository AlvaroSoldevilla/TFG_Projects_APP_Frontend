using Microsoft.Maui.Layouts;
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
        ViewModel._page = this;
    }

    public void AddComponent(View component, Point position)
    {
        AbsoluteLayout.SetLayoutBounds(component, new Rect(position.X, position.Y, -1, -1));
        AbsoluteLayout.SetLayoutFlags(component, AbsoluteLayoutFlags.None);
        ComponentCanvas.Children.Add(component);
    }

    public void ClearComponents()
    {
        ComponentCanvas.Children.Clear();
    }
}