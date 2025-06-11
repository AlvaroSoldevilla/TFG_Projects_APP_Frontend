using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.TaskSectionsService;

/*Inherits IService and adds model specific methods*/
public interface ITaskSectionsService : IService<TaskSection>
{
    /*Get all task sections from a board*/
    Task<List<TaskSection>> GetAllTaskSectionsByTaskBoard(int id);
}
