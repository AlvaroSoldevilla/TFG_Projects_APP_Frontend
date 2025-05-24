using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Components.CreateModal;
using TFG_Projects_APP_Frontend.Entities.Dtos.TaskProgress;
using TFG_Projects_APP_Frontend.Entities.Dtos.Tasks;
using TFG_Projects_APP_Frontend.Entities.Dtos.TaskSections;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Services;
using TFG_Projects_APP_Frontend.Services.TaskBoardsService;
using TFG_Projects_APP_Frontend.Services.TaskDependeciesService;
using TFG_Projects_APP_Frontend.Services.TaskProgressService;
using TFG_Projects_APP_Frontend.Services.TaskSectionsService;
using TFG_Projects_APP_Frontend.Services.TasksService;
using TFG_Projects_APP_Frontend.Services.UsersService;

namespace TFG_Projects_APP_Frontend.PageModels.Tasks;

[QueryProperty(nameof(TaskBoard), nameof(TaskBoard))]
public partial class TaskBoardPageModel : ObservableObject
{
    private readonly ITaskBoardsService taskBoardsService;
    private readonly ITaskSectionsService taskSectionsService;
    private readonly ITaskProgressService taskProgressService;
    private readonly ITasksService tasksService;
    private readonly ITaskDependenciesService taskDependenciesService;
    private readonly UserSession userSession;

    public TaskBoard TaskBoard { get; set; }

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private TaskSection _selectedTaskSection;

    [ObservableProperty]
    private ObservableCollection<TaskSection> _taskSections;


    public TaskBoardPageModel(
        ITaskBoardsService taskBoardsService,
        ITaskSectionsService taskSectionsService,
        ITaskProgressService taskProgressService,
        ITasksService tasksService, 
        ITaskDependenciesService taskDependenciesService, 
        UserSession userSesion)
    {
        this.taskBoardsService = taskBoardsService;
        this.taskSectionsService = taskSectionsService;
        this.taskProgressService = taskProgressService;
        this.tasksService = tasksService;
        this.taskDependenciesService = taskDependenciesService;
        this.userSession = userSesion;
    }

    public async Task OnNavigatedTo()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        IsLoading = true;
        SelectedTaskSection = null;

        if (TaskBoard == null)
        {
            if (NavigationContext.CurrentTaskBoard != null)
            {
                TaskBoard = NavigationContext.CurrentTaskBoard;
            }
            else
            {
                await Shell.Current.GoToAsync("..");
                IsLoading = false;
                return;
            }
            return;
        }

        var taskSections = await taskSectionsService.getAllTaskSectionsByTaskBoard(TaskBoard.Id);
        taskSections = taskSections.OrderBy(x => x.Order).ToList();
        foreach (var taskSection in taskSections)
        {
            taskSection.Tasks = await tasksService.GetAllTasksByTaskSection(taskSection.Id);
            taskSection.Tasks = taskSection.Tasks.OrderBy(x => x.Priority).ToList();
        }
        TaskSections = new(taskSections);
        IsLoading = false;
    }

    [RelayCommand]
    public async Task CreateTaskSection()
    {
        var taskSection = await FormDialog.ShowCreateObjectMenuAsync<TaskSectionFormCreate>();
        if (taskSection != null && !string.IsNullOrEmpty(taskSection.Title))
        {
            var taskSectionCreate = new TaskSectionCreate()
            {
                Title = taskSection.Title,
                IdBoard = TaskBoard.Id,
                Order = TaskSections.Count + 1
            };
            var taskSectionReturn = await taskSectionsService.Post(taskSectionCreate);
            var taskProgressCreate = new TaskProgressCreate()
            {
                IdSection = taskSectionReturn.Id,
                Title = "Default",
                Order = 1
            };
            var taskProgressReturn = await taskProgressService.Post(taskProgressCreate);
            var taskSectionUpdate = new TaskSectionUpdate()
            {
                IdBoard = TaskBoard.Id,
                IdDefaultProgress = taskProgressReturn.Id,
                Title = taskSectionReturn.Title,
                Order = taskSectionReturn.Order
            };
            if (await taskSectionsService.Patch(taskSectionReturn.Id, taskSectionUpdate) == "Task section updated")
            {
                taskSectionReturn.IdDefaultProgress = taskProgressReturn.Id;
                var taskSections = TaskSections.ToList();
                taskSections.Add(taskSectionReturn);
                taskSections = taskSections.OrderBy(x => x.Order).ToList();
                TaskSections.Clear();
            }
        }
        else
        {
            if (string.IsNullOrEmpty(taskSection.Title))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Title is required", "OK");
            }
        }
    }

    [RelayCommand]
    public async Task TaskCreate(TaskSection taskSection)
    {
        var task = await FormDialog.ShowCreateObjectMenuAsync<TaskFormCreate>();
        if (!string.IsNullOrEmpty(task.Title))
        {
            var taskCreate = new TaskCreate()
            {
                Title = task.Title,
                Description = task.Description,
                IdSection = taskSection.Id,
                IdProgressSection = taskSection.IdDefaultProgress,
                IdUserCreated = userSession.User.Id,
                IdPriority = 3,
                Progress = 0,
                Finished = false
            };

            await tasksService.Post(taskCreate);

            await LoadData();
        }
        else
        {
            if (string.IsNullOrEmpty(task.Title))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Title is required", "OK");
            }
        }
    }
}
