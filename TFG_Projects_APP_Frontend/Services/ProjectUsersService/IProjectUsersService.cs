using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.ProjectUsersService;

/*Inherits IService and adds model specific methods*/
public interface IProjectUsersService : IService<ProjectUser>
{
    /*Get every projectUser from a project*/
    Task<List<ProjectUser>> GetAllProjectUsersByProject(int id);
    /*Get every projectUser from a user*/
    Task<List<ProjectUser>> GetAllProjectUsersByUser(int id);
    /*Get every projectUser from a project and user (Used to get the role of the user in the project)*/
    Task<ProjectUser> GetProjectUserByUserAndProject(int userId, int projectId);
}
