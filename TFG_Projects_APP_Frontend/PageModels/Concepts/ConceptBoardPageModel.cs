using CommunityToolkit.Mvvm.ComponentModel;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Services.ComponentsService;
using TFG_Projects_APP_Frontend.Services.ConceptBoardsService;
using TFG_Projects_APP_Frontend.Services.TypesService;
using TFG_Projects_APP_Frontend.Services.UsersService;

namespace TFG_Projects_APP_Frontend.PageModels.Concepts;

[QueryProperty(nameof(Concept), nameof(Concept))]
public partial class ConceptBoardPageModel : ObservableObject
{
    private readonly IConceptBoardsService conceptBoardsService;
    private readonly IComponentsService componentsService;
    private readonly ITypesService typesService;
    private readonly UserSession userSession;

    public Concept Concept { get; set; }

    [ObservableProperty]
    private bool _isLoading;

    public ConceptBoardPageModel(
        IConceptBoardsService conceptBoardsService, 
        IComponentsService componentsService, 
        ITypesService typesService, 
        UserSession userSession)
    {
        this.conceptBoardsService = conceptBoardsService;
        this.componentsService = componentsService;
        this.typesService = typesService;
        this.userSession = userSession;
    }

    public async Task OnNavigatedTo()
    {
        IsLoading = true;
        IsLoading = false;
    }
}
