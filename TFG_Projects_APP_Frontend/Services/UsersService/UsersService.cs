using System.Collections.ObjectModel;
using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.UsersService;

internal class UsersService(RestClient restClient) : IUsersService
{
    private readonly string route = "users";
    public async Task<string> AuthenticateUser(string email, object data)
    {
        HttpResponseMessage response = await restClient.PostAsync($"{route}/auth/", data);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<string> Delete(int id)
    {
        HttpResponseMessage response = await restClient.DeleteAsync(route, id);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<ObservableCollection<AppUser>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        var users = await response.Content.ReadFromJsonAsync<ObservableCollection<AppUser>>(restClient._options);
        return users;
    }

    public async Task<AppUser> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        var user = await response.Content.ReadFromJsonAsync<AppUser>(restClient._options);
        return user;
    }

    public async Task<ObservableCollection<AppUser>> GetUsersByProject(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/project/{id}");
        var users = await response.Content.ReadFromJsonAsync<ObservableCollection<AppUser>>(restClient._options);
        return users;
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
