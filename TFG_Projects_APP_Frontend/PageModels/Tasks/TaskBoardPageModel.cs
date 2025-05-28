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
    private List<ProjectTask> AllTasks {get; set;} = new List<ProjectTask>();

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
    private int progressValue = 0;

    [ObservableProperty]
    private int unlockAtValue = 100;


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
                AllTasks.Add(task);
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
    public async Task RemoveParent(ProjectTask task)
    {
        if (task != null)
        {
            if (task.IdParentTask != null)
            {
                var parentTask = AllTasks.FirstOrDefault(x => x.Id == task.IdParentTask);
                task.IdParentTask = null;
                await tasksService.Patch(task.Id, new TaskUpdate
                {
                    IdSection = task.IdSection,
                    IdProgressSection = task.IdProgressSection,
                    IdUserCreated = task.IdUserCreated,
                    Title = task.Title,
                    IdUserAssigned = task.IdUserAssigned,
                    IdParentTask = task.IdParentTask,
                    IdPriority = task.IdPriority,
                    Description = task.Description,
                    Progress = task.Progress,
                    LimitDate = task.LimitDate,
                    CompletionDate = task.CompletionDate,
                    Finished = task.Finished,
                    IsParent = false
                });

                var isParent = AllTasks.Any(x => x.IdParentTask == task.Id);

                await tasksService.Patch(parentTask.Id, new TaskUpdate
                {
                    IdSection = parentTask.IdSection,
                    IdProgressSection = parentTask.IdProgressSection,
                    IdUserCreated = parentTask.IdUserCreated,
                    Title = parentTask.Title,
                    IdUserAssigned = parentTask.IdUserAssigned,
                    IdParentTask = parentTask.IdParentTask,
                    IdPriority = parentTask.IdPriority,
                    Description = parentTask.Description,
                    Progress = parentTask.Progress,
                    LimitDate = parentTask.LimitDate,
                    CompletionDate = parentTask.CompletionDate,
                    Finished = parentTask.Finished,
                    IsParent = isParent
                });
            }
            
            await LoadData();
        }
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

        AppUser? userAssigned;
        ProjectTask? parentTask;

        if (task.UserAssigned != null)
        {
            userAssigned = await usersService.GetById(task.UserAssigned.Id);
        } else
        {
            userAssigned = null;
        }

        if (task.IdParentTask != null)
        {
            parentTask = await tasksService.GetById((int)task.IdParentTask);
        }
        else
        {
            parentTask = null;
        }

        EditingTaskData = new ProjectTask
        {
            Id = task.Id,
            IdSection = task.IdSection,
            IdProgressSection = task.IdProgressSection,
            IdUserAssigned = task.IdUserAssigned,
            IdParentTask = task.IdParentTask,
            IdUserCreated = task.IdUserCreated,
            IdPriority = task.IdPriority,
            Title = task.Title,
            Description = task.Description,
            Progress = task.Progress,
            CreationDate = task.CreationDate,
            LimitDate = task.LimitDate,
            CompletionDate = task.CompletionDate,

            Finished = task.Finished,
            IsParent = task.IsParent,
            Priority = task.Priority,
            UserAssigned = userAssigned,
            Parent = parentTask
        };
        

        progressValue = EditingTaskData.Progress;

        IsEditingTask = true;
        SelectedTask = task;
    }

    [RelayCommand]
    private async void DependencySelected(TaskDependency taskDependency)
    {
        if (SelectedDependency != null)
        {
            unlockAtValue = SelectedDependency.UnlockAt;
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
                    IsLoading = true;
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
                var taskDependencies = await taskDependenciesService.GetAllTaskDependenciesByTask(EditingTaskData.Id);
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
                UnlockAt = unlockAtValue
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
            
            if (string.IsNullOrEmpty(EditingTaskData.Title))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Title is required", "OK");
                return;
            }

            IsLoading = true;
            EditingTaskData.Progress = progressValue;

            int? idUserAsigned;
            DateTime? limitDate;
            DateTime? completionDate;

            if (EditingTaskData.Parent != null)
            {
                EditingTaskData.IdParentTask = EditingTaskData.Parent.Id;
                if (EditingTaskData.IdParentTask != null)
                {
                    var parentReturn = await tasksService.GetById((int)EditingTaskData.IdParentTask);
                    await tasksService.Patch(parentReturn.Id, new TaskUpdate
                    {
                        IdSection = parentReturn.IdSection,
                        IdProgressSection = parentReturn.IdProgressSection,
                        IdUserCreated = parentReturn.IdUserCreated,
                        Title = parentReturn.Title,
                        IdUserAssigned = parentReturn.IdUserAssigned,
                        IdParentTask = parentReturn.IdParentTask,
                        IdPriority = parentReturn.IdPriority,
                        Description = parentReturn.Description,
                        Progress = parentReturn.Progress,
                        LimitDate = parentReturn.LimitDate,
                        CompletionDate = parentReturn.CompletionDate,
                        Finished = parentReturn.Finished,
                        IsParent = true
                    });
                }
            }

            if (EditingTaskData.IdUserAssigned == null)
            {
                idUserAsigned = null;
            } else
            {
                idUserAsigned = EditingTaskData.UserAssigned.Id;
            }

            if (EditingTaskData.CompletionDate != null && EditingTaskData.Progress < 100)
            {
                completionDate = null;
                EditingTaskData.Finished = false;
            }

            if (EditingTaskData.CompletionDate == null && EditingTaskData.Progress == 100)
            {
                completionDate = DateTime.Now;
                EditingTaskData.Finished = true;
            } else
            {
                completionDate = EditingTaskData.CompletionDate;
            }

            var taskUpdate = new TaskUpdate
            {
                IdSection = EditingTaskData.IdSection,
                IdProgressSection = EditingTaskData.IdProgressSection,
                IdUserCreated = EditingTaskData.IdUserCreated,
                Title = EditingTaskData.Title,
                IdUserAssigned = idUserAsigned,
                IdParentTask = EditingTaskData.IdParentTask,
                IdPriority = EditingTaskData.Priority.Id,
                Description = EditingTaskData.Description,
                Progress = EditingTaskData.Progress,
                LimitDate = EditingTaskData.LimitDate,
                CompletionDate = completionDate,
                Finished = EditingTaskData.Finished,
                IsParent = EditingTaskData.IsParent
            };

            
            await tasksService.Patch(EditingTaskData.Id, taskUpdate);

            await LoadData();
            EditingTaskData = null;
            IsEditingTask = false;
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async void SaveTaskSectionChanges()
    {
        if (EditingTaskSectionData != null)
        {
            var taskSectionUpdate = new TaskSectionUpdate
            {
                IdBoard = EditingTaskSectionData.IdBoard,
                IdDefaultProgress = EditingTaskSectionData.IdDefaultProgress,
                Title = EditingTaskSectionData.Title,
                Order = EditingTaskSectionData.Order
            };

            IsLoading = true;
            taskSectionsService.Patch(EditingTaskSectionData.Id, taskSectionUpdate);
            await LoadData();
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async void DeleteTaskSection(TaskSection taskSection)
    {
        if (taskSection != null)
        {
            var result = await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure you want to delete this section?", "Yes", "No");
            if (result)
            {
                IsLoading = true;

                if (taskSection.Order < TaskSections.Count)
                {
                    var nextSections = TaskSections.Where(x => x.Order > taskSection.Order).ToList();
                    foreach (var section in nextSections)
                    {
                        section.Order--;
                        await taskSectionsService.Patch(section.Id, new TaskSectionUpdate
                        {
                            IdBoard = section.IdBoard,
                            IdDefaultProgress = section.IdDefaultProgress,
                            Title = section.Title,
                            Order = section.Order
                        });
                    }
                }

                await taskSectionsService.Delete(taskSection.Id);
                await LoadData();
                IsLoading = false;
            }
        }
    }

    [RelayCommand]
    private async void DeleteTask(ProjectTask task)
    {
        if (task != null)
        {
            var result = await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure you want to delete this task?", "Yes", "No");
            if (result)
            {
                IsLoading = true;
                await tasksService.Delete(task.Id);
                await LoadData();
                IsLoading = false;
            }
        }
    }

}
