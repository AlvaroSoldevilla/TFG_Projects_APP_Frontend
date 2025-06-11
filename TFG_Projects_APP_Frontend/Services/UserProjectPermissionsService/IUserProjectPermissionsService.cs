using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.UserProjectPermissionsService;

/*Inherits IService and adds model specific methods*/
public interface IUserProjectPermissionsService : IService<UserProjectPermission>
{
    /*Get all userProjectPermissions by user*/
    Task<List<UserProjectPermission>> GetAllUserProjectPermissionsByUser(int id);
    /*Get all userProjectPermissions by project*/
    Task<List<UserProjectPermission>> GetAllUserProjectPermissionsByProject(int id);
    /*Get all userProjectPermissions by permission*/
    Task<List<UserProjectPermission>> GetAllUserProjectPermissionsByPermission(int id);
    /*Get all userProjectPermissions by user and project*/
    Task<List<UserProjectPermission>> GetAllUserProjectPermissionsByUserAndProject(int userId, int projectId);
}
