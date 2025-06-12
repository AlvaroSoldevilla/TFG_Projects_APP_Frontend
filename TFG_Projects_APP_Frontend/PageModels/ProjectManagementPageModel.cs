using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Components.CreateModal;
using TFG_Projects_APP_Frontend.Entities.Dtos.ConceptBoards;
using TFG_Projects_APP_Frontend.Entities.Dtos.Concepts;
using TFG_Projects_APP_Frontend.Entities.Dtos.ProjectUsers;
using TFG_Projects_APP_Frontend.Entities.Dtos.TaskBoards;
using TFG_Projects_APP_Frontend.Entities.Dtos.TaskProgress;
using TFG_Projects_APP_Frontend.Entities.Dtos.TaskSections;
using TFG_Projects_APP_Frontend.Entities.Dtos.UserProjectPermissions;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Properties;
using TFG_Projects_APP_Frontend.Services.ConceptBoardsService;
using TFG_Projects_APP_Frontend.Services.ConceptsService;
using TFG_Projects_APP_Frontend.Services.PermissionsService;
using TFG_Projects_APP_Frontend.Services.ProjectsService;
using TFG_Projects_APP_Frontend.Services.ProjectUsersService;
using TFG_Projects_APP_Frontend.Services.RolesService;
using TFG_Projects_APP_Frontend.Services.TaskBoardsService;
using TFG_Projects_APP_Frontend.Services.TaskProgressService;
using TFG_Projects_APP_Frontend.Services.TaskSectionsService;
using TFG_Projects_APP_Frontend.Services.UserProjectPermissionsService;
using TFG_Projects_APP_Frontend.Services.UsersService;
using TFG_Projects_APP_Frontend.Utils;

namespace TFG_Projects_APP_Frontend.PageModels;

public partial class ProjectManagementPageModel : ObservableObject
{
    private readonly IConceptsService conceptsService;
    private readonly IConceptBoardsService conceptBoardsService;
    private readonly ITaskBoardsService taskBoardsService;
    private readonly ITaskSectionsService taskSectionsService;
    private readonly ITaskProgressService taskProgressService;
    private readonly IUserProjectPermissionsService userProjectPermissionsService;
    private readonly IPermissionsService permissionsService;
    private readonly IProjectsService projectsService;
    private readonly IProjectUsersService projectUsersService;
    private readonly IUsersService usersService;
    private readonly IRolesService rolesService;
    private readonly UserSession userSession;
    private readonly PermissionsUtils permissionsUtils;

    public Project Project { get; set; }

    private bool _isLoadingData = false;

    [ObservableProperty]
    private bool _isLoadingConcepts;

    [ObservableProperty]
    private bool _isLoadingTaskBoards;

    [ObservableProperty]
    private bool _isLoadingUsers;

    [ObservableProperty]
    private bool _isEditingUser;

    [ObservableProperty]
    private bool _isEditingConcept;

    [ObservableProperty]
    private bool _isEditingTaskBoard;

    [ObservableProperty]
    private bool _isLookingForUser = false;

    [ObservableProperty]
    private string _adduserEmail = string.Empty;

    [ObservableProperty]
    private bool _canSeeConcepts = false;

    [ObservableProperty]
    private bool _canSeeTaskBoards = false;

    [ObservableProperty]
    private bool canSeeUsers = false;

    [ObservableProperty]
    private Project _currentProject;

    [ObservableProperty]
    private Concept _selectedConcept;

    [ObservableProperty]
    private AppUser _selectedUser;

    [ObservableProperty]
    private TaskBoard _selectedTaskBoard;

    [ObservableProperty]
    private ObservableCollection<Concept> _concepts = new();

    [ObservableProperty]
    private ObservableCollection<TaskBoard> _taskBoards = new();

    [ObservableProperty]
    private ObservableCollection<AppUser> _users = new();

    [ObservableProperty]
    private ObservableCollection<AppUser> _allusers = new();

    [ObservableProperty]
    private ObservableCollection<Role> _roles = new();

    [ObservableProperty]
    private ObservableCollection<Permission> _permissions = new();

    [ObservableProperty]
    private ObservableCollection<Permission> _userRemovePermissions = new();

    [ObservableProperty]
    private ObservableCollection<Permission> _userAddPermissions = new();

    [ObservableProperty]
    private ObservableCollection<object> _selectedUserRemovePermissions = new();

    [ObservableProperty]
    private ObservableCollection<object> _selectedUserAddPermissions = new();


    [ObservableProperty]
    private AppUser _editingUserData;

    [ObservableProperty]
    private Concept _editingConceptData;

    [ObservableProperty]
    private TaskBoard _editingTaskBoardData;

    public ProjectManagementPageModel(
        IConceptsService conceptsService,
        IConceptBoardsService conceptBoardsService,
        ITaskBoardsService taskBoardsService,
        ITaskSectionsService taskSectionsService,
        ITaskProgressService taskProgressService,
        IUserProjectPermissionsService userProjectPermissionsService,
        IPermissionsService permissionsService,
        IProjectsService projectsService,
        IProjectUsersService projectUsersService,
        IUsersService usersService,
        IRolesService rolesService, 
        UserSession userSession,
        PermissionsUtils permissionsUtils)
    {
        this.conceptsService = conceptsService;
        this.conceptBoardsService = conceptBoardsService;
        this.taskBoardsService = taskBoardsService;
        this.taskSectionsService = taskSectionsService;
        this.taskProgressService = taskProgressService;
        this.userProjectPermissionsService = userProjectPermissionsService;
        this.permissionsService = permissionsService;
        this.projectsService = projectsService;
        this.projectUsersService = projectUsersService;
        this.usersService = usersService;
        this.rolesService = rolesService;
        this.userSession = userSession;
        this.permissionsUtils = permissionsUtils;
    }

    /*When a user navigates to the page, resets the relevant properties and begins loading the data*/
    public async Task OnNavigatedTo()
    {
        IsLoadingConcepts = true;
        IsLoadingTaskBoards = true;
        IsLoadingUsers = true;

        IsEditingUser = false;
        IsEditingConcept = false;
        IsEditingTaskBoard = false;
        EditingUserData = null;
        EditingConceptData = null;
        EditingTaskBoardData = null;
        SelectedConcept = null;
        SelectedTaskBoard = null;
        SelectedUser = null;

        await LoadData();
        NavigationContext.CurrentProject = null;
        IsLoadingConcepts = false;
        IsLoadingTaskBoards = false;
        IsLoadingUsers = false;
    }

    /*Loads the relevant data*/
    private async Task LoadData()
    {
        if (_isLoadingData)
        {
            return;
        }
        _isLoadingData = true;
        if (NavigationContext.CurrentProject == null)
        {
            await Shell.Current.GoToAsync("//MainPage");
            return;
        }
        else
        {
            Project = NavigationContext.CurrentProject;
        }
        
        CurrentProject = Project;

        var concepts = await conceptsService.GetAllConceptsByProject(Project.Id);
        var taskBoards = await taskBoardsService.GetAllTaskBoardsByProject(Project.Id);
        var users = await usersService.GetUsersByProject(Project.Id);
        var roles = await rolesService.GetAll();
        var permissions = await permissionsService.GetAll();

        Concepts.Clear();
        foreach (var item in concepts)
        {
            Concepts.Add(item);
        }

        TaskBoards.Clear();
        foreach (var item in taskBoards)
        {
            TaskBoards.Add(item);
        }

        Users.Clear();
        foreach (var item in users)
        {
            var userRole = await projectUsersService.GetProjectUserByUserAndProject(item.Id, Project.Id);
            if (userRole.IdRole != null)
            {
                item.Role = await rolesService.GetById((int)userRole.IdRole);
            }
            Users.Add(item);
        }

        Roles.Clear();
        foreach (var item in roles)
        {
            Roles.Add(item);
        }

        Permissions.Clear();
        foreach (var item in permissions)
        {
            Permissions.Add(item);
        }

        CheckPermissions();

        _isLoadingData = false;
    }

    /*Updates the sections a user can see based on their permissions*/
    private void CheckPermissions()
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.ReadConcepts);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            CanSeeConcepts = true;
        }
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.ReadTasksBoards);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            CanSeeTaskBoards = true;
        }
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.ReadUsers);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            CanSeeUsers = true;
        }
    }

    /*Navigates to the task board page*/
    [RelayCommand]
    private async void TaskBoardSelected(TaskBoard taskBoard)
    {
        NavigationContext.CurrentProject = Project;
        NavigationContext.CurrentTaskBoard = SelectedTaskBoard;
        await Shell.Current.GoToAsync("TaskBoardPage", new Dictionary<string, object>
        {
             {"TaskBoard", SelectedTaskBoard }
        });
    }

    /*Navigates to the first concept board of a concept*/
    [RelayCommand]
    private async void ConceptSelected(Concept concept)
    {
        NavigationContext.CurrentConcept = SelectedConcept;
        if (SelectedConcept != null)
        {
            var conceptBoard = await conceptBoardsService.GetById(SelectedConcept.IdFirstBoard);
            NavigationContext.CurrentProject = Project;
            NavigationContext.CurrentConceptBoards.Push(conceptBoard);
            if (conceptBoard != null)
            {
                await Shell.Current.GoToAsync("ConceptBoardPage");
            }
        }
    }

    /*Opens the User editing menu*/
    [RelayCommand]
    private async void UserSelected(AppUser user)
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.EditUsers, PermissionsUtils.Permissions.FullUserPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            IsEditingConcept = false;
            IsEditingTaskBoard = false;
            EditingConceptData = null;
            EditingTaskBoardData = null;

            if (SelectedUser != null)
            {
                var matchingRole = Roles.FirstOrDefault(r => r.Id == SelectedUser.Role?.Id);
                if (matchingRole != null)
                {
                    EditingUserData = new AppUser
                    {
                        Id = SelectedUser.Id,
                        Username = SelectedUser.Username,
                        Email = SelectedUser.Email,
                        Role = matchingRole,
                    };
                }
                else
                {
                    EditingUserData = new AppUser
                    {
                        Id = SelectedUser.Id,
                        Username = SelectedUser.Username,
                        Email = SelectedUser.Email,
                        Role = null
                    };
                }

                var userPermissions = await userProjectPermissionsService.GetAllUserProjectPermissionsByUserAndProject(SelectedUser.Id, Project.Id);

                EditingUserData.ProjectPermissions = new(userPermissions);
                UserRemovePermissions.Clear();
                UserAddPermissions.Clear();

                foreach (var permission in Permissions)
                {
                    if (userPermissions.Any(up => up.IdPermission == permission.Id))
                    {
                        UserRemovePermissions.Add(permission);
                    }
                    else
                    {
                        UserAddPermissions.Add(permission);
                    }
                }

                IsEditingUser = true;
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.NoPermissionMessage, Resources.ConfirmButton);
        }
    }

    /*Creates a task board*/
    [RelayCommand]
    private async void CreateTaskBoard()
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.CreateTaskBoards, PermissionsUtils.Permissions.FullTaskPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            IsLoadingTaskBoards = true;
            NavigationContext.CurrentProject = Project;
            var taskBoardform = await FormDialog.ShowCreateObjectMenuAsync<TaskBoardFormCreate>(Resources.CreateTaskBoardTitle);
            if (taskBoardform != null && !string.IsNullOrEmpty(taskBoardform.Title))
            {
                if (string.IsNullOrEmpty(taskBoardform.Description))
                {
                    taskBoardform.Description = string.Empty;
                }

                var taskBoard = new TaskBoardCreate
                {
                    IdProject = Project.Id,
                    Title = taskBoardform.Title,
                    Description = taskBoardform.Description
                };

                var returnTaskBoard = await taskBoardsService.Post(taskBoard);
                var taskSection = await taskSectionsService.Post(new TaskSectionCreate
                {
                    IdBoard = returnTaskBoard.Id,
                    Title = Resources.DefaultSectionTitle,
                    Order = 1
                });
                var taskProgress = await taskProgressService.Post(new TaskProgressCreate
                {
                    IdSection = taskSection.Id,
                    Title = Resources.DefaultProgressTitle,
                    Order = 1,
                    ModifiesProgress = false,
                    ProgressValue = 0
                });
                await taskSectionsService.Patch(taskSection.Id, new TaskSectionUpdate
                {
                    IdBoard = taskSection.IdBoard,
                    IdDefaultProgress = taskProgress.Id,
                    Title = taskSection.Title,
                    Order = taskSection.Order
                });

                NavigationContext.CurrentProject = Project;
                await LoadData();
            }
            else
            {
                if (string.IsNullOrEmpty(taskBoardform.Title))
                {
                    await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.TitleIsRequiredMessage, Resources.ConfirmButton);
                }
            }
            IsLoadingTaskBoards = false;
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.NoPermissionMessage, Resources.ConfirmButton);
        }
    }

    /*Creates a concept*/
    [RelayCommand]
    private async void CreateConcept()
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.CreateConcepts, PermissionsUtils.Permissions.FullConceptPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            IsLoadingConcepts = true;
            NavigationContext.CurrentProject = Project;
            var conceptForm = await FormDialog.ShowCreateObjectMenuAsync<ConceptFormCreate>(Resources.CreateConceptTitle);

            if (conceptForm != null && !string.IsNullOrEmpty(conceptForm.Title))
            {
                if (string.IsNullOrEmpty(conceptForm.Description))
                {
                    conceptForm.Description = string.Empty;
                }

                var concept = new ConceptCreate
                {
                    IdProject = Project.Id,
                    Title = conceptForm.Title,
                    Description = conceptForm.Description
                };

                var returnConcept = await conceptsService.Post(concept);
                var conceptBoard = await conceptBoardsService.Post(new ConceptBoardCreate
                {
                    IdConcept = returnConcept.Id,
                    Name = Resources.DefaultConceptBoardName
                });
                conceptBoard.IdParent = conceptBoard.Id;
                var conceptBoardUpdate = new ConceptBoardUpdate
                {
                    IdConcept = conceptBoard.IdConcept,
                    IdParent = conceptBoard.IdParent,
                    Name = conceptBoard.Name,
                };
                var conceptBoardReturn = await conceptBoardsService.Patch(conceptBoard.Id, conceptBoardUpdate);
                returnConcept.IdFirstBoard = conceptBoard.Id;

                var conceptUpdate = new ConceptUpdate
                {
                    IdProject = returnConcept.IdProject,
                    IdFirstBoard = returnConcept.IdFirstBoard,
                    Title = returnConcept.Title,
                    Description = returnConcept.Description
                };
                await conceptsService.Patch(returnConcept.Id, conceptUpdate);

                if (conceptBoardReturn == "Concept board updated")
                {
                    var concepts = Concepts.ToList();
                    NavigationContext.CurrentProject = Project;
                    await LoadData();
                }
            }
            else
            {
                if (string.IsNullOrEmpty(conceptForm.Title))
                {
                    await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.TitleIsRequiredMessage, Resources.ConfirmButton);
                }
            }
            IsLoadingConcepts = false;
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.NoPermissionMessage, Resources.ConfirmButton);
        }
    }

    /*Allows the user to input an email to look for a user when they want to add them to the project*/
    [RelayCommand]
    private async void LookForUser()
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.AddUsers, PermissionsUtils.Permissions.FullUserPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            IsLookingForUser = true;
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.NoPermissionMessage, Resources.ConfirmButton);
        }
    }

    /*Looks for a user with a specific email, if found, adds it to the project*/
    [RelayCommand]
    private async void AddUserToProject()
    {
        IsLoadingUsers = true;
        var userFound = false;

        if (!string.IsNullOrEmpty(AdduserEmail))
        {
            if (Users.Any(user => user.Email == AdduserEmail))
            {
                await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.UserAlreadyInProjectMessage, Resources.ConfirmButton);
                return;
            }
            else
            {
                var emailUser = await usersService.GetUserByEmail(AdduserEmail);
                if (emailUser != null)
                {
                    var projectUserCreate = new ProjectUserCreate
                    {
                        IdProject = Project.Id,
                        IdUser = emailUser.Id,
                        IdRole = 2
                    };
                    projectUsersService.Post(projectUserCreate);
                    userFound = true;
                }

                if (userFound)
                {
                    await Application.Current.MainPage.DisplayAlert(Resources.SuccessMessageTitle, Resources.UserAddedToProjectMessage, Resources.ConfirmButton);
                    IsLookingForUser = false;

                    IsLoadingUsers = true;
                    NavigationContext.CurrentProject = Project;
                    await LoadData();
                    IsLoadingUsers = false;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.UserNotFoundMessage, Resources.ConfirmButton);
                }
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.EmailInputMessage, Resources.ConfirmButton);
        }
        IsLoadingUsers = false;
    }

    /*Deletes a task board*/
    [RelayCommand]
    private async void TaskBoardDelete(TaskBoard taskBoard)
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.DeleteTasksBoards, PermissionsUtils.Permissions.FullTaskPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            IsLoadingTaskBoards = true;
            bool confirmed = await Application.Current.MainPage.DisplayAlert(
                Resources.ConfirmDeleteMessageTitle,
                Resources.DeletionConfirmationMessage,
                Resources.DeleteButton,
                Resources.CancelButton
            );

            if (confirmed)
            {
                await taskBoardsService.Delete(taskBoard.Id);
                NavigationContext.CurrentProject = Project;
                await LoadData();
            }
            IsLoadingTaskBoards = false;
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.NoPermissionMessage, Resources.ConfirmButton);
        }
    }

    /*Deletes a concept*/
    [RelayCommand]
    private async void ConceptDelete(Concept concept)
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.DeleteConcepts, PermissionsUtils.Permissions.FullConceptPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            IsLoadingConcepts = true;
            bool confirmed = await Application.Current.MainPage.DisplayAlert(
                Resources.ConfirmDeleteMessageTitle,
                Resources.DeletionConfirmationMessage,
                Resources.DeleteButton,
                Resources.CancelButton
            );

            if (confirmed)
            {
                await conceptsService.Delete(concept.Id);
                NavigationContext.CurrentProject = Project;
                await LoadData();
            }
            IsLoadingConcepts = false;
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.NoPermissionMessage, Resources.ConfirmButton);
        }
    }

    /*Removes a user from the project*/
    [RelayCommand]
    private async void UserRemove(AppUser user)
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.RemoveUsers, PermissionsUtils.Permissions.FullUserPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            IsLoadingUsers = true;

            bool confirmed = await Application.Current.MainPage.DisplayAlert(
                Resources.ConfirmRemoveUserMessage,
                Resources.ConfirmRemoveUserMessage,
                Resources.RemoveButton,
                Resources.CancelButton
            );

            if (confirmed)
            {
                var projectUser = await projectUsersService.GetProjectUserByUserAndProject(user.Id, Project.Id);
                if (projectUser != null)
                {
                    await projectUsersService.Delete(projectUser.Id);
                }
                NavigationContext.CurrentProject = Project;
                await LoadData();
            }

            IsLoadingUsers = false;
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.NoPermissionMessage, Resources.ConfirmButton);
        }
    }

    /*Opens the editing menu for task boards*/
    [RelayCommand]
    private async void TaskBoardEdit(TaskBoard taskBoard)
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.EditTaskBoards, PermissionsUtils.Permissions.FullTaskPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            IsEditingTaskBoard = true;

            IsEditingConcept = false;
            IsEditingUser = false;
            EditingConceptData = null;
            EditingUserData = null;
            SelectedUser = null;

            EditingTaskBoardData = new TaskBoard
            {
                Id = taskBoard.Id,
                Title = taskBoard.Title,
                Description = taskBoard.Description
            };
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.NoPermissionMessage, Resources.ConfirmButton);
        }
    }

    /*Opens the editing menu for concepts*/
    [RelayCommand]
    private async void ConceptEdit(Concept concept)
    {
        List<PermissionsUtils.Permissions> permissions = new List<PermissionsUtils.Permissions>();
        permissions.AddRange(PermissionsUtils.Permissions.FullPermissions, PermissionsUtils.Permissions.EditConcepts, PermissionsUtils.Permissions.FullConceptPermissions);
        if (permissionsUtils.HasOnePermission(permissions))
        {
            IsEditingConcept = true;

            IsEditingTaskBoard = false;
            IsEditingUser = false;
            EditingTaskBoardData = null;
            EditingUserData = null;
            SelectedUser = null;

            EditingConceptData = new Concept
            {
                Id = concept.Id,
                IdProject = concept.IdProject,
                IdFirstBoard = concept.IdFirstBoard,
                Title = concept.Title,
                Description = concept.Description
            };
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.NoPermissionMessage, Resources.ConfirmButton);
        }
    }

    /*Closes the editing menu for task boards*/
    [RelayCommand]
    private async void CloseEditingTaskBoard()
    {
        EditingUserData = null;
        EditingConceptData = null;
        EditingTaskBoardData = null;
        IsEditingUser = false;
        IsEditingTaskBoard = false;
        IsEditingConcept = false;
    }

    /*Closes the editing menu for concepts*/
    [RelayCommand]
    private async void CloseEditingConcept()
    {
        EditingUserData = null;
        EditingConceptData = null;
        EditingTaskBoardData = null;
        IsEditingUser = false;
        IsEditingTaskBoard = false;
        IsEditingConcept = false;
    }

    /*Closes the editing menu for users*/
    [RelayCommand]
    private async void CloseEditingUser()
    {
        EditingUserData = null;
        EditingConceptData = null;
        EditingTaskBoardData = null;
        SelectedUser = null;
        UserRemovePermissions.Clear();
        UserAddPermissions.Clear();
        SelectedUserAddPermissions.Clear();
        SelectedUserRemovePermissions.Clear();

        IsEditingUser = false;
        IsEditingTaskBoard = false;
        IsEditingConcept = false;
    }

    /*Saves the changes made to the task board*/
    [RelayCommand]
    private async void SaveTaskBoardChanges()
    {
        if (EditingTaskBoardData != null)
        {
            var taskBoardUpdate = new TaskBoardUpdate
            {
                IdProject = Project.Id,
                Title = EditingTaskBoardData.Title,
                Description = EditingTaskBoardData.Description
            };
            IsEditingTaskBoard = true;
            await taskBoardsService.Patch(EditingTaskBoardData.Id, taskBoardUpdate);
            CloseEditingTaskBoard();
            NavigationContext.CurrentProject = Project;
            await LoadData();
        }
        IsEditingTaskBoard = false;
    }

    /*Saves the changes made to the concept*/
    [RelayCommand]
    private async void SaveConceptChanges()
    {
        if (EditingConceptData != null)
        {
            var conceptUpdate = new ConceptUpdate
            {
                IdProject = Project.Id,
                IdFirstBoard = EditingConceptData.IdFirstBoard,
                Title = EditingConceptData.Title,
                Description = EditingConceptData.Description
            };

            IsEditingConcept = true;
            await conceptsService.Patch(EditingConceptData.Id, conceptUpdate);
            CloseEditingConcept();
            NavigationContext.CurrentProject = Project;
            await LoadData();
        }
        IsEditingConcept = false;
    }

    /*Saves the changes made to the user*/
    [RelayCommand]
    private async void SaveUserChanges()
    {
        if (EditingUserData != null)
        {
            var updateRole = new ProjectUserUpdate
            {
                IdProject = Project.Id,
                IdUser = EditingUserData.Id,
                IdRole = EditingUserData.Role?.Id
            };
            IsLoadingUsers = true;

            var projectUser = await projectUsersService.GetProjectUserByUserAndProject(EditingUserData.Id, Project.Id);

            await projectUsersService.Patch(projectUser.Id, updateRole);

            var userPermissions = await userProjectPermissionsService.GetAllUserProjectPermissionsByUserAndProject(EditingUserData.Id, Project.Id);

            foreach (var p in SelectedUserRemovePermissions)
            {
                if (p is Permission permission)
                {
                    if (userPermissions.Any(up => up.IdPermission == permission.Id))
                    {
                        await userProjectPermissionsService.Delete(userPermissions.First(up => up.IdPermission == permission.Id).Id);
                    }
                }
            }

            foreach (var p in SelectedUserAddPermissions)
            {
                if (p is Permission permission)
                {
                    if (!userPermissions.Any(up => up.IdPermission == permission.Id))
                    {
                        await userProjectPermissionsService.Post(new UserProjectPermissionCreate
                        {
                            IdProject = Project.Id,
                            IdUser = EditingUserData.Id,
                            IdPermission = permission.Id
                        });
                    }
                }

            }

            CloseEditingUser();
            NavigationContext.CurrentProject = Project;
            await LoadData();
            IsLoadingUsers = false;
        }
    }
}