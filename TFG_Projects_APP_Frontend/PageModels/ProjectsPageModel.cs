using CommunityToolkit.Mvvm.ComponentModel;
using TFG_Projects_APP_Frontend.Services.ProjectsService;

namespace TFG_Projects_APP_Frontend.PageModels;

public class ProjectsPageModel(ProjectsService projectsService) : ObservableObject
{
    private readonly ProjectsService projectsService = projectsService;
}
