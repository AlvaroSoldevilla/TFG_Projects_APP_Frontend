using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Services.ProjectsService;
using TFG_Projects_APP_Frontend.Services.UsersService;

namespace TFG_Projects_APP_Frontend.PageModels;

public partial class MainPageModel : ObservableObject
{
    private readonly IProjectsService projectsService;
    private readonly UserSession userSession;

    [ObservableProperty]
    private bool _isloading;

    [ObservableProperty]
    ObservableCollection<Project> _projects;
    [ObservableProperty]
    Project _selectedProject;

    public MainPageModel(IProjectsService projectsService, UserSession userSession)
    {
        this.projectsService = projectsService;
        this.userSession = userSession;
        LoadData();
    }

    private async Task LoadData()
    {
        Isloading = true;
        Projects = new(await projectsService.GetAllProjectsByUser(userSession.User.Id));
        Isloading = false;
    }

    [RelayCommand]
    private async void ProjectSelected(Project project)
    {
        await Shell.Current.GoToAsync("ProjectmanagementPage", new Dictionary<string, object>
        {
             {"Project", SelectedProject }
        });
    }
}