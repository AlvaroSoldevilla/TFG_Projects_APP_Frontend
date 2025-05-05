using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.ConceptsService;

internal class ConceptsService(RestClient restClient) : IConceptsService
{
    public async Task<string> Delete(string query, int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ObservableCollection<Concept>> GetAll(string query)
    {
        throw new NotImplementedException();
    }

    public async Task<ObservableCollection<Concept>> GetAllConceptsByProject(string query, int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Concept> GetById(string query, int id)
    {
        throw new NotImplementedException();
    }

    public async Task<string> Patch(string query, object data)
    {
        throw new NotImplementedException();
    }

    public async Task<string> Post(string query, object data)
    {
        throw new NotImplementedException();
    }
}
