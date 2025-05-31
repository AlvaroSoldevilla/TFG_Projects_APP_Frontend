using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Dtos.Priorities;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.PrioritiesService;

public class PrioritiesService(RestClient restClient) : IPrioritiesService
{
    private readonly string route = "priorities";
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

    public async Task<List<Priority>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        if (response == null)
        {
            return null;
        }
        var priorities = await response.Content.ReadFromJsonAsync<List<PriorityRead>>(restClient._options);
        return new List<Priority>(priorities.Select(priority =>
        {
            return new Priority
            {
                Id = priority.Id,
                Name = priority.Name,
                Color = priority.Color,
                PriorityValue = priority.PriorityValue,
            };
        }).ToList());
    }

    public async Task<Priority> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        if (response == null)
        {
            return null;
        }
        var priority = await response.Content.ReadFromJsonAsync<PriorityRead>(restClient._options);
        return new Priority
        {
            Id = priority.Id,
            Name = priority.Name,
            Color = priority.Color,
            PriorityValue = priority.PriorityValue,
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

    public async Task<Priority> Post(object data)
    {
        HttpResponseMessage response = await restClient.PostAsync(route, data);
        if (response == null)
        {
            return null;
        }
        var priority = await response.Content.ReadFromJsonAsync<PriorityRead>(restClient._options);
        return new Priority
        {
            Id = priority.Id,
            Name = priority.Name,
            Color = priority.Color,
            PriorityValue = priority.PriorityValue,
        };
    }
}
