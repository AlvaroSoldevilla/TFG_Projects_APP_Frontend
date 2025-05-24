using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.ProjectUsersService;

public interface IProjectUsersService : IService<ProjectUser>
{
    Task<List<ProjectUser>> GetAllProjectUsersByProject(int id);
    Task<List<ProjectUser>> GetAllProjectUsersByUser(int id);
    Task<ProjectUser> GetProjectUserByUserAndProject(int userId, int projectId);
}
