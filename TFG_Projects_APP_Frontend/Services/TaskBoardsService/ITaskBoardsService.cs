using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.TaskBoardsService;

/*Inherits IService and adds model specific methods*/
public interface ITaskBoardsService : IService<TaskBoard>
{
    /*Get every task board from a project*/
    Task<List<TaskBoard>> GetAllTaskBoardsByProject(int id);
}
