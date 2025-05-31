using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Entities.Dtos.Permissions;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.PermissionsService;

public class PermissionsService(RestClient restClient) : IPermissionsService
{
    private readonly string route = "permissions";
    public async Task<string> Delete(int id)
    {
        HttpResponseMessage response = await restClient.DeleteAsync(route, id);
        if (response == null)
        {
            return null;
        }
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<List<Permission>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        if (response == null)
        {
            return null;
        }
        var permissions = await response.Content.ReadFromJsonAsync<List<PermissionRead>>(restClient._options);
        return new List<Permission>(permissions.Select(permission =>
        {
            return new Permission
            {
                Id = permission.Id,
                Name = permission.Name,
            };
        }).ToList());
    }

    public async Task<Permission> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        if (response == null)
        {
            return null;
        }
        var permission = await response.Content.ReadFromJsonAsync<PermissionRead>(restClient._options);
        return new Permission
        {
            Id = permission.Id,
            Name = permission.Name,
        };
    }

    public async Task<string> Patch(int id, object data)
    {
        HttpResponseMessage response = await restClient.PatchAsync(route, id, data);
        if (response == null)
        {
            return null;
        }
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<Permission> Post(object data)
    {
        HttpResponseMessage response = await restClient.PostAsync(route, data);
        if (response == null)
        {
            return null;
        }
        var permission = await response.Content.ReadFromJsonAsync<PermissionRead>(restClient._options);
        return new Permission
        {
            Id = permission.Id,
            Name = permission.Name,
        };
    }
}
