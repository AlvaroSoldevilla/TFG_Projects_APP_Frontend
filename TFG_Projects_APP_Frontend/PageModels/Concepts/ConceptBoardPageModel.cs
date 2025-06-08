using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Components.ConceptComponents;
using TFG_Projects_APP_Frontend.Components.CreateModal;
using TFG_Projects_APP_Frontend.Entities.Dtos.Components;
using TFG_Projects_APP_Frontend.Entities.Dtos.ConceptBoards;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Pages.Concepts;
using TFG_Projects_APP_Frontend.Properties;
using TFG_Projects_APP_Frontend.Services.ComponentsService;
using TFG_Projects_APP_Frontend.Services.ConceptBoardsService;
using TFG_Projects_APP_Frontend.Services.TypesService;
using TFG_Projects_APP_Frontend.Services.UsersService;
using TFG_Projects_APP_Frontend.Utils;

namespace TFG_Projects_APP_Frontend.PageModels.Concepts;

public partial class ConceptBoardPageModel : ObservableObject
{
    private readonly IConceptBoardsService conceptBoardsService;
    private readonly IComponentsService componentsService;
    private readonly ITypesService typesService;
    private readonly UserSession userSession;
    private readonly PermissionsUtils permissionsUtils;

    public ConceptBoardPage? _page;

    public ConceptBoard ConceptBoard { get; set; }

    private ConceptComponent _hoveringContainer { get; set; }

    [ObservableProperty]
    private bool _isLoading;

    private bool _isLoadingData = false;
    private bool _isNavigating = false;

    [ObservableProperty]
    private bool _isEditingNote;

    [ObservableProperty]
    private bool _isEditingComponent;

    [ObservableProperty]
    private ConceptComponent _editComponentData;

    [ObservableProperty]
    private ObservableCollection<ConceptComponent> _components;

    [ObservableProperty]
    private ObservableCollection<ProjectType> _types;


    public ConceptBoardPageModel(
        IConceptBoardsService conceptBoardsService, 
        IComponentsService componentsService, 
        ITypesService typesService, 
        UserSession userSession,
        PermissionsUtils permissionsUtils)
    {
        this.conceptBoardsService = conceptBoardsService;
        this.componentsService = componentsService;
        this.typesService = typesService;
        this.userSession = userSession;
        this.permissionsUtils = permissionsUtils;
    }

    public async Task OnNavigatedTo()
    {
        IsLoading = true;
        IsEditingComponent = false;
        IsEditingNote = false;
        await LoadTypes();
        await LoadData();
        IsLoading = false;
    }

    [RelayCommand]
    private async void NavigateInside(ConceptComponent component)
    {
        if (component != null && component.IdType == 1 && !_isNavigating)
        {
            _isNavigating = true;
            var componentBaordId = int.Parse(component.Content);
            var componentBoard = await conceptBoardsService.GetById(componentBaordId);
            NavigationContext.CurrentConceptBoards.Push(ConceptBoard);
            NavigationContext.CurrentConceptBoards.Push(component.Board);
            if (component.Board != null)
            {
                await Shell.Current.GoToAsync("ConceptBoardPage");
            }
            _isNavigating = false;
        }
    }

    private async Task LoadTypes()
    {
        if (Types == null || Types.Count == 0)
        {
            var types = await typesService.GetAll();
            Types = new ObservableCollection<ProjectType>(types);
        }
    }

    private async Task LoadData()
    {
        if (_isLoadingData)
        {
            return;
        }
        _isLoadingData = true;
        if (ConceptBoard == null)
        {
            if (NavigationContext.CurrentConceptBoards.Count() == 0)
            {
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                ConceptBoard = NavigationContext.CurrentConceptBoards.Pop();
            }
        }
        
        IsLoading = true;
        var componentList = await componentsService.GetAllComponentsByBoard(ConceptBoard.Id);
        _page.ClearComponents();
        foreach (var component in componentList)
        {
            if (component.IdType == 1 && !string.IsNullOrEmpty(component.Content))
            {
                var componentBoardId = int.Parse(component.Content);
                var componentBoard = await conceptBoardsService.GetById(componentBoardId);
                component.Board = componentBoard;
            }
            else
            {
                component.Board = ConceptBoard;
            }
            
        }

        foreach (var component in componentList)
        {
            if (component.IdType == 3)
            {
                var allComponents = await componentsService.GetAllComponentsByBoard(ConceptBoard.Id);
                component.Children = allComponents.Where(c => c.IdParent == component.Id).ToList();
            }

            PaintComponent(component);
        }
        IsLoading = false;
        _isLoadingData = false;
    }

    private void PaintComponent(ConceptComponent component)
    {
        View componentView = null;
        switch (component.IdType)
        {
            case 1: // Board
                componentView = new ConceptBoardComponent
                {
                    Component = component,
                    TapCommand = new Command<ConceptComponent>(EditComponent),
                    DoubleTapCommand = new Command<ConceptComponent>(NavigateInside),
                    DragEndCommand = new Command<ConceptComponent>(DropComponent)
                };
                break;
            case 2: // Note

                if (component.IdParent == null || component.IdParent == component.Id)
                {
                    componentView = new NoteComponent
                    {
                        Component = component,
                        TapCommand = new Command<ConceptComponent>(EditNote),
                        DragEndCommand = new Command<ConceptComponent>(DropComponent)
                    };
                }
                break;
            case 3: // Container
                component.Children ??= new List<ConceptComponent>();

                componentView = new ContainerComponent
                {
                    Component = component,
                    TapCommand = new Command<ConceptComponent>(EditComponent),
                    ChildTapCommand = new Command<ConceptComponent>(EditNote),
                    DragEndCommand = new Command<ConceptComponent>(DropComponent),
                    ChildDragEndCommand = new Command<ConceptComponent>(DropComponent),
                    HoverEnterCommand = new Command<ConceptComponent>(HoverEnter),
                    HoverExitCommand = new Command<ConceptComponent>(HoverExit)
                };
                break;
            case 4: // Table
                componentView = new TableComponent
                {
                    Component = component,
                    TapCommand = new Command<ConceptComponent>(EditComponent),
                    DragEndCommand = new Command<ConceptComponent>(DropComponent),
                    UpdateContentCommand = new Command<ConceptComponent>(SaveComponent)
                };
                break;
        }

        if (componentView != null)
        {
            _page.AddComponent(componentView, new Point(component.PosX ?? 0, component.PosY ?? 0));
        }
    }

    private void HoverExit(ConceptComponent component)
    {
        _hoveringContainer = null;
    }

    private void HoverEnter(ConceptComponent component)
    {
        _hoveringContainer = component;
    }

    [RelayCommand]
    public async Task TypeSelected(ProjectType projectType)
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.CreateComponents, PermissionsUtils.Permissions.FullConceptPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            IsLoading = true;
            ComponentFormCreate returnComponent;
            string title;
            switch (projectType.Id)
            {
                case 1:
                    title = Resources.CreateBoardTitle;
                    break;
                case 2:
                    title = Resources.CreateNoteTitle;
                    break;
                case 3:
                    title = Resources.CreateContainerTitle;
                    break;
                case 4:
                    title = Resources.CreateTableTitle;
                    break;
                default:
                    title = Resources.CreateComponentTitle;
                    break;
            }

            if (projectType.Id == 2)
            {
                returnComponent = await FormDialog.ShowCreateObjectMenuAsync<NoteComponentFormCreate>(title);
            }
            else
            {
                returnComponent = await FormDialog.ShowCreateObjectMenuAsync<ComponentFormCreate>(title);
            }

            if (returnComponent != null && !string.IsNullOrEmpty(returnComponent.Title))
            {
                ConceptComponent component;
                if (returnComponent is NoteComponentFormCreate noteComponentForm)
                {
                    if (string.IsNullOrEmpty(noteComponentForm.Content))
                    {
                        noteComponentForm.Content = string.Empty;
                    }

                    ComponentCreate componentCreate = new()
                    {
                        IdBoard = ConceptBoard.Id,
                        IdType = projectType.Id,
                        Title = noteComponentForm.Title,
                        Content = noteComponentForm.Content,
                        PosX = 0,
                        PosY = 0
                    };
                    component = await componentsService.Post(componentCreate);
                }
                else
                {
                    ComponentCreate componentCreate = new()
                    {
                        IdBoard = ConceptBoard.Id,
                        IdType = projectType.Id,
                        Title = returnComponent.Title,
                        Content = string.Empty,
                        PosX = 0,
                        PosY = 0
                    };
                    component = await componentsService.Post(componentCreate);
                }

                component.IdParent = component.Id;
                var componentUpdate = new ComponentUpdate
                {
                    Title = component.Title,
                    Content = component.Content,
                    IdType = component.IdType,
                    PosX = component.PosX,
                    PosY = component.PosY,
                    IdBoard = component.IdBoard,
                    IdParent = component.IdParent
                };
                await componentsService.Patch(component.Id, componentUpdate);

                if (component.IdType == 1)
                {
                    var componentBoard = await conceptBoardsService.Post(new ConceptBoardCreate
                    {
                        Name = component.Title,
                        IdConcept = ConceptBoard.IdConcept,
                        IdParent = ConceptBoard.Id
                    });
                    component.Content = componentBoard.Id.ToString();
                    await componentsService.Patch(component.Id, new ComponentUpdate
                    {
                        Title = component.Title,
                        Content = component.Content,
                        IdType = component.IdType,
                        PosX = component.PosX,
                        PosY = component.PosY,
                        IdBoard = component.IdBoard,
                        IdParent = component.IdParent
                    });
                }
                else if (component.IdType == 4)
                {
                    component.Content = string.Join("\n", new string[] { $"{Resources.DefaultColumnTitle}1\t{Resources.DefaultColumnTitle}2\t{Resources.DefaultColumnTitle}3\n" });
                    component.Content += "\t\t\n";
                    component.Content += "\t\t\n";
                    await componentsService.Patch(component.Id, new ComponentUpdate
                    {
                        Title = component.Title,
                        Content = component.Content,
                        IdType = component.IdType,
                        PosX = component.PosX,
                        PosY = component.PosY,
                        IdBoard = component.IdBoard,
                        IdParent = component.IdParent
                    });
                }
                await LoadData();
            }
            else
            {
                if (string.IsNullOrEmpty(returnComponent.Title))
                {
                    await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.TitleIsRequiredMessage, Resources.ConfirmButton);
                }
            }
            IsLoading = false;
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.NoPermissionMessage, Resources.ConfirmButton);
        }
    }

    [RelayCommand]
    public async void DeleteComponent(ConceptComponent component)
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.DeleteComponents, PermissionsUtils.Permissions.FullConceptPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            if (EditComponentData != null)
            {
                var confirm = await Application.Current.MainPage.DisplayAlert(Resources.ConfirmDeleteMessageTitle, Resources.DeletionConfirmationMessage, Resources.YesButton, Resources.NoButton);
                if (confirm)
                {
                    await componentsService.Delete(EditComponentData.Id);
                    await LoadData();
                }
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.NoPermissionMessage, Resources.ConfirmButton);
        }
    }

    [RelayCommand]
    private async void DropComponent(ConceptComponent component)
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.EditComponents, PermissionsUtils.Permissions.FullConceptPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            if (component != null && component != null)
            {
                if (component.IdType == 2 && _hoveringContainer != null)
                {
                    component.IdParent = _hoveringContainer.Id;
                }
                else if (component.IdType == 2)
                {
                    component.IdParent = component.Id;
                }

                var componentUpdate = new ComponentUpdate
                {
                    IdBoard = component.IdBoard,
                    IdParent = component.IdParent,
                    IdType = component.IdType,
                    Title = component.Title,
                    PosX = component.PosX,
                    PosY = component.PosY,
                    Content = component.Content
                };
                await componentsService.Patch(component.Id, componentUpdate);
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.NoPermissionMessage, Resources.ConfirmButton);
        }
        await LoadData();
    }

    [RelayCommand]
    private async void EditNote(ConceptComponent component)
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.EditComponents, PermissionsUtils.Permissions.FullConceptPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            IsEditingComponent = true;
            IsEditingNote = true;
            EditComponentData = component;
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.NoPermissionMessage, Resources.ConfirmButton);
        }
    }

    [RelayCommand]
    private async void EditComponent(ConceptComponent component)
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.EditComponents, PermissionsUtils.Permissions.FullConceptPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            IsEditingNote = false;
            IsEditingComponent = true;
            EditComponentData = component;
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.NoPermissionMessage, Resources.ConfirmButton);
        }
    }

    [RelayCommand]
    private async void CloseEditingComponent()
    {
        IsEditingNote = false;
        IsEditingComponent = false;
        EditComponentData = null;
    }

    [RelayCommand]
    private async void SaveComponent(ConceptComponent component)
    {
        if (EditComponentData != null && !string.IsNullOrEmpty(EditComponentData.Title))
        {
            var componentUpdate = new ComponentUpdate
            {
                IdBoard = ConceptBoard.Id,
                IdType = EditComponentData.IdType,
                Title = EditComponentData.Title,
                PosX = EditComponentData.PosX,
                PosY = EditComponentData.PosY,
                Content = EditComponentData.Content
            };
            await componentsService.Patch(EditComponentData.Id, componentUpdate);
            IsEditingNote = false;
            IsEditingComponent = false;
            EditComponentData = null;
            await LoadData();
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.TitleIsRequiredMessage, Resources.ConfirmButton);
        }
    }

}
