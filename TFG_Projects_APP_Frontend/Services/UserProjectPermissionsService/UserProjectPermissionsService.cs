using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.UserProjectPermissionsService
{
    internal class UserProjectPermissionsService(RestClient restClient) : IUserProjectPermissionsService
    {
        public Task<string> Delete(string query, int id)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<UserProjectPermission>> GetAll(string query)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<UserProjectPermission>> getAllUserProjectPermissionsByPermission(string query, int id)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<UserProjectPermission>> getAllUserProjectPermissionsByProject(string query, int id)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<UserProjectPermission>> getAllUserProjectPermissionsByUser(string query, int id)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<UserProjectPermission>> getAllUserProjectPermissionsByUserAndProject(string query, int userId, int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<UserProjectPermission> GetById(string query, int id)
        {
            throw new NotImplementedException();
        }

        public Task<string> Patch(string query, object data)
        {
            throw new NotImplementedException();
        }

        public Task<string> Post(string query, object data)
        {
            throw new NotImplementedException();
        }
    }
}
