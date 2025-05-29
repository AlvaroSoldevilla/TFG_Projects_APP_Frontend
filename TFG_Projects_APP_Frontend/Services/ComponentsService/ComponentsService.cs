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

    public async Task<List<ConceptComponent>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        var components = await response.Content.ReadFromJsonAsync<List<ComponentRead>>(restClient._options);
        return new List<ConceptComponent>(components.Select(component => new ConceptComponent
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

    public async Task<List<ConceptComponent>> GetAllComponentsByBoard(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/board/{id}");
        var components = await response.Content.ReadFromJsonAsync<List<ComponentRead>>(restClient._options);
        return new List<ConceptComponent>(components.Select(component => new ConceptComponent
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

    public async Task<ConceptComponent> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync($"{route}", id);
        var component = await response.Content.ReadFromJsonAsync<ComponentRead>(restClient._options);
        return new ConceptComponent
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

    public async Task<ConceptComponent> Post(object data)
    {
        HttpResponseMessage response = await restClient.PostAsync($"{route}", data);
        var component = await response.Content.ReadFromJsonAsync<ComponentRead>(restClient._options);
        return new ConceptComponent
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
}
