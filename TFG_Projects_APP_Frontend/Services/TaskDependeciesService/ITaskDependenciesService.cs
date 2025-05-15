using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.TaskDependeciesService;

public interface ITaskDependenciesService : IService<TaskDependency>
{
    Task<List<TaskDependency>> GetAllTaskDependenciesByTask(int id);
}
