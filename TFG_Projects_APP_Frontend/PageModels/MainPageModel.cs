using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Components.CreateModal;
using TFG_Projects_APP_Frontend.Entities.Dtos.Projects;
using TFG_Projects_APP_Frontend.Entities.Dtos.ProjectUsers;
using TFG_Projects_APP_Frontend.Entities.Dtos.UserProjectPermissions;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Services;
using TFG_Projects_APP_Frontend.Services.ProjectsService;
using TFG_Projects_APP_Frontend.Services.ProjectUsersService;
using TFG_Projects_APP_Frontend.Services.UserProjectPermissionsService;
using TFG_Projects_APP_Frontend.Services.UsersService;

namespace TFG_Projects_APP_Frontend.PageModels;

public partial class MainPageModel : ObservableObject
{
    private readonly IProjectsService projectsService;
    private readonly IUserProjectPermissionsService userProjectPermissionsService;
    private readonly IProjectUsersService projectUsersService;
    private readonly UserSession userSession;

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
        UserSession userSession)
    {
        this.projectsService = projectsService;
        this.userProjectPermissionsService = userProjectPermissionsService;
        this.projectUsersService = projectUsersService;
        this.userSession = userSession;
    }

    public async Task OnNavigatedTo()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        Isloading = true;
        Projects = new(await projectsService.GetAllProjectsByUser(userSession.User.Id));
        SelectedProject = null;
        Isloading = false;
    }

    [RelayCommand]
    private async void ProjectSelected(Project project)
    {
        NavigationContext.CurrentProject = SelectedProject;
        userSession.User.ProjectPermissions = await userProjectPermissionsService.getAllUserProjectPermissionsByUserAndProject(userSession.User.Id, SelectedProject.Id);
        await Shell.Current.GoToAsync("ProjectmanagementPage", new Dictionary<string, object>
        {
             {"Project", SelectedProject }
        });
    }

    [RelayCommand]
    private async void CreateProject()
    {
        Isloading = true;
        var project = await FormDialog.ShowCreateObjectMenuAsync<ProjectCreate>("Create Project");
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
            Projects.Add(returnProject);
        } else
        {
            if (string.IsNullOrEmpty(project.Title))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Title is required", "OK");
            }
        }
        Isloading = false;
    }

    [RelayCommand]
    private async void ProjectDelete(Project project)
    {
        userSession.User.ProjectPermissions = await userProjectPermissionsService.getAllUserProjectPermissionsByUserAndProject(userSession.User.Id, project.Id);
        if (userSession.User.ProjectPermissions != null && userSession.User.ProjectPermissions.Find(x=> x.IdPermission == 1) != null)
        {
            bool confirmed = await Application.Current.MainPage.DisplayAlert(
                "Confirm Delete",
                "Are you sure you want to delete this item?",
                "Delete",
                "Cancel"
            );

            if (confirmed)
            {
                await projectsService.Delete(project.Id);
                Projects.Remove(project);
            }
        } else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "You don't have permission to do that", "OK");
        }
        
    }

    [RelayCommand]
    private async void ProjectEdit(Project project)
    {
        IsEditingProject = true;
        EditingProjectData = new Project
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description
        };
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
            await Application.Current.MainPage.DisplayAlert("Error", "Title is required", "OK");
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