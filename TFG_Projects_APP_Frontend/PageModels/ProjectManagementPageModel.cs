using CommunityToolkit.Mvvm.ComponentModel;
using TFG_Projects_APP_Frontend.Services.ConceptsService;
using TFG_Projects_APP_Frontend.Services.PermissionsService;
using TFG_Projects_APP_Frontend.Services.ProjectsService;
using TFG_Projects_APP_Frontend.Services.ProjectUsersService;
using TFG_Projects_APP_Frontend.Services.RolesService;
using TFG_Projects_APP_Frontend.Services.TaskBoardsService;
using TFG_Projects_APP_Frontend.Services.UserProjectPermissionsService;

namespace TFG_Projects_APP_Frontend.PageModels;

public class ProjectManagementPageModel(
    ConceptsService conceptsService,
    TaskBoardsService taskBoardsService,
    UserProjectPermissionsService userProjectPermissionsService,
    PermissionsService permissionsService,
    ProjectsService projectsService,
    ProjectUsersService projectUsersService,
    RolesService rolesService) : ObservableObject
{
    private readonly ConceptsService conceptsService = conceptsService;
    private readonly TaskBoardsService taskBoardsService = taskBoardsService;
    private readonly UserProjectPermissionsService userProjectPermissionsService = userProjectPermissionsService;
    private readonly PermissionsService permissionsService = permissionsService;
    private readonly ProjectsService projectsService = projectsService;
    private readonly ProjectUsersService projectUsersService = projectUsersService;
    private readonly RolesService rolesService = rolesService;
}
