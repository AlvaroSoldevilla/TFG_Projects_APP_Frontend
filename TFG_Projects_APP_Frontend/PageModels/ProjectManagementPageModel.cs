using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
    private readonly ConceptsService conceptsService;
    private readonly TaskBoardsService taskBoardsService;
    private readonly UserProjectPermissionsService userProjectPermissionsService;
    private readonly PermissionsService permissionsService;
    private readonly ProjectsService projectsService;
    private readonly ProjectUsersService projectUsersService;
    private readonly UsersService usersService;
    private readonly RolesService rolesService;

    public Project Project { get; set; }

    [ObservableProperty]
    private ObservableCollection<Concept> _concepts;
    [ObservableProperty]
    private ObservableCollection<TaskBoard> _taskBoards;
    [ObservableProperty]
    private ObservableCollection<Permission> _permissions;
    [ObservableProperty]
    private ObservableCollection<AppUser> _users;
    [ObservableProperty]
    private ObservableCollection<Role> _roles;

    public ProjectManagementPageModel(
        ConceptsService conceptsService,
        TaskBoardsService taskBoardsService,
        UserProjectPermissionsService userProjectPermissionsService,
        PermissionsService permissionsService,
        ProjectsService projectsService,
        ProjectUsersService projectUsersService,
        UsersService usersService,
        RolesService rolesService)
    {
        this.conceptsService = conceptsService;
        this.taskBoardsService = taskBoardsService;
        this.userProjectPermissionsService = userProjectPermissionsService;
        this.permissionsService = permissionsService;
        this.projectsService = projectsService;
        this.projectUsersService = projectUsersService;
        this.usersService = usersService;
        this.rolesService = rolesService;

        LoadData();
    }

    private async Task LoadData()
    {
        Concepts = await conceptsService.GetAllConceptsByProject(Project.Id);
        TaskBoards = await taskBoardsService.GetAllTaskBoardsByProject(Project.Id);
        Users = await usersService.GetUsersByProject(Project.Id);
    }
}
