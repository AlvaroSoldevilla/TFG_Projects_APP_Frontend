using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.TasksService;

internal interface ITasksService : IService<ProjectTask>
{
    Task<ObservableCollection<ProjectTask>> GetAllTasksByTaskSection(string query, int id);
    Task<ObservableCollection<ProjectTask>> GetAllTasksByTaskProgress(string query, int id);
    Task<AppUser> getUserCreated(string query, int id);
    Task<AppUser> getUserAssigned(string query, int id);
}
