using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.UserProjectPermissionsService;

public interface IUserProjectPermissionsService : IService<UserProjectPermission>
{
    Task<ObservableCollection<UserProjectPermission>> getAllUserProjectPermissionsByUser(int id);
    Task<ObservableCollection<UserProjectPermission>> getAllUserProjectPermissionsByProject(int id);
    Task<ObservableCollection<UserProjectPermission>> getAllUserProjectPermissionsByPermission(int id);
    Task<ObservableCollection<UserProjectPermission>> getAllUserProjectPermissionsByUserAndProject(int userId, int projectId);
}
