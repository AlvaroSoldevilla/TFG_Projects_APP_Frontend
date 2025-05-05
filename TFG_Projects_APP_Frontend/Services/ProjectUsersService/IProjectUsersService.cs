using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.ProjectUsersService;

internal interface IProjectUsersService : IService<ProjectUser>
{
    Task<ObservableCollection<ProjectUser>> GetAllProjectUsersByProject(string query, int id);
    Task<ObservableCollection<ProjectUser>> GetAllProjectUsersByUser(string query, int id);
}
