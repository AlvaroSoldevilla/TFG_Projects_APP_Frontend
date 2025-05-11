using CommunityToolkit.Mvvm.ComponentModel;
using TFG_Projects_APP_Frontend.Services.ComponentsService;
using TFG_Projects_APP_Frontend.Services.ConceptBoardsService;
using TFG_Projects_APP_Frontend.Services.TypesService;

namespace TFG_Projects_APP_Frontend.PageModels.Concepts;

public class ConceptBoardPageModel(ConceptBoardsService conceptBoardsService, ComponentsService componentsService, TypesService typesService) : ObservableObject
{
    private readonly ConceptBoardsService conceptBoardsService = conceptBoardsService;
    private readonly ComponentsService componentsService = componentsService;
    private readonly TypesService typesService = typesService;
}
