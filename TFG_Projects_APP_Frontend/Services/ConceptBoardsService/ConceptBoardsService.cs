﻿using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Dtos.ConceptBoards;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.ConceptBoardsService;

/*Implementation of the ConceptBoardsService Interface*/
public class ConceptBoardsService(RestClient restClient) : IConceptBoardsService
{
    private readonly string route = "concept_boards";
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

    public async Task<List<ConceptBoard>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        if (response == null)
        {
            return null;
        }
        var conceptBoards = await response.Content.ReadFromJsonAsync<List<ConceptBoardRead>>(restClient._options);
        return new List<ConceptBoard>(conceptBoards.Select(conceptBoard =>
        {
            return new ConceptBoard
            {
                Id = conceptBoard.Id,
                Name = conceptBoard.Name,
                IdConcept = conceptBoard.IdConcept,
                IdParent = conceptBoard.IdParent
            };
        }).ToList());
    }

    public async Task<List<ConceptBoard>> GetAllConceptBoardsByConcept(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/concept/{id}");
        if (response == null)
        {
            return null;
        }
        var conceptBoards = await response.Content.ReadFromJsonAsync<List<ConceptBoardRead>>(restClient._options);
        return new List<ConceptBoard>(conceptBoards.Select(conceptBoard =>
        {
            return new ConceptBoard
            {
                Id = conceptBoard.Id,
                Name = conceptBoard.Name,
                IdConcept = conceptBoard.IdConcept,
                IdParent = conceptBoard.IdParent
            };
        }).ToList());
    }

    public async Task<ConceptBoard> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        if (response == null)
        {
            return null;
        }
        var conceptBoard = await response.Content.ReadFromJsonAsync<ConceptBoardRead>(restClient._options);
        return new ConceptBoard 
        {
            Id = conceptBoard.Id,
            Name = conceptBoard.Name,
            IdConcept = conceptBoard.IdConcept,
            IdParent = conceptBoard.IdParent
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

    public async Task<ConceptBoard> Post(object data)
    {
        HttpResponseMessage response = await restClient.PostAsync(route, data);
        if (response == null)
        {
            return null;
        }
        var conceptBoard = await response.Content.ReadFromJsonAsync<ConceptBoardRead>(restClient._options);
        return new ConceptBoard
        {
            Id = conceptBoard.Id,
            Name = conceptBoard.Name,
            IdConcept = conceptBoard.IdConcept,
            IdParent = conceptBoard.IdParent
        };
    }
}
