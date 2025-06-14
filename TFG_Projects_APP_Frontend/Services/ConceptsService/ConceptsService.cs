﻿using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Dtos.Concepts;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.ConceptsService;

/*Implementation of the ConceptsService Interface*/
public class ConceptsService(RestClient restClient) : IConceptsService
{
    private readonly string route = "concepts";
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

    public async Task<List<Concept>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        if (response == null)
        {
            return null;
        }
        var concepts = await response.Content.ReadFromJsonAsync<List<ConceptRead>>(restClient._options);
        return new List<Concept>(concepts.Select(concept => new Concept
        {
            Id = concept.Id,
            IdFirstBoard = concept.IdFirstBoard,
            Title = concept.Title,
            Description = concept.Description,
            IdProject = concept.IdProject
        }).ToList());
    }

    public async Task<List<Concept>> GetAllConceptsByProject(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/project/{id}");
        if (response == null)
        {
            return null;
        }
        var concepts = await response.Content.ReadFromJsonAsync<List<ConceptRead>>(restClient._options);
        return new List<Concept>(concepts.Select(concept => new Concept
        {
            Id = concept.Id,
            Title = concept.Title,
            IdFirstBoard = concept.IdFirstBoard,
            Description = concept.Description,
            IdProject = concept.IdProject
        }).ToList());
    }

    public async Task<Concept> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        if (response == null)
        {
            return null;
        }
        var concept = await response.Content.ReadFromJsonAsync<ConceptRead>(restClient._options);
        return new Concept
        {
            Id = concept.Id,
            Title = concept.Title,
            IdFirstBoard = concept.IdFirstBoard,
            Description = concept.Description,
            IdProject = concept.IdProject
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

    public async Task<Concept> Post(object data)
    {
        HttpResponseMessage response = await restClient.PostAsync(route, data);
        if (response == null)
        {
            return null;
        }
        var concept = await response.Content.ReadFromJsonAsync<ConceptRead>(restClient._options);
        return new Concept
        {
            Id = concept.Id,
            Title = concept.Title,
            IdFirstBoard = concept.IdFirstBoard,
            Description = concept.Description,
            IdProject = concept.IdProject
        };
    }
}
