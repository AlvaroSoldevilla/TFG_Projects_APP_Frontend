using System.Collections.ObjectModel;
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
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<ObservableCollection<Role>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        var roles = await response.Content.ReadFromJsonAsync<ObservableCollection<RoleRead>>(restClient._options);
        return new ObservableCollection<Role>(roles.Select(role =>
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
