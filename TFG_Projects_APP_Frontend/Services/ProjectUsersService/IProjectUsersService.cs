using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.ProjectUsersService;

public interface IProjectUsersService : IService<ProjectUser>
{
    Task<ObservableCollection<ProjectUser>> GetAllProjectUsersByProject(int id);
    Task<ObservableCollection<ProjectUser>> GetAllProjectUsersByUser(int id);
}
