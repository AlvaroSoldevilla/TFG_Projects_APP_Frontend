using System.Collections.ObjectModel;
using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Dtos.Components;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.ComponentsService;

internal class ComponentsService(RestClient restClient) : IComponentsService
{
    private readonly string route = "components";
    public async Task<string> Delete(int id)
    {
        HttpResponseMessage response = await restClient.DeleteAsync(route, id);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<ObservableCollection<Component>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        var components = await response.Content.ReadFromJsonAsync<ObservableCollection<Component>>(restClient._options);
        return components;
    }

    public async Task<ObservableCollection<Component>> GetAllComponentsByBoard(int id)
    {
        string query = $"{route}/board/{id}";
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        var components = await response.Content.ReadFromJsonAsync<ObservableCollection<Component>>(restClient._options);
        return components;
    }

    public async Task<Component> GetById(int id)
    {
        string query = $"{route}";
        HttpResponseMessage response = await restClient.GetByIdAsync(query,id);
        var component = await response.Content.ReadFromJsonAsync<Component>(restClient._options);
        return component;
    }

    public async Task<string> Patch(int id, object data)
    {
        string query = $"{route}";
        HttpResponseMessage response = await restClient.PatchAsync(query, id, data);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<string> Post(object data)
    {
        string query = $"{route}";
        HttpResponseMessage response = await restClient.PostAsync(query, data);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }
}
