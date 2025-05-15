using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.ProjectsService;

public interface IProjectsService : IService<Project>
{
    Task<List<Project>> GetAllProjectsByUser(int id);
}
