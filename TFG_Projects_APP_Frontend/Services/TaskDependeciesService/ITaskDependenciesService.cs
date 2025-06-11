using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.TaskDependeciesService;

/*Inherits IService and adds model specific methods*/
public interface ITaskDependenciesService : IService<TaskDependency>
{
    /*Get every dependency from a task*/
    Task<List<TaskDependency>> GetAllTaskDependenciesByTask(int id);
}
