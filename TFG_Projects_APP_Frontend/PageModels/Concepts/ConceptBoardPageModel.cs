﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Components.ConceptComponents;
using TFG_Projects_APP_Frontend.Components.CreateModal;
using TFG_Projects_APP_Frontend.Entities.Dtos.Components;
using TFG_Projects_APP_Frontend.Entities.Dtos.ConceptBoards;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Pages.Concepts;
using TFG_Projects_APP_Frontend.Services;
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

    public ConceptBoardPage? _page;

    public ConceptBoard ConceptBoard { get; set; }

    [ObservableProperty]
    private bool _isLoading;

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
        IsEditingComponent = false;
        IsEditingNote = false;
        await LoadTypes();
        await LoadData();
        IsLoading = false;
    }

    [RelayCommand]
    private async void NavigateInside(ConceptComponent component)
    {
        if (component != null && component.IdType == 1)
        {
            var componentBaordId = int.Parse(component.Content);
            var componentBoard = await conceptBoardsService.GetById(componentBaordId);
            NavigationContext.CurrentConceptBoards.Push(component.Board);
            if (component.Board != null)
            {
                await Shell.Current.GoToAsync("ConceptBoardPage", new Dictionary<string, object>
            {
                 {"ConceptBoard", component.Board }
            });
            }
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
            PaintComponent(component);
        }
        IsLoading = false;
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
                componentView = new NoteComponent
                {
                    Component = component,
                    TapCommand = new Command<ConceptComponent>(EditNote),
                    DragEndCommand = new Command<ConceptComponent>(DropComponent)
                };
                break;
        }

        if (componentView != null)
        {
            _page.AddComponent(componentView, new Point(component.PosX ?? 0, component.PosY ?? 0));
        }
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
            await componentsService.Patch(component.Id, componentUpdate);

            if (component.IdType == 1)
            {
                var componentBoard = await conceptBoardsService.Post(new ConceptBoardCreate
                {
                    Name = component.Title,
                    IdConcept = ConceptBoard.IdConcept,
                    IdParent = ConceptBoard.Id
                });
                component.Board = componentBoard;
                component.Content = componentBoard.Id.ToString();
                await componentsService.Patch(component.Id, new ComponentUpdate
                {
                    Title = component.Title,
                    PosX = component.PosX,
                    PosY = component.PosY,
                    Content = component.Content,
                    IdBoard = componentBoard.Id,
                });
            }
            else
            {
                component.Board = ConceptBoard;
            }
            await LoadData();
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

    [RelayCommand]
    public async void DeleteComponent(ConceptComponent component)
    {
        if (EditComponentData != null)
        {
            var confirm = await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure you want to delete this component?", "Yes", "No");
            if (confirm)
            {
                await componentsService.Delete(EditComponentData.Id);
                await LoadData();
            }
        }
    }

    [RelayCommand]
    private async void DropComponent(ConceptComponent component)
    {
        if (component != null && component != null)
        {
            var componentUpdate = new ComponentUpdate
            {
                IdBoard = component.IdBoard,
                IdType = component.IdType,
                Title = component.Title,
                PosX = component.PosX,
                PosY = component.PosY,
                Content = component.Content
            };
            await componentsService.Patch(component.Id, componentUpdate);
            await LoadData();
        }
    }

    [RelayCommand]
    private async void EditNote(ConceptComponent component)
    {
        IsEditingComponent = true;
        IsEditingNote = true;
        EditComponentData = component;
    }

    [RelayCommand]
    private async void EditComponent(ConceptComponent component)
    {
        IsEditingNote = false;
        IsEditingComponent = true;
        EditComponentData = component;
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
            await Application.Current.MainPage.DisplayAlert("Error", "Title is required", "OK");
        }
    }

}
