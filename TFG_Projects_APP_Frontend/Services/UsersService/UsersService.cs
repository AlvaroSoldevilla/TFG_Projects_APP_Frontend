using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Dtos.Users;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.UsersService;

public class UsersService(RestClient restClient) : IUsersService
{
    private readonly string route = "users";
    public async Task<AppUser> AuthenticateUser(object data)
    {
        HttpResponseMessage response = await restClient.AuthenticateUser($"{route}/auth/", data);
        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            return null;
        }
        var result = await response.Content.ReadFromJsonAsync<AppUser>(restClient._options);
        return result;
    }

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

    public async Task<List<AppUser>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        if (response == null)
        {
            return null;
        }
        var users = await response.Content.ReadFromJsonAsync<List<UserRead>>(restClient._options);
        return new List<AppUser>(users.Select(user =>
        {
            return new AppUser
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
            };
        }).ToList());
    }

    public async Task<List<AppUser>> GetUsersByProject(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/project/{id}");
        if (response == null)
        {
            return null;
        }
        var users = await response.Content.ReadFromJsonAsync<List<UserRead>>(restClient._options);
        return new List<AppUser>(users.Select(user =>
        {
            return new AppUser
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
            };
        }).ToList());
    }

    public async Task<AppUser> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        if (response == null)
        {
            return null;
        }
        var user = await response.Content.ReadFromJsonAsync<UserRead>(restClient._options);
        return new AppUser
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
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

    public async Task<AppUser> Post(object data)
    {
        HttpResponseMessage response = await restClient.PostAsync(route, data);
        if (response == null)
        {
            return null;
        }
        var user = await response.Content.ReadFromJsonAsync<UserRead>(restClient._options);
        return new AppUser
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
        };
    }

    public async Task<AppUser> GetUserByEmail(string email)
    {
        var userEmail = new UserEmail { Email = email };
        HttpResponseMessage response = await restClient.PostAsync($"{route}/email/", userEmail);
        if (response == null)
        {
            return null;
        }
        var user = await response.Content.ReadFromJsonAsync<UserRead>(restClient._options);
        return new AppUser
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
        };
    }
}
