using System.Collections.ObjectModel;
using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Dtos.ConceptBoards;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.ConceptBoardsService;

public class ConceptBoardsService(RestClient restClient) : IConceptBoardsService
{
    private readonly string route = "concept_boards";
    public async Task<string> Delete(int id)
    {
        HttpResponseMessage response = await restClient.DeleteAsync(route, id);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<ObservableCollection<ConceptBoard>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        var conceptBoards = await response.Content.ReadFromJsonAsync<ObservableCollection<ConceptBoardRead>>(restClient._options);
        return new ObservableCollection<ConceptBoard>(conceptBoards.Select(conceptBoard =>
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

    public async Task<ObservableCollection<ConceptBoard>> GetAllConceptBoardsByConcept(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/concept/{id}");
        var conceptBoards = await response.Content.ReadFromJsonAsync<ObservableCollection<ConceptBoardRead>>(restClient._options);
        return new ObservableCollection<ConceptBoard>(conceptBoards.Select(conceptBoard =>
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
