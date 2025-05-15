using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.TasksService;

public interface ITasksService : IService<ProjectTask>
{
    Task<List<ProjectTask>> GetAllTasksByTaskSection(int id);
    Task<List<ProjectTask>> GetAllTasksByTaskProgress(int id);
    Task<AppUser> getUserCreated(int id);
    Task<AppUser> getUserAssigned(int id);
}
