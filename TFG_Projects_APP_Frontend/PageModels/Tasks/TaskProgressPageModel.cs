using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Components.CreateModal;
using TFG_Projects_APP_Frontend.Entities.Dtos.TaskProgress;
using TFG_Projects_APP_Frontend.Entities.Dtos.Tasks;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Services;
using TFG_Projects_APP_Frontend.Services.TaskProgressService;
using TFG_Projects_APP_Frontend.Services.TasksService;
using TFG_Projects_APP_Frontend.Services.UsersService;

namespace TFG_Projects_APP_Frontend.PageModels.Tasks;

[QueryProperty(nameof(TaskSection), nameof(TaskSection))]
public partial class TaskProgressPageModel : ObservableObject
{
    private readonly ITaskProgressService taskProgressService;
    private readonly ITasksService tasksService;
    private readonly UserSession userSession;

    public TaskSection TaskSection { get; set; }

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private ObservableCollection<TaskProgress> _taskProgresses;

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
        await LoadData();
        IsLoading = false;
    }

    public async Task LoadData()
    {
        if (TaskSection == null)
        {
            if (NavigationContext.CurrentTaskSection != null)
            {
                TaskSection = NavigationContext.CurrentTaskSection;
            }
            else
            {
                await Shell.Current.GoToAsync("..");
                IsLoading = false;
                return;
            }
        }

        var taskProgresses = await taskProgressService.getAlltaskProgressByTaskSection(TaskSection.Id);
        taskProgresses = taskProgresses.OrderBy(tp => tp.Order).ToList();
        foreach (var taskprogress in taskProgresses)
        {
            taskprogress.Tasks = await tasksService.GetAllTasksByTaskProgress(taskprogress.Id);
            taskprogress.Tasks = taskprogress.Tasks.OrderBy(t => t.Priority).ToList();
        }
        TaskProgresses = new(taskProgresses);
    }

    [RelayCommand]
    public async Task CreateTaskProgress()
    {
        var taskProgress = await FormDialog.ShowCreateObjectMenuAsync<TaskProgressFormCreate>();

        if (taskProgress != null && !string.IsNullOrEmpty(taskProgress.Name))
        {
            var taskProgressCreate = new TaskProgress
            {
                IdSection = TaskSection.Id,
                Title = taskProgress.Name,
                ModifiesProgress = taskProgress.ModifiesProgress,
                ProgressValue = taskProgress.ProgressValue ?? 0,
                Order = TaskProgresses.Count + 1
            };

            IsLoading = true;

            await taskProgressService.Post(taskProgressCreate);
            await LoadData();
        } 
        else
        {
            if (string.IsNullOrEmpty(taskProgress.Name))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Name is required", "OK");
            }
        }

        IsLoading = false;
    }

    [RelayCommand]
    public async Task TaskCreate(TaskProgress taskProgress)
    {
        var task = await FormDialog.ShowCreateObjectMenuAsync<TaskFormCreate>();
        if (!string.IsNullOrEmpty(task.Title))
        {
            int progressvalue;

            if (taskProgress.ModifiesProgress == true && taskProgress.ProgressValue.HasValue)
            {
                progressvalue = taskProgress.ProgressValue.Value;
            }
            else
            {
                progressvalue = 0;
            }

            var taskCreate = new TaskCreate()
            {
                Title = task.Title,
                Description = task.Description,
                IdSection = taskProgress.IdSection,
                IdProgressSection = taskProgress.Id,
                IdUserCreated = userSession.User.Id,
                IdPriority = 3,
                Progress = progressvalue,
                Finished = false,
                IsParent = false
            };

            IsLoading = true;

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

        IsLoading = false;
    }

    [RelayCommand]
    public async Task MoveProgressLeft(TaskProgress taskProgress)
    {
        if (taskProgress.Order > 1)
        {
            IsLoading = true;
            var taskProgressUpdate = new TaskProgressUpdate
            {
                IdSection = taskProgress.IdSection,
                Title = taskProgress.Title,
                ModifiesProgress = taskProgress.ModifiesProgress,
                ProgressValue = taskProgress.ProgressValue ?? 0,
                Order = taskProgress.Order - 1
            };
            await taskProgressService.Patch(taskProgress.Id, taskProgressUpdate);
            await LoadData();
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task MoveProgressRight(TaskProgress taskProgress)
    {
        if (taskProgress.Order < TaskProgresses.Count)
        {
            IsLoading = true;
            var taskProgressUpdate = new TaskProgressUpdate
            {
                IdSection = taskProgress.IdSection,
                Title = taskProgress.Title,
                ModifiesProgress = taskProgress.ModifiesProgress,
                ProgressValue = taskProgress.ProgressValue ?? 0,
                Order = taskProgress.Order + 1
            };
            await taskProgressService.Patch(taskProgress.Id, taskProgressUpdate);
            await LoadData();
            IsLoading = false;
        }
    }

}
