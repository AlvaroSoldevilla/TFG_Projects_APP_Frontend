using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Services.ProjectsService;

namespace TFG_Projects_APP_Frontend.PageModels;

public partial class MainPageModel : ObservableObject
{
    private readonly ProjectsService projectsService;

    [ObservableProperty]
    ObservableCollection<Project> _projects;
    public MainPageModel(ProjectsService projectsService)
    {
        this.projectsService = projectsService;
        LoadData();
    }

    private async Task LoadData()
    {
        Projects = await projectsService.GetAllProjectsByUser(1);
    }
}
