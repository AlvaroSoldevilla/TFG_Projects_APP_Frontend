using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.UserProjectPermissionsService;

public interface IUserProjectPermissionsService : IService<UserProjectPermission>
{
    Task<List<UserProjectPermission>> getAllUserProjectPermissionsByUser(int id);
    Task<List<UserProjectPermission>> getAllUserProjectPermissionsByProject(int id);
    Task<List<UserProjectPermission>> getAllUserProjectPermissionsByPermission(int id);
    Task<List<UserProjectPermission>> getAllUserProjectPermissionsByUserAndProject(int userId, int projectId);
}
