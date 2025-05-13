using System.Collections.ObjectModel;
using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Dtos.Components;
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
        var components = await response.Content.ReadFromJsonAsync<ObservableCollection<ComponentRead>>(restClient._options);
        return new ObservableCollection<Component>(components.Select(component => new Component
        {
            Id = component.Id,
            Title = component.Title,
            Content = component.Content,
            IdType = component.IdType,
            PosX = component.PosX,
            PosY = component.PosY,
            IdBoard = component.IdBoard,
            IdParent = component.IdParent
        }).ToList());
    }

    public async Task<ObservableCollection<Component>> GetAllComponentsByBoard(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/board/{id}");
        var components = await response.Content.ReadFromJsonAsync<ObservableCollection<ComponentRead>>(restClient._options);
        return new ObservableCollection<Component>(components.Select(component => new Component
        {
            Id = component.Id,
            Title = component.Title,
            Content = component.Content,
            IdType = component.IdType,
            PosX = component.PosX,
            PosY = component.PosY,
            IdBoard = component.IdBoard,
            IdParent = component.IdParent
        }).ToList());
    }

    public async Task<Component> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync($"{route}", id);
        var component = await response.Content.ReadFromJsonAsync<ComponentRead>(restClient._options);
        return new Component
        {
            Id = component.Id,
            Title = component.Title,
            Content = component.Content,
            IdType = component.IdType,
            PosX = component.PosX,
            PosY = component.PosY,
            IdBoard = component.IdBoard,
            IdParent = component.IdParent
        };
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
