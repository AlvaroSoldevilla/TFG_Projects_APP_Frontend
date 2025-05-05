using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.UserProjectPermissionsService
{
    internal interface IUserProjectPermissionsService : IService<UserProjectPermission>
    {
        Task<ObservableCollection<UserProjectPermission>> getAllUserProjectPermissionsByUser(string query, int id);
        Task<ObservableCollection<UserProjectPermission>> getAllUserProjectPermissionsByProject(string query, int id);
        Task<ObservableCollection<UserProjectPermission>> getAllUserProjectPermissionsByPermission(string query, int id);
        Task<ObservableCollection<UserProjectPermission>> getAllUserProjectPermissionsByUserAndProject(string query, int userId, int projectId);
    }
}
