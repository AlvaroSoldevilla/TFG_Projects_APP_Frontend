using CommunityToolkit.Mvvm.ComponentModel;
using TFG_Projects_APP_Frontend.Services.TaskBoardsService;
using TFG_Projects_APP_Frontend.Services.TaskDependeciesService;
using TFG_Projects_APP_Frontend.Services.TasksService;

namespace TFG_Projects_APP_Frontend.PageModels.Tasks;

public class TaskBoardPageModel(TaskBoardsService taskBoardsService, TasksService tasksService, TaskDependenciesService taskDependenciesService) : ObservableObject
{
    private readonly TaskBoardsService taskBoardsService = taskBoardsService;
    private readonly TasksService tasksService = tasksService;
    private readonly TaskDependenciesService taskDependenciesService = taskDependenciesService;
}
