using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Dtos.Concepts;
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

    public async Task<List<Concept>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        var concepts = await response.Content.ReadFromJsonAsync<List<ConceptRead>>(restClient._options);
        return new List<Concept>(concepts.Select(concept => new Concept
        {
            Id = concept.Id,
            Title = concept.Title,
            Description = concept.Description,
            IdProject = concept.IdProject
        }).ToList());
    }

    public async Task<List<Concept>> GetAllConceptsByProject(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/project/{id}");
        var concepts = await response.Content.ReadFromJsonAsync<List<ConceptRead>>(restClient._options);
        return new List<Concept>(concepts.Select(concept => new Concept
        {
            Id = concept.Id,
            Title = concept.Title,
            Description = concept.Description,
            IdProject = concept.IdProject
        }).ToList());
    }

    public async Task<Concept> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        var concept = await response.Content.ReadFromJsonAsync<ConceptRead>(restClient._options);
        return new Concept
        {
            Id = concept.Id,
            Title = concept.Title,
            Description = concept.Description,
            IdProject = concept.IdProject
        };
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
