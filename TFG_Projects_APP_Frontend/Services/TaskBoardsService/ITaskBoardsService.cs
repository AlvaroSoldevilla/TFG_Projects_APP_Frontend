using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.TaskBoardsService;

public interface ITaskBoardsService : IService<TaskBoard>
{
    Task<List<TaskBoard>> GetAllTaskBoardsByProject(int id);
}
