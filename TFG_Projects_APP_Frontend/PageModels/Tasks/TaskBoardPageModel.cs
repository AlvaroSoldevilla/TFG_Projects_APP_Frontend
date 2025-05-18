using CommunityToolkit.Mvvm.ComponentModel;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Services.TaskBoardsService;
using TFG_Projects_APP_Frontend.Services.TaskDependeciesService;
using TFG_Projects_APP_Frontend.Services.TasksService;
using TFG_Projects_APP_Frontend.Services.UsersService;

namespace TFG_Projects_APP_Frontend.PageModels.Tasks;

[QueryProperty(nameof(TaskBoard), nameof(TaskBoard))]
public partial class TaskBoardPageModel : ObservableObject
{
    private readonly ITaskBoardsService taskBoardsService;
    private readonly ITasksService tasksService;
    private readonly ITaskDependenciesService taskDependenciesService;
    private readonly UserSession userSession;

    [ObservableProperty]
    private bool _isLoading;

    public TaskBoardPageModel(
        ITaskBoardsService taskBoardsService, 
        ITasksService tasksService, 
        ITaskDependenciesService taskDependenciesService, 
        UserSession userSesion)
    {
        this.taskBoardsService = taskBoardsService;
        this.tasksService = tasksService;
        this.taskDependenciesService = taskDependenciesService;
        this.userSession = userSesion;
    }

    public async Task OnNavigatedTo()
    {
        IsLoading = true;
        // Load data here
        IsLoading = false;
    }
}
