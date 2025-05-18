using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TFG_Projects_APP_Frontend.Components.CreateModal;
using TFG_Projects_APP_Frontend.Entities.Dtos.ConceptBoards;
using TFG_Projects_APP_Frontend.Entities.Dtos.Concepts;
using TFG_Projects_APP_Frontend.Entities.Dtos.Projects;
using TFG_Projects_APP_Frontend.Entities.Dtos.ProjectUsers;
using TFG_Projects_APP_Frontend.Entities.Dtos.TaskBoards;
using TFG_Projects_APP_Frontend.Entities.Dtos.TaskProgress;
using TFG_Projects_APP_Frontend.Entities.Dtos.TaskSections;
using TFG_Projects_APP_Frontend.Entities.Dtos.UserProjectPermissions;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Services.ConceptBoardsService;
using TFG_Projects_APP_Frontend.Services.ConceptsService;
using TFG_Projects_APP_Frontend.Services.PermissionsService;
using TFG_Projects_APP_Frontend.Services.ProjectsService;
using TFG_Projects_APP_Frontend.Services.ProjectUsersService;
using TFG_Projects_APP_Frontend.Services.RolesService;
using TFG_Projects_APP_Frontend.Services.TaskBoardsService;
using TFG_Projects_APP_Frontend.Services.TaskProgressService;
using TFG_Projects_APP_Frontend.Services.TaskSectionsService;
using TFG_Projects_APP_Frontend.Services.UserProjectPermissionsService;
using TFG_Projects_APP_Frontend.Services.UsersService;

namespace TFG_Projects_APP_Frontend.PageModels;

[QueryProperty(nameof(Project), nameof(Project))]
public partial class ProjectManagementPageModel : ObservableObject
{
    private readonly IConceptsService conceptsService;
    private readonly IConceptBoardsService conceptBoardsService;
    private readonly ITaskBoardsService taskBoardsService;
    private readonly ITaskSectionsService taskSectionsService;
    private readonly ITaskProgressService taskProgressService;
    private readonly IUserProjectPermissionsService userProjectPermissionsService;
    private readonly IPermissionsService permissionsService;
    private readonly IProjectsService projectsService;
    private readonly IProjectUsersService projectUsersService;
    private readonly IUsersService usersService;
    private readonly IRolesService rolesService;
    private readonly UserSession userSession;

    public Project Project { get; set; }

    [ObservableProperty]
    private bool _isLoadingConcepts;

    [ObservableProperty]
    private bool _isLoadingTaskBoards;

    [ObservableProperty]
    private bool _isLoadingUsers;

    [ObservableProperty]
    private Project _currentProject;

    [ObservableProperty]
    private Concept _selectedConcept;

    [ObservableProperty]
    private AppUser _selectedUser;

    [ObservableProperty]
    private TaskBoard _selectedTaskBoard;

    [ObservableProperty]
    private ObservableCollection<Concept> _concepts = new();

    [ObservableProperty]
    private ObservableCollection<TaskBoard> _taskBoards = new();

    [ObservableProperty]
    private ObservableCollection<AppUser> _users = new();

    public ProjectManagementPageModel(
        IConceptsService conceptsService,
        IConceptBoardsService conceptBoardsService,
        ITaskBoardsService taskBoardsService,
        ITaskSectionsService taskSectionsService,
        ITaskProgressService taskProgressService,
        IUserProjectPermissionsService userProjectPermissionsService,
        IPermissionsService permissionsService,
        IProjectsService projectsService,
        IProjectUsersService projectUsersService,
        IUsersService usersService,
        IRolesService rolesService, 
        UserSession userSession)
    {
        this.conceptsService = conceptsService;
        this.conceptBoardsService = conceptBoardsService;
        this.taskBoardsService = taskBoardsService;
        this.taskSectionsService = taskSectionsService;
        this.taskProgressService = taskProgressService;
        this.userProjectPermissionsService = userProjectPermissionsService;
        this.permissionsService = permissionsService;
        this.projectsService = projectsService;
        this.projectUsersService = projectUsersService;
        this.usersService = usersService;
        this.rolesService = rolesService;
        this.userSession = userSession;
    }

    public async Task OnNavigatedTo()
    {
        IsLoadingConcepts = true;
        IsLoadingTaskBoards = true;
        IsLoadingUsers = true;
        CurrentProject = Project;

        var concepts = await conceptsService.GetAllConceptsByProject(Project.Id);
        var taskBoards = await taskBoardsService.GetAllTaskBoardsByProject(Project.Id);
        var users = await usersService.GetUsersByProject(Project.Id);

        Concepts.Clear();
        foreach (var item in concepts)
        {
            Concepts.Add(item);
        }
            
        TaskBoards.Clear();
        foreach (var item in taskBoards)
        {
            TaskBoards.Add(item);
        }

        Users.Clear();
        foreach (var item in users)
        {
            Users.Add(item);
        }
        IsLoadingConcepts = false;
        IsLoadingTaskBoards = false;
        IsLoadingUsers = false;
    }

    [RelayCommand]
    private async void ConceptSelected(Concept concept)
    {
        await Shell.Current.GoToAsync("ConceptBoardPage", new Dictionary<string, object>
        {
             {"Concept", SelectedConcept }
        });
    }

    [RelayCommand]
    private async void UserSelected(AppUser user)
    {

    }

    [RelayCommand]
    private async void TaskBoardSelected(TaskBoard taskBoard)
    {
        await Shell.Current.GoToAsync("TaskBoardPage", new Dictionary<string, object>
        {
             {"TaskBoard", SelectedTaskBoard }
        });
    }

    [RelayCommand]
    private async void CreateTaskBoard()
    {
        IsLoadingTaskBoards = true;
        var taskBoardform = await FormDialog.ShowCreateObjectMenuAsync<TaskBoardFormCreate>();
        if (taskBoardform != null && !string.IsNullOrEmpty(taskBoardform.Title))
        {
            if (string.IsNullOrEmpty(taskBoardform.Description))
            {
                taskBoardform.Description = string.Empty;
            }

            var taskBoard = new TaskBoardCreate
            {
                IdProject = Project.Id,
                Title = taskBoardform.Title,
                Description = taskBoardform.Description
            };

            var returnTaskBoard = await taskBoardsService.Post(taskBoard);
            var taskSection = await taskSectionsService.Post(new TaskSectionCreate
            {
                IdBoard = returnTaskBoard.Id,
                Title = "Default Section",
                Order = 0
            });
            var taskProgress = await taskProgressService.Post(new TaskProgressCreate
            {
                IdSection = taskSection.Id,
                Title = "Default Progress",
                Order = 0,
                ModifiesProgress = false,
                ProgressValue = 0
            });
            var taskBoards = TaskBoards.ToList();
            TaskBoards.Clear();
            TaskBoards = new(taskBoards);
        } else
        {
            if (string.IsNullOrEmpty(taskBoardform.Title))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Title is required", "OK");
            }
        }
        IsLoadingTaskBoards = false;
    }

    [RelayCommand]
    private async void CreateConcept()
    {
        IsLoadingConcepts = true;
        var conceptForm = await FormDialog.ShowCreateObjectMenuAsync<ConceptFormCreate>();
        
        if (conceptForm != null && !string.IsNullOrEmpty(conceptForm.Title))
        {
            if (string.IsNullOrEmpty(conceptForm.Description))
            {
                conceptForm.Description = string.Empty;
            }

            var concept = new ConceptCreate
            {
                IdProject = Project.Id,
                Title = conceptForm.Title,
                Description = conceptForm.Description
            };

            var returnConcept = await conceptsService.Post(concept);
            var conceptBoard = await conceptBoardsService.Post(new ConceptBoardCreate
            {
                IdConcept = returnConcept.Id,
                Name = "Default Board"
            });
            var concepts = Concepts.ToList();
            Concepts.Clear();
            Concepts = new(concepts);
        } else
        {
            if (string.IsNullOrEmpty(conceptForm.Title))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Title is required", "OK");
            }
        }
            IsLoadingConcepts = false;
    }

    [RelayCommand]
    private async void TaskBoardDelete(TaskBoard taskBoard)
    {
        IsLoadingTaskBoards = true;
        bool confirmed = await Application.Current.MainPage.DisplayAlert(
            "Confirm Delete",
            "Are you sure you want to delete this item?",
            "Delete",
            "Cancel"
        );

        if (confirmed)
        {
            await taskBoardsService.Delete(taskBoard.Id);
            TaskBoards.Remove(taskBoard);
        }
        IsLoadingTaskBoards = false;
    }

    [RelayCommand]
    private async void ConceptDelete(Concept concept)
    {
        IsLoadingConcepts = true;
        bool confirmed = await Application.Current.MainPage.DisplayAlert(
            "Confirm Delete",
            "Are you sure you want to delete this item?",
            "Delete",
            "Cancel"
        );

        if (confirmed)
        {
            await projectsService.Delete(concept.Id);
            Concepts.Remove(concept);
        }
        IsLoadingConcepts = false;
    }
}