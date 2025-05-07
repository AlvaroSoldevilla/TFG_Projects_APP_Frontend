using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.TasksService;

internal interface ITasksService : IService<ProjectTask>
{
    Task<ObservableCollection<ProjectTask>> GetAllTasksByTaskSection(int id);
    Task<ObservableCollection<ProjectTask>> GetAllTasksByTaskProgress(int id);
    Task<AppUser> getUserCreated(int id);
    Task<AppUser> getUserAssigned(int id);
}
