using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.TaskProgressService;

/*Inherits IService and adds model specific methods*/
public interface ITaskProgressService : IService<TaskProgress>
{
    /*Get all progress sections from a section*/
    Task<List<TaskProgress>> GetAlltaskProgressByTaskSection(int idSection);
}
