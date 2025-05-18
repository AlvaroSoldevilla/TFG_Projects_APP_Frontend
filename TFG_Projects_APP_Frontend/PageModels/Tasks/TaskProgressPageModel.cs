using CommunityToolkit.Mvvm.ComponentModel;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Services.TaskProgressService;
using TFG_Projects_APP_Frontend.Services.TasksService;
using TFG_Projects_APP_Frontend.Services.UsersService;

namespace TFG_Projects_APP_Frontend.PageModels.Tasks;

[QueryProperty(nameof(TaskProgress), nameof(TaskProgress))]
public partial class TaskProgressPageModel : ObservableObject
{
    private readonly ITaskProgressService taskProgressService;
    private readonly ITasksService tasksService;
    private readonly UserSession userSession;

    public TaskProgress TaskProgress { get; set; }

    [ObservableProperty]
    private bool _isLoading;

    public TaskProgressPageModel(
        ITaskProgressService taskProgressService, 
        ITasksService tasksService, 
        UserSession userSession)
    {
        this.taskProgressService = taskProgressService;
        this.tasksService = tasksService;
        this.userSession = userSession;
    }

    public async Task OnNavigatedTo()
    {
        IsLoading = true;
        IsLoading = false;
    }
}
