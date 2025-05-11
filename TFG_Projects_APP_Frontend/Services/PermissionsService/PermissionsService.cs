using System.Collections.ObjectModel;
using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.PermissionsService;

public class PermissionsService(RestClient restClient) : IPermissionsService
{
    private readonly string route = "permissions";
    public async Task<string> Delete(int id)
    {
        HttpResponseMessage response = await restClient.DeleteAsync(route, id);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<ObservableCollection<Permission>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        var permissions = await response.Content.ReadFromJsonAsync<ObservableCollection<Permission>>(restClient._options);
        return permissions;
    }

    public async Task<Permission> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        var permission = await response.Content.ReadFromJsonAsync<Permission>(restClient._options);
        return permission;
    }

    public async Task<string> Patch(int id, object data)
    {
        HttpResponseMessage response = await restClient.PatchAsync(route, id, data);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<string> Post(object data)
    {
        HttpResponseMessage response = await restClient.PostAsync(route, data);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }
}
