using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Dtos.UserProjectPermissions;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.UserProjectPermissionsService;

/*Implementation of the UserProjectPermissionService Interface*/
public class UserProjectPermissionsService(RestClient restClient) : IUserProjectPermissionsService
{
    private readonly string route = "user_project_permissions";
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

    public async Task<List<UserProjectPermission>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        if (response == null)
        {
            return null;
        }
        var userProjectPermissions = await response.Content.ReadFromJsonAsync<List<UserProjectPermissionRead>>(restClient._options);
        return new List<UserProjectPermission>(userProjectPermissions.Select(userProjectPermission =>
        {
            return new UserProjectPermission
            {
                Id = userProjectPermission.Id,
                IdUser = userProjectPermission.IdUser,
                IdProject = userProjectPermission.IdProject,
                IdPermission = userProjectPermission.IdPermission
            };
        }).ToList());
    }

    public async Task<List<UserProjectPermission>> GetAllUserProjectPermissionsByPermission(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/permission/{id}");
        if (response == null)
        {
            return null;
        }
        var userProjectPermissions = await response.Content.ReadFromJsonAsync<List<UserProjectPermissionRead>>(restClient._options);
        return new List<UserProjectPermission>(userProjectPermissions.Select(userProjectPermission =>
        {
            return new UserProjectPermission
            {
                Id = userProjectPermission.Id,
                IdUser = userProjectPermission.IdUser,
                IdProject = userProjectPermission.IdProject,
                IdPermission = userProjectPermission.IdPermission
            };
        }).ToList());
    }

    public async Task<List<UserProjectPermission>> GetAllUserProjectPermissionsByProject(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/project/{id}");
        if (response == null)
        {
            return null;
        }
        var userProjectPermissions = await response.Content.ReadFromJsonAsync<List<UserProjectPermissionRead>>(restClient._options);
        return new List<UserProjectPermission>(userProjectPermissions.Select(userProjectPermission =>
        {
            return new UserProjectPermission
            {
                Id = userProjectPermission.Id,
                IdUser = userProjectPermission.IdUser,
                IdProject = userProjectPermission.IdProject,
                IdPermission = userProjectPermission.IdPermission
            };
        }).ToList());
    }

    public async Task<List<UserProjectPermission>> GetAllUserProjectPermissionsByUser(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/user/{id}");
        if (response == null)
        {
            return null;
        }
        var userProjectPermissions = await response.Content.ReadFromJsonAsync<List<UserProjectPermissionRead>>(restClient._options);
        return new List<UserProjectPermission>(userProjectPermissions.Select(userProjectPermission =>
        {
            return new UserProjectPermission
            {
                Id = userProjectPermission.Id,
                IdUser = userProjectPermission.IdUser,
                IdProject = userProjectPermission.IdProject,
                IdPermission = userProjectPermission.IdPermission
            };
        }).ToList());
    }

    public async Task<List<UserProjectPermission>> GetAllUserProjectPermissionsByUserAndProject(int userId, int projectId)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/user/{userId}/project/{projectId}");
        if (response == null)
        {
            return null;
        }
        var userProjectPermissions = await response.Content.ReadFromJsonAsync<List<UserProjectPermissionRead>>(restClient._options);
        return new List<UserProjectPermission>(userProjectPermissions.Select(userProjectPermission =>
        {
            return new UserProjectPermission
            {
                Id = userProjectPermission.Id,
                IdUser = userProjectPermission.IdUser,
                IdProject = userProjectPermission.IdProject,
                IdPermission = userProjectPermission.IdPermission
            };
        }).ToList());
    }

    public async Task<UserProjectPermission> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        if (response == null)
        {
            return null;
        }
        var userProjectPermission = await response.Content.ReadFromJsonAsync<UserProjectPermissionRead>(restClient._options);
        return new UserProjectPermission
        {
            Id = userProjectPermission.Id,
            IdUser = userProjectPermission.IdUser,
            IdProject = userProjectPermission.IdProject,
            IdPermission = userProjectPermission.IdPermission
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

    public async Task<UserProjectPermission> Post(object data)
    {
        HttpResponseMessage response = await restClient.PostAsync(route, data);
        if (response == null)
        {
            return null;
        }
        var userProjectPermission = await response.Content.ReadFromJsonAsync<UserProjectPermissionRead>(restClient._options);
        return new UserProjectPermission
        {
            Id = userProjectPermission.Id,
            IdUser = userProjectPermission.IdUser,
            IdProject = userProjectPermission.IdProject,
            IdPermission = userProjectPermission.IdPermission
        };
    }
}
