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

    [ObservableProperty]
    private bool _isLoading;

    private bool _isLoadingData = false;
    private bool _isNavigating = false;
    private bool _isRemovingNoteFromComntainer = false;

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

    /*When a user navigates to the page, resets the relevant properties and begins loading the data*/
    public async Task OnNavigatedTo()
    {
        IsLoading = true;
        IsEditingComponent = false;
        IsEditingNote = false;
        await LoadTypes();
        await LoadData();
        IsLoading = false;
    }

    

    /*Loads the types*/
    private async Task LoadTypes()
    {
        if (Types == null || Types.Count == 0)
        {
            var types = await typesService.GetAll();
            Types = new ObservableCollection<ProjectType>(types);
        }
    }

    /*Loads the components*/
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

        Components = new(componentList);

        IsLoading = false;
        _isLoadingData = false;
    }

    /*When a ConceptBoard is clicked, updates the state of the NavigationContext and navigates to the new Concept Board*/
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


    /*Calls the page to paint the components*/
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
                    ChildDoubleTapCommand = new Command<ConceptComponent>(RemoveNoteFromContainer)
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

    /*When a type is selected, creates the component*/
    [RelayCommand]
    public async Task TypeSelected(ProjectType projectType)
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.CreateComponents, PermissionsUtils.Permissions.FullConceptPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            IsLoading = true;
            NoteComponentFormCreate returnNoteComponent = null;
            ComponentFormCreate returnComponent = null;
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
                returnNoteComponent = await FormDialog.ShowCreateObjectMenuAsync<NoteComponentFormCreate>(title);
            }
            else
            {
                returnComponent = await FormDialog.ShowCreateObjectMenuAsync<ComponentFormCreate>(title);
            }

            if ((returnComponent != null || returnNoteComponent != null) && (!string.IsNullOrEmpty(returnComponent.Title) || !string.IsNullOrEmpty(returnNoteComponent.Title)))
            {
                ConceptComponent component;
                if (returnComponent == null)
                {
                    if (string.IsNullOrEmpty(returnNoteComponent.Content))
                    {
                        returnNoteComponent.Content = string.Empty;
                    }

                    ComponentCreate componentCreate = new()
                    {
                        IdBoard = ConceptBoard.Id,
                        IdType = projectType.Id,
                        Title = returnNoteComponent.Title,
                        Content = returnNoteComponent.Content,
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

    /*Deletes a component*/
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

    /*Removes a note component from its container component*/
    [RelayCommand]
    private async void RemoveNoteFromContainer(ConceptComponent note)
    {
        _isRemovingNoteFromComntainer = true;
        IsEditingComponent = false;
        IsEditingNote = false;
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.EditComponents, PermissionsUtils.Permissions.FullConceptPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            if (note != null && note.IdParent != null && note.IdParent != note.Id)
            {
                
                note.PosX = 0;
                note.PosY = 0;

                var noteUpdate = new ComponentUpdate
                {
                    IdBoard = note.IdBoard,
                    IdParent = note.Id,
                    IdType = note.IdType,
                    Title = note.Title,
                    PosX = note.PosX,
                    PosY = note.PosY,
                    Content = note.Content
                };
                await componentsService.Patch(note.Id, noteUpdate);
            }
        }
        await LoadData();
    }

    /*Logic for when a component is dropped. If the component is a note, checks if it should place it inside a container component. For any other component, updates its position*/
    [RelayCommand]
    private async void DropComponent(ConceptComponent component)
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.EditComponents, PermissionsUtils.Permissions.FullConceptPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            if (component != null)
            {
                if (component.IdType == 2 && (component.IdParent == null || component.IdParent == component.Id))
                {
                    var container = CheckCollission(component);
                    if (container != null)
                    {
                        component.IdParent = container.Id;
                    }
                    
                }
                else if (component.IdType == 2)
                {
                    component.IdParent = component.Id;
                    component.PosX = 0;
                    component.PosY = 0;
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

    /*Edit a note (Unlike other components, the content of the note component should be edited in the menu)*/
    [RelayCommand]
    private async void EditNote(ConceptComponent component)
    {
        if (!_isRemovingNoteFromComntainer) {
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
        } else
        {
            _isRemovingNoteFromComntainer = false;
        }
    }

    /*Edit any non Note component (Unlike notes, the rest of the components don't allow for their content to be modified directly)*/
    [RelayCommand]
    private async void EditComponent(ConceptComponent component)
    {
        if (!_isRemovingNoteFromComntainer) {
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
    }

    /*Closes the editing menu*/
    [RelayCommand]
    private async void CloseEditingComponent()
    {
        IsEditingNote = false;
        IsEditingComponent = false;
        EditComponentData = null;
    }

    /*Saves the changes done to a component*/
    [RelayCommand]
    private async void SaveComponent(ConceptComponent component)
    {
        ConceptComponent conceptComponent = null;

        if (component == null && EditComponentData != null)
        {
            conceptComponent = EditComponentData;
        } else if (component != null && EditComponentData == null)
        {
            conceptComponent = component;
        }

        if (conceptComponent != null && !string.IsNullOrEmpty(conceptComponent.Title))
        {
            var componentUpdate = new ComponentUpdate
            {
                IdBoard = ConceptBoard.Id,
                IdType = conceptComponent.IdType,
                Title = conceptComponent.Title,
                PosX = conceptComponent.PosX,
                PosY = conceptComponent.PosY,
                Content = conceptComponent.Content
            };
            await componentsService.Patch(conceptComponent.Id, componentUpdate);
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

    /*When a note is dropped, simulates the size of the note component and the container components in the page to check if it should be placed inside it*/
    private ConceptComponent CheckCollission(ConceptComponent note)
    {

        const double containerWidth = 300;
        const double noteHeight = 100; // Approximate height of one NoteComponent
        const double labelHeight = 50; // From MinimumHeightRequest
        const double titlePadding = 10; // Label padding
        const double stackPadding = 10; // ChildrenContainer padding
        const double maxContainerHeight = 500;

        // Simulate bounds of the dragged note
        double noteX = note.PosX ?? 0;
        double noteY = note.PosY ?? 0;
        double noteWidth = 150;
        double noteRectHeight = 100;

        var noteRect = new Rect(noteX, noteY, noteWidth, noteRectHeight);

        foreach (var container in Components.Where(c => c.IdType == 3)) // Only containers
        {
            double containerX = container.PosX ?? 0;
            double containerY = container.PosY ?? 0;

            int childNoteCount = Components.Count(c => c.IdParent == container.Id && c.Id != c.IdParent && c.IdType == 2);

            double totalContainerHeight =
                labelHeight + (2 * titlePadding) +   // Label + its padding
                (childNoteCount * noteHeight) +      // Notes inside
                (2 * stackPadding);                  // StackLayout padding

            var containerRect = new Rect(containerX, containerY, containerWidth, totalContainerHeight);

            if (noteRect.IntersectsWith(containerRect))
            {
                return container;
            }
        }

        return null;
    }
}
