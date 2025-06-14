﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Components.CreateModal;
using TFG_Projects_APP_Frontend.Entities.Dtos.Projects;
using TFG_Projects_APP_Frontend.Entities.Dtos.ProjectUsers;
using TFG_Projects_APP_Frontend.Entities.Dtos.UserProjectPermissions;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Properties;
using TFG_Projects_APP_Frontend.Rest;
using TFG_Projects_APP_Frontend.Services.ProjectsService;
using TFG_Projects_APP_Frontend.Services.ProjectUsersService;
using TFG_Projects_APP_Frontend.Services.UserProjectPermissionsService;
using TFG_Projects_APP_Frontend.Services.UsersService;
using TFG_Projects_APP_Frontend.Utils;

namespace TFG_Projects_APP_Frontend.PageModels;

public partial class MainPageModel : ObservableObject
{
    private readonly IProjectsService projectsService;
    private readonly IUserProjectPermissionsService userProjectPermissionsService;
    private readonly IProjectUsersService projectUsersService;
    private readonly IUsersService usersService;
    private readonly UserSession userSession;
    private readonly PermissionsUtils permissionsUtils;
    private readonly RestClient restClient;

    [ObservableProperty]
    private bool _isloading;

    [ObservableProperty]
    private bool _isEditingProject;

    [ObservableProperty]
    ObservableCollection<Project> _projects;

    [ObservableProperty]
    private Project _editingProjectData;

    [ObservableProperty]
    Project _selectedProject;

    public MainPageModel(IProjectsService projectsService, 
        IUserProjectPermissionsService userProjectPermissionsService,
        IProjectUsersService projectUsersService,
        IUsersService usersService,
        UserSession userSession,
        PermissionsUtils permissionsUtils,
        RestClient restClient)
    {
        this.projectsService = projectsService;
        this.userProjectPermissionsService = userProjectPermissionsService;
        this.projectUsersService = projectUsersService;
        this.usersService = usersService;
        this.userSession = userSession;
        this.permissionsUtils = permissionsUtils;
        this.restClient = restClient;

    }

    /*When a user navigates to the page, checks if it's the startup sequence, in which case, if the user has set remember me, it updates the UserSession properties based on the Preferences, 
     * otherwise it sends the user to the Login Page
     * If it's not the startup sequence, begins loading data
     */
    public async Task OnNavigatedTo()
    {
        SelectedProject = null;
        Isloading = true;
        if (NavigationContext.Startup)
        {
            NavigationContext.Startup = false;
            var rememberMe = Preferences.Get("RememberMe", false);
            if (rememberMe)
            {
                var testUrl = Preferences.Get("APIurl", "http://localhost:8080");
                var result = await restClient.TestConnection(testUrl + "/connection/test");
                if (result.IsSuccessStatusCode)
                {
                    userSession.User = new AppUser
                    {
                        Id = Preferences.Get("UserId", 1),
                        Username = Preferences.Get("Username", "Admin"),
                        Email = Preferences.Get("Email", "admin@test.com")
                    };
                    userSession.Token = Preferences.Get("Token", "");

                    var user = await usersService.GetById(userSession.User.Id);

                    if (user == null || userSession.User.Email != user.Email || userSession.User.Username != user.Username)
                    {
                        await GoToLogin();
                    }

                    userSession.Token = Preferences.Get("Token", "");
                    await LoadData();
                }
                else
                {
                    await GoToLogin();
                }
            } else
            {
                await GoToLogin();
            }
        } else
        {
            await LoadData();
        }
    }

    /*A method to take the user to the Login page during the startup sequence*/
    private async Task GoToLogin()
    {
        Application.Current.Dispatcher.Dispatch(async () =>
        {
            await Task.Delay(50);
            await Shell.Current.GoToAsync("//LoginPage");
        });
    }

    /*Loads the relevant data*/
    private async Task LoadData()
    {
        Isloading = true;
        Projects = new(await projectsService.GetAllProjectsByUser(userSession.User.Id));
        Isloading = false;
    }

    [RelayCommand]
    private async void ProjectSelected(Project project)
    {
        if (SelectedProject != null)
        {
            NavigationContext.CurrentProject = SelectedProject;
            userSession.User.ProjectPermissions = await userProjectPermissionsService.GetAllUserProjectPermissionsByUserAndProject(userSession.User.Id, SelectedProject.Id);
            await Shell.Current.GoToAsync("ProjectmanagementPage");
        }
    }

    [RelayCommand]
    private async void CreateProject()
    {
        Isloading = true;
        var project = await FormDialog.ShowCreateObjectMenuAsync<ProjectCreate>(Resources.CreateProjectTitle);
        if (project != null && !string.IsNullOrEmpty(project.Title))
        {
            if (string.IsNullOrEmpty(project.Description))
            {
                project.Description = string.Empty;
            }
            var returnProject = await projectsService.Post(project);
            await projectUsersService.Post(new ProjectUserCreate
            {
                IdProject = returnProject.Id,
                IdUser = userSession.User.Id,
                IdRole = 1
            });
            await userProjectPermissionsService.Post(new UserProjectPermissionCreate
            {
                IdProject = returnProject.Id,
                IdUser = userSession.User.Id,
                IdPermission = 1
            });
            await LoadData();
        } else
        {
            if (string.IsNullOrEmpty(project.Title))
            {
                await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.TitleIsRequiredMessage, Resources.ConfirmButton);
            }
        }
        Isloading = false;
    }

    [RelayCommand]
    private async void ProjectDelete(Project project)
    {
        userSession.User.ProjectPermissions = await userProjectPermissionsService.GetAllUserProjectPermissionsByUserAndProject(userSession.User.Id, project.Id);
        List<PermissionsUtils.Permissions> requiredPermissions = [PermissionsUtils.Permissions.FullPermissions];
        if (userSession.User.ProjectPermissions != null && permissionsUtils.HasAllPermissions(requiredPermissions))
        {
            bool confirmed = await Application.Current.MainPage.DisplayAlert(
                Resources.ConfirmDeleteMessageTitle,
                Resources.DeletionConfirmationMessage,
                Resources.DeleteButton,
                Resources.CancelButton
            );

            if (confirmed)
            {
                await projectsService.Delete(project.Id);
                await LoadData();
            }
        } else
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.NoPermissionMessage, Resources.ConfirmButton);
        }
        
    }

    [RelayCommand]
    private async void ProjectEdit(Project project)
    {
        userSession.User.ProjectPermissions = await userProjectPermissionsService.GetAllUserProjectPermissionsByUserAndProject(userSession.User.Id, project.Id);
        List<PermissionsUtils.Permissions> requiredPermissions = [PermissionsUtils.Permissions.FullPermissions];

        if (userSession.User.ProjectPermissions != null && permissionsUtils.HasAllPermissions(requiredPermissions))
        {
            IsEditingProject = true;
            EditingProjectData = new Project
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description
            };
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.NoPermissionMessage, Resources.ConfirmButton);
        }
    }

    [RelayCommand]
    private async void CloseEditing()
    {
        IsEditingProject = false;
        EditingProjectData = null;
    }

    [RelayCommand]
    private async void SaveProject(Project project)
    {
        Isloading = true;
        if (string.IsNullOrEmpty(EditingProjectData.Title))
        {
            await Application.Current.MainPage.DisplayAlert(Resources.ErrorMessageTitle, Resources.TitleIsRequiredMessage, Resources.ConfirmButton);
            Isloading = false;
            return;
        }

        var projectUpdate = new ProjectUpdate
        {
            Title = EditingProjectData.Title,
            Description = EditingProjectData.Description
        };

        IsEditingProject = false;

        await projectsService.Patch(EditingProjectData.Id, projectUpdate);
        EditingProjectData = null;
        await LoadData();

        Isloading = false;
    }
}