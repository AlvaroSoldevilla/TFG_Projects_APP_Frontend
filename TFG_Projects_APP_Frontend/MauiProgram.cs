using Microsoft.Extensions.Logging;
using TFG_Projects_APP_Frontend.PageModels;
using TFG_Projects_APP_Frontend.PageModels.Concepts;
using TFG_Projects_APP_Frontend.PageModels.Tasks;
using TFG_Projects_APP_Frontend.Pages;
using TFG_Projects_APP_Frontend.Pages.Concepts;
using TFG_Projects_APP_Frontend.Pages.Tasks;
using TFG_Projects_APP_Frontend.Rest;
using TFG_Projects_APP_Frontend.Services.ComponentsService;
using TFG_Projects_APP_Frontend.Services.ConceptBoardsService;
using TFG_Projects_APP_Frontend.Services.ConceptsService;
using TFG_Projects_APP_Frontend.Services.PermissionsService;
using TFG_Projects_APP_Frontend.Services.PrioritiesService;
using TFG_Projects_APP_Frontend.Services.ProjectsService;
using TFG_Projects_APP_Frontend.Services.ProjectUsersService;
using TFG_Projects_APP_Frontend.Services.RolesService;
using TFG_Projects_APP_Frontend.Services.TaskBoardsService;
using TFG_Projects_APP_Frontend.Services.TaskDependeciesService;
using TFG_Projects_APP_Frontend.Services.TaskProgressService;
using TFG_Projects_APP_Frontend.Services.TaskSectionsService;
using TFG_Projects_APP_Frontend.Services.TasksService;
using TFG_Projects_APP_Frontend.Services.TypesService;
using TFG_Projects_APP_Frontend.Services.UserProjectPermissionsService;
using TFG_Projects_APP_Frontend.Services.UsersService;

namespace TFG_Projects_APP_Frontend;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<RestClient>();

        builder.Services.AddSingleton<IComponentsService, ComponentsService>();
        builder.Services.AddSingleton<IConceptBoardsService, ConceptBoardsService>();
        builder.Services.AddSingleton<IConceptsService, ConceptsService>();
        builder.Services.AddSingleton<IPermissionsService, PermissionsService>();
        builder.Services.AddSingleton<IPrioritiesService, PrioritiesService>();
        builder.Services.AddSingleton<IProjectsService, ProjectsService>();
        builder.Services.AddSingleton<IProjectUsersService, ProjectUsersService>();
        builder.Services.AddSingleton<IRolesService, RolesService>();
        builder.Services.AddSingleton<ITaskBoardsService, TaskBoardsService>();
        builder.Services.AddSingleton<ITaskDependenciesService, TaskDependenciesService>();
        builder.Services.AddSingleton<ITaskProgressService, TaskProgressService>();
        builder.Services.AddSingleton<ITaskSectionsService, TaskSectionsService>();
        builder.Services.AddSingleton<ITasksService, TasksService>();
        builder.Services.AddSingleton<ITypesService, TypesService>();
        builder.Services.AddSingleton<IUserProjectPermissionsService, UserProjectPermissionsService>();
        builder.Services.AddSingleton<IUsersService, UsersService>();

        builder.Services.AddTransient<ConceptBoardPageModel>();
        builder.Services.AddTransient<TaskBoardPageModel>();
        builder.Services.AddTransient<TaskProgressPageModel>();
        builder.Services.AddTransient<AppSettingsPageModel>();
        builder.Services.AddScoped<MainPageModel>();
        builder.Services.AddScoped<LoginPageModel>();
        builder.Services.AddTransient<ProjectManagementPageModel>();
        builder.Services.AddTransient<UserSettingsPageModel>();

        builder.Services.AddTransient<ConceptBoardPage>();
        builder.Services.AddTransient<TaskBoardPage>();
        builder.Services.AddTransient<TaskProgressPage>();
        builder.Services.AddTransient<AppSettingsPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<ProjectManagementPage>();
        builder.Services.AddTransient<UserSettingsPage>();

        return builder.Build();
	}
}
