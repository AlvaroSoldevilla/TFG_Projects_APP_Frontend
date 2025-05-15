using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Services.ConceptsService;
using TFG_Projects_APP_Frontend.Services.PermissionsService;
using TFG_Projects_APP_Frontend.Services.ProjectsService;
using TFG_Projects_APP_Frontend.Services.ProjectUsersService;
using TFG_Projects_APP_Frontend.Services.RolesService;
using TFG_Projects_APP_Frontend.Services.TaskBoardsService;
using TFG_Projects_APP_Frontend.Services.UserProjectPermissionsService;
using TFG_Projects_APP_Frontend.Services.UsersService;

namespace TFG_Projects_APP_Frontend.PageModels;

[QueryProperty(nameof(Project), nameof(Project))]
public partial class ProjectManagementPageModel : ObservableObject
{
    private readonly IConceptsService conceptsService;
    private readonly ITaskBoardsService taskBoardsService;
    private readonly IUserProjectPermissionsService userProjectPermissionsService;
    private readonly IPermissionsService permissionsService;
    private readonly IProjectsService projectsService;
    private readonly IProjectUsersService projectUsersService;
    private readonly IUsersService usersService;
    private readonly IRolesService rolesService;

    // Passed project info (Title, Id, etc.)
    public Project Project { get; set; }

    [ObservableProperty]
    private Project _currentProject;

    // Selected items
    [ObservableProperty]
    private Concept _selectedConcept;

    [ObservableProperty]
    private AppUser _selectedUser;

    [ObservableProperty]
    private TaskBoard _selectedTaskBoard;

    // Collections (NEVER replace the collection → only clear + add)
    [ObservableProperty]
    private ObservableCollection<Concept> _concepts = new();

    [ObservableProperty]
    private ObservableCollection<TaskBoard> _taskBoards = new();

    [ObservableProperty]
    private ObservableCollection<AppUser> _users = new();

    public ProjectManagementPageModel(
        IConceptsService conceptsService,
        ITaskBoardsService taskBoardsService,
        IUserProjectPermissionsService userProjectPermissionsService,
        IPermissionsService permissionsService,
        IProjectsService projectsService,
        IProjectUsersService projectUsersService,
        IUsersService usersService,
        IRolesService rolesService)
    {
        this.conceptsService = conceptsService;
        this.taskBoardsService = taskBoardsService;
        this.userProjectPermissionsService = userProjectPermissionsService;
        this.permissionsService = permissionsService;
        this.projectsService = projectsService;
        this.projectUsersService = projectUsersService;
        this.usersService = usersService;
        this.rolesService = rolesService;
    }

    public async Task OnNavigatedTo()
    {
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
    }
}