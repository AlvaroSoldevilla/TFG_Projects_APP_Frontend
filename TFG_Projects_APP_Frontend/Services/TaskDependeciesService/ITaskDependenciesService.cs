using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.TaskDependeciesService;

internal interface ITaskDependenciesService : IService<TaskDependency>
{
    Task<ObservableCollection<TaskDependency>> GetAllTaskDependenciesByTask(int id);
}
