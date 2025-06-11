using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.ProjectsService;

/*Inherits IService and adds model specific methods*/
public interface IProjectsService : IService<Project>
{
    /*gets every project a user is in*/
    Task<List<Project>> GetAllProjectsByUser(int id);
}
