using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Dtos.Roles;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.RolesService;

public class RolesService(RestClient restClient) : IRolesService
{
    private readonly string route = "roles";
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

    public async Task<List<Role>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        if (response == null)
        {
            return null;
        }
        var roles = await response.Content.ReadFromJsonAsync<List<RoleRead>>(restClient._options);
        return new List<Role>(roles.Select(role =>
        {
            return new Role
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };
        }).ToList());
    }

    public async Task<Role> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        if (response == null)
        {
            return null;
        }
        var role = await response.Content.ReadFromJsonAsync<RoleRead>(restClient._options);
        return new Role
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description
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

    public async Task<Role> Post(object data)
    {
        HttpResponseMessage response = await restClient.PostAsync(route, data);
        if (response == null)
        {
            return null;
        }
        var role = await response.Content.ReadFromJsonAsync<RoleRead>(restClient._options);
        return new Role
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description
        };
    }
}
