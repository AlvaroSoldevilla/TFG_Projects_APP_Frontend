using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.TaskProgressService;

public interface ITaskProgressService : IService<TaskProgress>
{
    Task<List<TaskProgress>> getAlltaskProgressByTaskSection(int idSection);
}
