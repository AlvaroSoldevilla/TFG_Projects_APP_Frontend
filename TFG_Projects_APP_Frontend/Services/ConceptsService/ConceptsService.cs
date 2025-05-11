using System.Collections.ObjectModel;
using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.ConceptsService;

public class ConceptsService(RestClient restClient) : IConceptsService
{
    private readonly string route = "concepts";
    public async Task<string> Delete(int id)
    {
        HttpResponseMessage response = await restClient.DeleteAsync(route, id);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<ObservableCollection<Concept>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        var concepts = await response.Content.ReadFromJsonAsync<ObservableCollection<Concept>>(restClient._options);
        return concepts;
    }

    public async Task<ObservableCollection<Concept>> GetAllConceptsByProject(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/project/{id}");
        var concepts = await response.Content.ReadFromJsonAsync<ObservableCollection<Concept>>(restClient._options);
        return concepts;
    }

    public async Task<Concept> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        var concept = await response.Content.ReadFromJsonAsync<Concept>(restClient._options);
        return concept;
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
