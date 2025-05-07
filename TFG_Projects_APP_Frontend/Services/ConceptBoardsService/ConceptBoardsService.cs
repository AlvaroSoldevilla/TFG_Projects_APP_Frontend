using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.ConceptBoardsService;

internal class ConceptBoardsService(RestClient restClient) : IConceptBoardsService
{
    public async Task<string> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ObservableCollection<ConceptBoard>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<ObservableCollection<ConceptBoard>> GetAllConceptBoardsByConcept(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ConceptBoard> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<string> Patch(int id, object data)
    {
        throw new NotImplementedException();
    }

    public async Task<string> Post(object data)
    {
        throw new NotImplementedException();
    }
}
