using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.UserProjectPermissionsService
{
    internal class UserProjectPermissionsService(RestClient restClient) : IUserProjectPermissionsService
    {
        public Task<string> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<UserProjectPermission>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<UserProjectPermission>> getAllUserProjectPermissionsByPermission(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<UserProjectPermission>> getAllUserProjectPermissionsByProject(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<UserProjectPermission>> getAllUserProjectPermissionsByUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<UserProjectPermission>> getAllUserProjectPermissionsByUserAndProject(int userId, int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<UserProjectPermission> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<string> Patch(int id, object data)
        {
            throw new NotImplementedException();
        }

        public Task<string> Post(object data)
        {
            throw new NotImplementedException();
        }
    }
}
