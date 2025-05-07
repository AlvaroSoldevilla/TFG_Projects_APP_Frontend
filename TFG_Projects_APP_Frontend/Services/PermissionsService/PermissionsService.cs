using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.PermissionsService;

internal class PermissionsService(RestClient restClient) : IPermissionsService
{
    public async Task<string> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ObservableCollection<Permission>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<Permission> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<string> Patch(int id, object data)
    {
        throw new NotImplementedException();
    }

    public async Task<string> Post(object data)
    {
        throw new NotImplementedException();
    }
}
