using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Components.CreateModal;
using TFG_Projects_APP_Frontend.Entities.Dtos.TaskDependecies;
using TFG_Projects_APP_Frontend.Entities.Dtos.TaskProgress;
using TFG_Projects_APP_Frontend.Entities.Dtos.Tasks;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Services.PrioritiesService;
using TFG_Projects_APP_Frontend.Services.ProjectUsersService;
using TFG_Projects_APP_Frontend.Services.TaskDependeciesService;
using TFG_Projects_APP_Frontend.Services.TaskProgressService;
using TFG_Projects_APP_Frontend.Services.TaskSectionsService;
using TFG_Projects_APP_Frontend.Services.TasksService;
using TFG_Projects_APP_Frontend.Services.UsersService;
using TFG_Projects_APP_Frontend.Services.Utils;

namespace TFG_Projects_APP_Frontend.PageModels.Tasks;

[QueryProperty(nameof(TaskSection), nameof(TaskSection))]
public partial class TaskProgressPageModel : ObservableObject
{
    private readonly ITaskProgressService taskProgressService;
    private readonly ITasksService tasksService;
    private readonly ITaskDependenciesService taskDependenciesService;
    private readonly IProjectUsersService projectUsersService;
    private readonly IUsersService usersService;
    private readonly IPrioritiesService prioritiesService;
    private readonly ITaskSectionsService taskSectionService;
    private readonly UserSession userSession;
    private readonly PermissionsUtils permissionsUtils;

    public TaskSection TaskSection { get; set; }
    private List<ProjectTask> AllTasks { get; set; } = new List<ProjectTask>();

    private ProjectTask _grabbedTask { get; set; }

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _isEditingTask;

    [ObservableProperty]
    private bool _isEditingTaskProgress;

    [ObservableProperty]
    private bool _isEditingTaskDependency;

    [ObservableProperty]
    private ProjectTask _selectedTask;

    [ObservableProperty]
    private ObservableCollection<TaskProgress> _taskProgresses;

    [ObservableProperty]
    private ObservableCollection<Priority> _priorities;

    [ObservableProperty]
    private ObservableCollection<AppUser> _users;

    [ObservableProperty]
    private ObservableCollection<ProjectTask> _possibleDependencies;

    [ObservableProperty]
    private ObservableCollection<TaskDependency> _taskDependencies;

    [ObservableProperty]
    private int progressValue = 0;
    private int initialProgressValue = 0;

    [ObservableProperty]
    private int taskProgressValue = 0;

    [ObservableProperty]
    private int unlockAtValue = 100;
    private int initialUnlockAtValue = 100;


    [ObservableProperty]
    private TaskDependency _selectedDependency;

    [ObservableProperty]
    private ProjectTask _dependencyTaskSelected;

    [ObservableProperty]
    private AppUser _selectedUser;

    [ObservableProperty]
    private TaskProgress _editingTaskProgressData;

    [ObservableProperty]
    private ProjectTask _editingTaskData;

    [ObservableProperty]
    private TaskDependency _editingTaskDependencyData;

    public TaskProgressPageModel(
        ITaskProgressService taskProgressService, 
        ITasksService tasksService,
        ITaskDependenciesService taskDependenciesService,
        IProjectUsersService projectUsersService,
        IUsersService usersService,
        IPrioritiesService prioritiesService,
        ITaskSectionsService taskSectionService,
        UserSession userSession,
        PermissionsUtils permissionsUtils)
    {
        this.taskProgressService = taskProgressService;
        this.tasksService = tasksService;
        this.taskDependenciesService = taskDependenciesService;
        this.projectUsersService = projectUsersService;
        this.usersService = usersService;
        this.prioritiesService = prioritiesService;
        this.taskSectionService = taskSectionService;
        this.userSession = userSession;
        this.permissionsUtils = permissionsUtils;
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

        var priorities = await prioritiesService.GetAll();
        Priorities = new ObservableCollection<Priority>(priorities.OrderBy(x => x.PriorityValue));

        var taskProgresses = await taskProgressService.getAlltaskProgressByTaskSection(TaskSection.Id);
        taskProgresses = taskProgresses.OrderBy(tp => tp.Order).ToList();
        foreach (var taskProgress in taskProgresses)
        {
            var returnTasks = await tasksService.GetAllTasksByTaskProgress(taskProgress.Id);
            AllTasks.AddRange(returnTasks);
        }
        foreach (var taskprogress in taskProgresses)
        {
            var realTasks = new List<ProjectTask>();
            taskprogress.Tasks = await tasksService.GetAllTasksByTaskProgress(taskprogress.Id);
            foreach(var task in taskprogress.Tasks)
            {
                if (!task.IsParent)
                {
                    var taskDependencies = await taskDependenciesService.GetAllTaskDependenciesByTask(task.Id);
                    if (taskDependencies != null && taskDependencies.Count > 0)
                    {
                        var unlocked = true;
                       foreach (var dependency in taskDependencies)
                       {
                            var dependencyTask = await tasksService.GetById(dependency.IdDependsOn);
                            if (dependencyTask.Progress < dependency.UnlockAt)
                            {
                                unlocked = false;
                            }
                       }
                       if (unlocked)
                        {
                            task.Dependecies = taskDependencies;
                            realTasks.Add(task);
                        }
                    }
                    else
                    {
                        realTasks.Add(task);
                    }
                    
                } 
            }

            taskprogress.Tasks.Clear();

            foreach (var task in realTasks)
            {
                if (task.IdUserAssigned != null)
                {
                    task.UserAssigned = await usersService.GetById(task.IdUserAssigned.Value);
                }
                task.Priority = priorities.FirstOrDefault(p => p.Id == task.IdPriority);
                if (task.Priority == null)
                {
                    var minPriority = priorities.OrderBy(p => p.PriorityValue).LastOrDefault();
                    task.Priority = minPriority;
                }
                taskprogress.Tasks.Add(task);
            }

            taskprogress.Tasks = taskprogress.Tasks.OrderBy(t => t.Priority.PriorityValue).ToList();
        }
        if (TaskProgresses != null)
        {
            TaskProgresses.Clear();
        }
        TaskProgresses = new(taskProgresses);
    }

    [RelayCommand]
    public async Task CreateTaskProgress()
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.CreateTaskBoardSections, PermissionsUtils.Permissions.FullTaskPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            var taskProgress = await FormDialog.ShowCreateObjectMenuAsync<TaskProgressFormCreate>("Create Progress Section");

            if (taskProgress != null && !string.IsNullOrEmpty(taskProgress.Name))
            {
                if (taskProgress.ProgressValue < 0 || taskProgress.ProgressValue > 100)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Progress value must be between 0 and 100", "OK");
                    return;
                }

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
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "You don't have permission to do that", "OK");
        }
    }

    [RelayCommand]
    public async Task TaskCreate(TaskProgress taskProgress)
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.CreateTasks, PermissionsUtils.Permissions.FullTaskPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            var task = await FormDialog.ShowCreateObjectMenuAsync<TaskFormCreate>("Create Task");
            if (!string.IsNullOrEmpty(task.Title))
            {
                int progressvalue;

                if (taskProgress.ModifiesProgress)
                {
                    progressvalue = taskProgress.ProgressValue;
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
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "You don't have permission to do that", "OK");
        }
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
    public async Task MoveProgressLeft(TaskProgress taskProgress)
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.EditTaskBoardSections, PermissionsUtils.Permissions.FullTaskPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            if (taskProgress.Order > 1)
            {
                IsLoading = true;
                var taskProgressUpdate = new TaskProgressUpdate
                {
                    IdSection = taskProgress.IdSection,
                    Title = taskProgress.Title,
                    ModifiesProgress = taskProgress.ModifiesProgress,
                    ProgressValue = taskProgress.ProgressValue,
                    Order = taskProgress.Order - 1
                };
                await taskProgressService.Patch(taskProgress.Id, taskProgressUpdate);
                await LoadData();
                IsLoading = false;
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "You don't have permission to do that", "OK");
        }
    }

    [RelayCommand]
    public async Task MoveProgressRight(TaskProgress taskProgress)
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.EditTaskBoardSections, PermissionsUtils.Permissions.FullTaskPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            if (taskProgress.Order < TaskProgresses.Count)
            {
                IsLoading = true;
                var taskProgressUpdate = new TaskProgressUpdate
                {
                    IdSection = taskProgress.IdSection,
                    Title = taskProgress.Title,
                    ModifiesProgress = taskProgress.ModifiesProgress,
                    ProgressValue = taskProgress.ProgressValue,
                    Order = taskProgress.Order + 1
                };
                await taskProgressService.Patch(taskProgress.Id, taskProgressUpdate);
                await LoadData();
                IsLoading = false;
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "You don't have permission to do that", "OK");
        }
    }

    [RelayCommand]
    public async void EditTaskProgress(TaskProgress taskProgress)
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.EditTaskBoardSections, PermissionsUtils.Permissions.FullTaskPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            bool confirmed = true;
            if (EditingTaskData != null)
            {
                if (SelectedTask != null)
                {
                    if (HasChangedTaskData(SelectedTask))
                    {
                        confirmed = await Application.Current.MainPage.DisplayAlert(
                        "Change edit",
                        "You have unsaved changes, are you sure you want to edit another task?",
                        "Accept",
                        "Cancel"
                        );
                    }
                }
            }
            if (confirmed)
            {
                TaskProgressValue = taskProgress.ProgressValue;
                EditingTaskData = null;
                IsEditingTask = false;

                IsEditingTaskProgress = true;
                EditingTaskProgressData = new TaskProgress
                {
                    Id = taskProgress.Id,
                    IdSection = taskProgress.IdSection,
                    Title = taskProgress.Title,
                    ModifiesProgress = taskProgress.ModifiesProgress,
                    ProgressValue = taskProgress.ProgressValue,
                    Order = taskProgress.Order
                };
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "You don't have permission to do that", "OK");
        }
    }

    [RelayCommand]
    public async Task EditTask(ProjectTask task)
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.EditTasks, PermissionsUtils.Permissions.FullTaskPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            bool confirmed = true;
            if (EditingTaskData != null)
            {
                if (SelectedTask != null)
                {
                    if (HasChangedTaskData(SelectedTask))
                    {
                        confirmed = await Application.Current.MainPage.DisplayAlert(
                        "Change edit",
                        "You have unsaved changes, are you sure you want to edit another task?",
                        "Accept",
                        "Cancel"
                        );
                    }
                }
            }
            if (confirmed)
            {
                EditingTaskProgressData = null;
                IsEditingTaskProgress = false;

                var returnUsers = await usersService.GetUsersByProject(NavigationContext.CurrentProject.Id);
                Users = new ObservableCollection<AppUser>(returnUsers);

                List<ProjectTask> tasks = new();
                List<ProjectTask> parentCandidates = new();
                List<TaskDependency> dependencies = new();


                foreach (var tasprogress in TaskProgresses)
                {
                    if (tasprogress.Tasks != null && tasprogress.Tasks.Count != 0)
                    {
                        foreach (var t in tasprogress.Tasks)
                        {
                            if (t.Id != task.Id)
                            {
                                tasks.Add(t);
                            }
                        }
                    }
                }

                var currenttaskProgress = TaskProgresses.FirstOrDefault(x => x.Id == task.IdProgressSection);

                var currentDependencies = await taskDependenciesService.GetAllTaskDependenciesByTask(task.Id);

                foreach (var dependency in currentDependencies)
                {
                    var dependsOnTask = await tasksService.GetById(dependency.IdDependsOn);

                    dependency.DisplayName = dependsOnTask.Title + "->" + task.Title;
                }


                PossibleDependencies = new ObservableCollection<ProjectTask>(tasks);
                TaskDependencies = new ObservableCollection<TaskDependency>(currentDependencies);

                AppUser? userAssigned;

                if (task.UserAssigned != null)
                {
                    userAssigned = await usersService.GetById(task.UserAssigned.Id);
                }
                else
                {
                    userAssigned = null;
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
                };


                ProgressValue = EditingTaskData.Progress;
                initialProgressValue = EditingTaskData.Progress;

                IsEditingTask = true;
                SelectedTask = task;
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "You don't have permission to do that", "OK");
        }
    }

    [RelayCommand]
    private async void DependencySelected(TaskDependency taskDependency)
    {
        if (SelectedDependency != null)
        {
            UnlockAtValue = SelectedDependency.UnlockAt;
            initialUnlockAtValue = SelectedDependency.UnlockAt;
            IsEditingTaskDependency = true;
            EditingTaskDependencyData = new TaskDependency
            {
                Id = SelectedDependency.Id,
                IdTask = SelectedDependency.IdTask,
                IdDependsOn = SelectedDependency.IdDependsOn,
                UnlockAt = SelectedDependency.UnlockAt,
                DisplayName = SelectedDependency.DisplayName
            };
        }
        else
        {
            IsEditingTaskDependency = false;
            EditingTaskDependencyData = null;
        }
    }

    [RelayCommand]
    private async void CloseEditingtTaskProgress()
    {
        EditingTaskProgressData = null;
        IsEditingTaskProgress = false;
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
            int? priorityId;

            if (EditingTaskData.Parent != null)
            {
                EditingTaskData.IdParentTask = EditingTaskData.Parent.Id;
                if (EditingTaskData.IdParentTask != null)
                {
                    var parentReturn = await tasksService.GetById((int)EditingTaskData.IdParentTask);
                    await tasksService.Patch(parentReturn.Id, new TaskUpdate
                    {
                        Id = parentReturn.Id,
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
            }
            else
            {
                idUserAsigned = EditingTaskData.UserAssigned.Id;
            }

            if (EditingTaskData.Priority == null)
            {
                priorityId = null;
            }
            else
            {
                priorityId = EditingTaskData.Priority.Id;
            }

            var taskUpdate = new TaskUpdate
            {
                IdSection = EditingTaskData.IdSection,
                IdProgressSection = EditingTaskData.IdProgressSection,
                IdUserCreated = EditingTaskData.IdUserCreated,
                Title = EditingTaskData.Title,
                IdUserAssigned = idUserAsigned,
                IdParentTask = EditingTaskData.IdParentTask,
                IdPriority = priorityId,
                Description = EditingTaskData.Description,
                Progress = EditingTaskData.Progress,
                LimitDate = EditingTaskData.LimitDate,
                CompletionDate = EditingTaskData.CompletionDate,
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
    private async void SaveTaskProgressChanges()
    {
        if (EditingTaskProgressData != null)
        {
            var taskProgressUpdate = new TaskProgressUpdate
            {
                IdSection = EditingTaskProgressData.IdSection,
                Title = EditingTaskProgressData.Title,
                ModifiesProgress = EditingTaskProgressData.ModifiesProgress,
                ProgressValue = TaskProgressValue,
                Order = EditingTaskProgressData.Order
            };

            IsLoading = true;
            taskProgressService.Patch(EditingTaskProgressData.Id, taskProgressUpdate);
            await LoadData();
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async void DeleteTaskProgress(TaskProgress taskProgress)
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.DeleteTaskBoardSections, PermissionsUtils.Permissions.FullTaskPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            if (taskProgress != null)
            {
                var result = await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure you want to delete this section?", "Yes", "No");
                if (result)
                {
                    IsLoading = true;

                    var taskSection = await taskSectionService.GetById(taskProgress.IdSection);
                    if (taskSection.IdDefaultProgress == taskProgress.Id)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "You cannot delete the default progress section", "OK");
                        IsLoading = false;
                        return;
                    }

                    if (taskProgress.Order < TaskProgresses.Count)
                    {
                        var nextSections = TaskProgresses.Where(x => x.Order > taskProgress.Order).ToList();
                        foreach (var section in nextSections)
                        {
                            section.Order--;
                            await taskProgressService.Patch(section.Id, new TaskProgressUpdate
                            {
                                IdSection = section.IdSection,
                                Title = section.Title,
                                ModifiesProgress = section.ModifiesProgress,
                                ProgressValue = section.ProgressValue,
                                Order = section.Order
                            });
                        }
                    }

                    if (taskProgress.Tasks != null && taskProgress.Tasks.Count > 0)
                    {
                        foreach (var task in taskProgress.Tasks)
                        {
                            await tasksService.Patch(task.Id, new TaskUpdate
                            {
                                IdSection = task.IdSection,
                                IdProgressSection = taskSection.IdDefaultProgress,
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
                                IsParent = task.IsParent
                            });
                        }
                    }

                    await taskProgressService.Delete(taskProgress.Id);
                    await LoadData();
                    IsLoading = false;
                }
            }

            EditingTaskProgressData = null;
            IsEditingTaskProgress = false;
            EditingTaskData = null;
            IsEditingTask = false;
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "You don't have permission to do that", "OK");
        }
    }

    [RelayCommand]
    private async void DeleteTask(ProjectTask task)
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.DeleteTasks, PermissionsUtils.Permissions.FullTaskPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
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

            EditingTaskData = null;
            IsEditingTask = false;
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "You don't have permission to do that", "OK");
        }
    }

    private bool HasChangedTaskData(ProjectTask task)
    {
        if (EditingTaskData == null || task == null)
        {
            return false;
        }
        if (initialProgressValue != ProgressValue || initialUnlockAtValue != UnlockAtValue)
        {
            return true;
        }
        return EditingTaskData.Id != task.Id ||
               EditingTaskData.Title != task.Title ||
               EditingTaskData.Description != task.Description ||
               EditingTaskData.Progress != task.Progress ||
               EditingTaskData.LimitDate != task.LimitDate ||
               EditingTaskData.CompletionDate != task.CompletionDate ||
               EditingTaskData.IdUserAssigned != task.IdUserAssigned ||
               EditingTaskData.IdPriority != task.IdPriority;
    }

    [RelayCommand]
    public async void DropOnProgressSection(TaskProgress taskProgress)
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.EditTasks, PermissionsUtils.Permissions.FullTaskPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            if (_grabbedTask != null && taskProgress != null)
            {
                var progress = _grabbedTask.Progress;
                if (taskProgress.ModifiesProgress)
                {
                    progress = taskProgress.ProgressValue;
                }

                var taskUpdate = new TaskUpdate
                {
                    Id = _grabbedTask.Id,
                    IdSection = _grabbedTask.IdSection,
                    IdProgressSection = taskProgress.Id,
                    IdUserCreated = _grabbedTask.IdUserCreated,
                    Title = _grabbedTask.Title,
                    IdUserAssigned = _grabbedTask.IdUserAssigned,
                    IdParentTask = _grabbedTask.Id,
                    IdPriority = _grabbedTask.IdPriority,
                    Description = _grabbedTask.Description,
                    Progress = progress,
                    LimitDate = _grabbedTask.LimitDate,
                    CompletionDate = _grabbedTask.CompletionDate,
                    Finished = _grabbedTask.Finished,
                    IsParent = _grabbedTask.IsParent
                };

                IsLoading = true;
                await tasksService.Patch(_grabbedTask.Id, taskUpdate);
                await LoadData();
                IsLoading = false;
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "You don't have permission to do that", "OK");
        }
        _grabbedTask = null;
    }

    [RelayCommand]
    public async void DroppedOnTask(ProjectTask task)
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.EditTasks, PermissionsUtils.Permissions.FullTaskPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            if (_grabbedTask != null && task != null)
            {
                int progressSectionId = _grabbedTask.IdProgressSection;
                var progress = _grabbedTask.Progress;


                if (task.IdProgressSection != progressSectionId)
                {
                    var progressSection = await taskProgressService.GetById(task.IdProgressSection);
                    progressSectionId = progressSection.Id;
                    if (progressSection.ModifiesProgress)
                    {
                        progress = progressSection.ProgressValue;
                    }
                }

                await tasksService.Patch(_grabbedTask.Id, new TaskUpdate
                {
                    Id = _grabbedTask.Id,
                    IdSection = _grabbedTask.IdSection,
                    IdProgressSection = progressSectionId,
                    IdUserCreated = _grabbedTask.IdUserCreated,
                    Title = _grabbedTask.Title,
                    IdUserAssigned = _grabbedTask.IdUserAssigned,
                    IdParentTask = _grabbedTask.Id,
                    IdPriority = _grabbedTask.IdPriority,
                    Description = _grabbedTask.Description,
                    Progress = progress,
                    LimitDate = _grabbedTask.LimitDate,
                    CompletionDate = _grabbedTask.CompletionDate,
                    Finished = _grabbedTask.Finished,
                    IsParent = _grabbedTask.IsParent
                });

                await LoadData();
                IsLoading = false;
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "You don't have permission to do that", "OK");
        }

        _grabbedTask = null;
    }

    [RelayCommand]
    private void GrabTask(ProjectTask task)
    {
        _grabbedTask = task;
    }

}
