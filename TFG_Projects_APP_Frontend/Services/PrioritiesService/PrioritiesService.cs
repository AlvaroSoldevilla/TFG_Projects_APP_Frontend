using System.Collections.ObjectModel;
using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.PrioritiesService;

internal class PrioritiesService(RestClient restClient) : IPrioritiesService
{
    private readonly string route = "priorities";
    public async Task<string> Delete(int id)
    {
        HttpResponseMessage response = await restClient.DeleteAsync(route, id);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<ObservableCollection<Priority>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        var priorities = await response.Content.ReadFromJsonAsync<ObservableCollection<Priority>>(restClient._options);
        return priorities;
    }

    public async Task<Priority> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        var priority = await response.Content.ReadFromJsonAsync<Priority>(restClient._options);
        return priority;
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
