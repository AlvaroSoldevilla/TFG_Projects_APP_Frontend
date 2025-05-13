using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Services.ProjectsService;

namespace TFG_Projects_APP_Frontend.PageModels;

public partial class MainPageModel : ObservableObject
{
    private readonly IProjectsService projectsService;

    [ObservableProperty]
    ObservableCollection<Project> _projects;
    [ObservableProperty]
    Project _selectedProject;

    public MainPageModel(IProjectsService projectsService)
    {
        this.projectsService = projectsService;
        LoadData();
    }

    private async Task LoadData()
    {
        Projects = await projectsService.GetAllProjectsByUser(1);
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