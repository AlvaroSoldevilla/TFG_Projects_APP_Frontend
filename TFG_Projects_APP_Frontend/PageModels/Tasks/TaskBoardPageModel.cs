using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Components.CreateModal;
using TFG_Projects_APP_Frontend.Entities.Dtos.TaskDependecies;
using TFG_Projects_APP_Frontend.Entities.Dtos.TaskProgress;
using TFG_Projects_APP_Frontend.Entities.Dtos.Tasks;
using TFG_Projects_APP_Frontend.Entities.Dtos.TaskSections;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Services;
using TFG_Projects_APP_Frontend.Services.PrioritiesService;
using TFG_Projects_APP_Frontend.Services.ProjectUsersService;
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
    private readonly IProjectUsersService projectUsersService;
    private readonly IUsersService usersService;
    private readonly IPrioritiesService prioritiesService;
    private readonly UserSession userSession;

    public TaskBoard TaskBoard { get; set; }

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _isEditingTaskSection;

    [ObservableProperty]
    private bool _isEditingTask;

    [ObservableProperty]
    private bool _isEditingTaskDependency;

    [ObservableProperty]
    private TaskSection _selectedTaskSection;

    [ObservableProperty]
    private ProjectTask _selectedTask;

    [ObservableProperty]
    private ObservableCollection<TaskSection> _taskSections;

    [ObservableProperty]
    private ObservableCollection<Priority> _priorities;

    [ObservableProperty]
    private ObservableCollection<AppUser> _users;

    [ObservableProperty]
    private ObservableCollection<ProjectTask> _possibleDependencies;

    [ObservableProperty]
    private ObservableCollection<TaskDependency> _taskDependencies;

    [ObservableProperty]
    private ObservableCollection<ProjectTask> _possibleParents;


    [ObservableProperty]
    private TaskDependency _selectedDependency;

    [ObservableProperty]
    private ProjectTask _dependencyTaskSelected;

    [ObservableProperty]
    private AppUser _selectedUser;

    [ObservableProperty]
    private TaskSection _editingTaskSectionData;

    [ObservableProperty]
    private ProjectTask _editingTaskData;

    [ObservableProperty]
    private TaskDependency _editingTaskDependencyData;


    public TaskBoardPageModel(
        ITaskBoardsService taskBoardsService,
        ITaskSectionsService taskSectionsService,
        ITaskProgressService taskProgressService,
        ITasksService tasksService,
        ITaskDependenciesService taskDependenciesService,
        IProjectUsersService projectUsersService,
        IUsersService usersService,
        IPrioritiesService prioritiesService,
        UserSession userSesion)
    {
        this.taskBoardsService = taskBoardsService;
        this.taskSectionsService = taskSectionsService;
        this.taskProgressService = taskProgressService;
        this.tasksService = tasksService;
        this.taskDependenciesService = taskDependenciesService;
        this.projectUsersService = projectUsersService;
        this.usersService = usersService;
        this.prioritiesService = prioritiesService;
        this.userSession = userSesion;
    }

    public async Task OnNavigatedTo()
    {
        IsLoading = true;
        SelectedTaskSection = null;

        await LoadData();

        IsLoading = false;
    }

    private async Task LoadData()
    {

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
        }

        var priorities = await prioritiesService.GetAll();
        Priorities = new ObservableCollection<Priority>(priorities.OrderBy(x => x.PriorityValue));

        var taskSections = await taskSectionsService.getAllTaskSectionsByTaskBoard(TaskBoard.Id);
        taskSections = taskSections.OrderBy(x => x.Order).ToList();
        foreach (var taskSection in taskSections)
        {
            taskSection.Tasks = await tasksService.GetAllTasksByTaskSection(taskSection.Id);
            foreach (var task in taskSection.Tasks)
            {
                if (task.IdUserAssigned != null)
                {
                    task.UserAssigned = await tasksService.getUserAssigned(task.Id);
                }
                task.Priority = priorities.FirstOrDefault(x => x.Id == task.IdPriority);
                if (task.Priority == null)
                {
                    var minPriority = priorities.OrderBy(x => x.PriorityValue).LastOrDefault();
                    task.Priority = minPriority;
                }
            }

            foreach (var task in taskSection.Tasks)
            {
                var dependencies = await taskDependenciesService.GetAllTaskDependenciesByTask(task.Id);
                if (dependencies != null && dependencies.Count > 0)
                {
                    task.Dependecies = dependencies;
                    task.Parent = taskSection.Tasks.FirstOrDefault(x => x.Id == task.IdParentTask);
                    foreach (var dependency in dependencies)
                    {
                        dependency.DependsOn = taskSection.Tasks.FirstOrDefault(x => x.Id == dependency.IdDependsOn);
                    }
                }
            }

            taskSection.Tasks = taskSection.Tasks.OrderBy(x => x.Priority.PriorityValue).ToList();
        }

        TaskSections = new(taskSections);
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

            IsLoading = true;

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

        IsLoading = false;
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
    public async Task MoveSectionLeft(TaskSection taskSection)
    {
        if (taskSection.Order > 1)
        {
            var previousSection = TaskSections.FirstOrDefault(x => x.Order == taskSection.Order - 1);
            if (previousSection != null)
            {
                taskSection.Order--;
                previousSection.Order++;
                await taskSectionsService.Patch(taskSection.Id, new TaskSectionUpdate
                {
                    IdBoard = taskSection.IdBoard,
                    IdDefaultProgress = taskSection.IdDefaultProgress,
                    Title = taskSection.Title,
                    Order = taskSection.Order
                });
                await taskSectionsService.Patch(previousSection.Id, new TaskSectionUpdate
                {
                    IdBoard = previousSection.IdBoard,
                    IdDefaultProgress = previousSection.IdDefaultProgress,
                    Title = previousSection.Title,
                    Order = previousSection.Order
                });
                await LoadData();
            }
        }
    }

    [RelayCommand]
    public async Task MoveSectionRight(TaskSection taskSection)
    {
        if (taskSection.Order < TaskSections.Count)
        {
            var nextSection = TaskSections.FirstOrDefault(x => x.Order == taskSection.Order + 1);
            if (nextSection != null)
            {
                taskSection.Order++;
                nextSection.Order--;
                await taskSectionsService.Patch(taskSection.Id, new TaskSectionUpdate
                {
                    IdBoard = taskSection.IdBoard,
                    IdDefaultProgress = taskSection.IdDefaultProgress,
                    Title = taskSection.Title,
                    Order = taskSection.Order
                });
                await taskSectionsService.Patch(nextSection.Id, new TaskSectionUpdate
                {
                    IdBoard = nextSection.IdBoard,
                    IdDefaultProgress = nextSection.IdDefaultProgress,
                    Title = nextSection.Title,
                    Order = nextSection.Order
                });
                await LoadData();
            }
        }
    }

    [RelayCommand]
    public async Task TaskSectionSelected(TaskSection taskSection)
    {
        NavigationContext.CurrentTaskSection = SelectedTaskSection;
        await Shell.Current.GoToAsync("TaskProgressPage", new Dictionary<string, object>
        {
            {"TaskSection", taskSection }
        });
    }

    [RelayCommand]
    public async void EditTaskSection(TaskSection taskSection)
    {
        EditingTaskData = null;
        IsEditingTask = false;

        IsEditingTaskSection = true;
        EditingTaskSectionData = new TaskSection
        {
            Id = taskSection.Id,
            Title = taskSection.Title,
            IdBoard = taskSection.IdBoard,
            IdDefaultProgress = taskSection.IdDefaultProgress,
            Order = taskSection.Order
        };
    }

    [RelayCommand]
    public async Task EditTask(ProjectTask task)
    {
        EditingTaskSectionData = null;
        IsEditingTaskSection = false;

        IsEditingTask = true;
        SelectedTask = task;

        var returnUsers = await usersService.GetUsersByProject(NavigationContext.CurrentProject.Id);
        Users = new ObservableCollection<AppUser>(returnUsers);

        List<ProjectTask> tasks = new();
        List<ProjectTask> parentCandidates = new();
        List<TaskDependency> dependencies = new();


        foreach (var taskSection in TaskSections)
        {
            if (taskSection.Tasks != null && taskSection.Tasks.Count != 0)
            { 
                foreach (var t in taskSection.Tasks)
                {
                    if (t.Id != task.Id)
                    {
                        tasks.Add(t);
                    }
                }
            }
        }

        var currentTaskSection = TaskSections.FirstOrDefault(x => x.Id == task.IdSection);

        if (currentTaskSection.Tasks != null && currentTaskSection.Tasks.Count != 0)
        {
            foreach (var t in currentTaskSection.Tasks)
            {
                if (t.Id != task.Id)
                {
                    if (t.IsParent || t.Parent == null)
                    {
                        parentCandidates.Add(t);
                    }
                }
            }
        }

        var currentDependencies = await taskDependenciesService.GetAllTaskDependenciesByTask(task.Id);

        foreach (var dependency in currentDependencies)
        {
            var dependsOnTask = await tasksService.GetById(dependency.IdDependsOn);

            dependency.DisplayName = dependsOnTask.Title + "->" + task.Title;
        }


        PossibleDependencies = new ObservableCollection<ProjectTask>(tasks);
        PossibleParents = new ObservableCollection<ProjectTask>(parentCandidates);
        TaskDependencies = new ObservableCollection<TaskDependency>(currentDependencies);

        if (task.UserAssigned != null)
        {
            EditingTaskData = new ProjectTask
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IdSection = task.IdSection,
                IdProgressSection = task.IdProgressSection,
                IdUserCreated = task.IdUserCreated,
                IdUserAssigned = task.IdUserAssigned,
                IdPriority = task.IdPriority,
                Progress = task.Progress,
                Finished = task.Finished,
                IsParent = task.IsParent,
                Priority = task.Priority,
                UserAssigned = task.UserAssigned
            };
        } else
        {
            EditingTaskData = new ProjectTask
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IdSection = task.IdSection,
                IdProgressSection = task.IdProgressSection,
                IdUserCreated = task.IdUserCreated,
                IdUserAssigned = task.IdUserAssigned,
                IdPriority = task.IdPriority,
                Progress = task.Progress,
                Finished = task.Finished,
                IsParent = task.IsParent,
                Priority = task.Priority
            };
        }
    }

    [RelayCommand]
    private async void DependencySelected(TaskDependency taskDependency)
    {
        if (SelectedDependency != null)
        {
            IsEditingTaskDependency = true;
            EditingTaskDependencyData = new TaskDependency
            {
                Id = SelectedDependency.Id,
                IdTask = SelectedDependency.IdTask,
                IdDependsOn = SelectedDependency.IdDependsOn,
                UnlockAt = SelectedDependency.UnlockAt,
                DisplayName = SelectedDependency.DisplayName
            };
        } else
        {
            IsEditingTaskDependency = false;
            EditingTaskDependencyData = null;
        }
    }

    [RelayCommand]
    private async void CloseEditingtTaskSection()
    {
        EditingTaskSectionData = null;
        IsEditingTaskSection = false;
    }

    [RelayCommand]
    private async void CloseEditingtTask()
    {
        EditingTaskData = null;
        IsEditingTask = false;
    }

    [RelayCommand]
    private async void AddDependency(ProjectTask task)
    {
        if (EditingTaskData != null)
        {
            if (DependencyTaskSelected == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You need to select a task", "OK");
            }
            else
            {
                var currentDependencies = await taskDependenciesService.GetAllTaskDependenciesByTask(EditingTaskData.Id);
                if (currentDependencies.Any(x => x.IdDependsOn == DependencyTaskSelected.Id))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "This task is already a dependency", "OK");
                }
                else
                {
                    var dependency = new TaskDependencyCreate
                    {
                        IdTask = EditingTaskData.Id,
                        IdDependsOn = DependencyTaskSelected.Id,
                        UnlockAt = 100
                    };

                    var returnTaskDependency = await taskDependenciesService.Post(dependency);
                    returnTaskDependency.DisplayName = DependencyTaskSelected.Title + "->" + EditingTaskData.Title;

                    var taskDependencies = TaskDependencies.ToList();
                    taskDependencies.Add(returnTaskDependency);
                    TaskDependencies.Clear();
                    TaskDependencies = new ObservableCollection<TaskDependency>(taskDependencies);

                    IsLoading = true;
                    await LoadData();
                    IsLoading = false;
                }
            }
        }
    }

    [RelayCommand]
    private async void RemoveDependency(TaskDependency taskDependency)
    {
        if (SelectedDependency != null)
        {
            var result = await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure you want to remove this dependency?", "Yes", "No");
            if (result)
            {
                IsLoading = true;
                await taskDependenciesService.Delete(SelectedDependency.Id);
                var taskDependencies = TaskDependencies.ToList();
                taskDependencies.Remove(taskDependency);
                TaskDependencies.Clear();
                TaskDependencies = new ObservableCollection<TaskDependency>(taskDependencies);
                SelectedDependency = null;
                await LoadData();
                IsLoading = false;
            }
        }
    }

    [RelayCommand]
    private async void SaveTaskDependencyChanges()
    {
        if (EditingTaskDependencyData != null)
        {
            IsLoading = true;
            var taskDependencyUpdate = new TaskDependencyUpdate
            {
                IdTask = EditingTaskDependencyData.IdTask,
                IdDependsOn = EditingTaskDependencyData.IdDependsOn,
                UnlockAt = EditingTaskDependencyData.UnlockAt
            };
            await taskDependenciesService.Patch(EditingTaskDependencyData.Id, taskDependencyUpdate);
            IsEditingTaskDependency = false;
            EditingTaskDependencyData = null;
            SelectedDependency = null;
            await LoadData();
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async void SaveTaskChanges()
    {
        if (EditingTaskData != null)
        {
            
            await LoadData();
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async void SaveTaskSectionChanges()
    {
        if (EditingTaskSectionData != null)
        {

            await LoadData();
            IsLoading = false;
        }
    }

}
