using System.Collections.ObjectModel;
using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.ComponentsService;

public class ComponentsService(RestClient restClient) : IComponentsService
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
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/board/{id}");
        var components = await response.Content.ReadFromJsonAsync<ObservableCollection<Component>>(restClient._options);
        return components;
    }

    public async Task<Component> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync($"{route}", id);
        var component = await response.Content.ReadFromJsonAsync<Component>(restClient._options);
        return component;
    }

    public async Task<string> Patch(int id, object data)
    {
        HttpResponseMessage response = await restClient.PatchAsync($"{route}", id, data);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<string> Post(object data)
    {
        HttpResponseMessage response = await restClient.PostAsync($"{route}", data);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }
}
