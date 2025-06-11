using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.TasksService;

/*Inherits IService and adds model specific methods*/
public interface ITasksService : IService<ProjectTask>
{
    /*Get all tasks in a task section*/
    Task<List<ProjectTask>> GetAllTasksByTaskSection(int id);
    /*Get all tasks in a progress section*/
    Task<List<ProjectTask>> GetAllTasksByTaskProgress(int id);
    /*Get all child tasks from a parent*/
    Task<List<ProjectTask>> GetAllTasksByParent(int id);
    /*Get the user that created the task*/
    Task<AppUser> GetUserCreated(int id);
    /*Get the user assigned to a task*/
    Task<AppUser> GetUserAssigned(int id);
}
