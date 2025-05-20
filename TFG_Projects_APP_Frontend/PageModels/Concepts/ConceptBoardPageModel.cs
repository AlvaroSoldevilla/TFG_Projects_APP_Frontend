using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Components.CreateModal;
using TFG_Projects_APP_Frontend.Entities.Dtos.Components;
using TFG_Projects_APP_Frontend.Entities.Dtos.ProjectUsers;
using TFG_Projects_APP_Frontend.Entities.Dtos.UserProjectPermissions;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Services.ComponentsService;
using TFG_Projects_APP_Frontend.Services.ConceptBoardsService;
using TFG_Projects_APP_Frontend.Services.TypesService;
using TFG_Projects_APP_Frontend.Services.UsersService;

namespace TFG_Projects_APP_Frontend.PageModels.Concepts;

[QueryProperty(nameof(ConceptBoard), nameof(ConceptBoard))]
public partial class ConceptBoardPageModel : ObservableObject
{
    private readonly IConceptBoardsService conceptBoardsService;
    private readonly IComponentsService componentsService;
    private readonly ITypesService typesService;
    private readonly UserSession userSession;

    public ConceptBoard ConceptBoard { get; set; }

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private Component _selectedComponent;

    [ObservableProperty]
    private ObservableCollection<Component> _components;

    [ObservableProperty]
    private ObservableCollection<ProjectType> _types;

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
        Components = new(await componentsService.GetAllComponentsByBoard(ConceptBoard.Id));
        Types = new(await typesService.GetAll());
        IsLoading = false;
    }

    [RelayCommand]
    public async Task TypeSelected(ProjectType projectType)
    {
        IsLoading = true;
        var returnComponent = await FormDialog.ShowCreateObjectMenuAsync<ComponentFormCreate>();
        if (returnComponent != null && !string.IsNullOrEmpty(returnComponent.Title))
        {
            if (string.IsNullOrEmpty(returnComponent.Content))
            {
                returnComponent.Content = string.Empty;
            }

            ComponentCreate componentCreate = new()
            {
                IdBoard = ConceptBoard.Id,
                IdType = projectType.Id,
                Title = returnComponent.Title,
                Content = returnComponent.Content,
                PosX = 0,
                PosY = 0
            };

            var component = await componentsService.Post(componentCreate);
            component.IdParent = component.Id;
            var componentUpdate = new ComponentUpdate
            {
                Title = component.Title,
                Content = component.Content,
                IdType = component.IdType,
                PosX = component.PosX,
                PosY = component.PosY,
                IdBoard = component.IdBoard,
                IdParent = component.Id
            };
            var componentResult = await componentsService.Patch(component.Id, componentUpdate);
            if (componentResult == "Component updated")
            {
                var components = Components.ToList();
                components.Add(component);
                Components.Clear();
                Components = new(components);
            }
        }
        else
        {
            if (string.IsNullOrEmpty(returnComponent.Title))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Title is required", "OK");
            }
        }
        IsLoading = false;
    }
}
