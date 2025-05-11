using CommunityToolkit.Mvvm.ComponentModel;
using TFG_Projects_APP_Frontend.Services.TaskProgressService;
using TFG_Projects_APP_Frontend.Services.TasksService;

namespace TFG_Projects_APP_Frontend.PageModels.Tasks;

public class TaskProgressPageModel(TaskProgressService taskProgressService, TasksService tasksService) : ObservableObject
{
    private readonly TaskProgressService taskProgressService = taskProgressService;
    private readonly TasksService tasksService = tasksService;
}
