using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.ConceptBoardsService;

public interface IConceptBoardsService : IService<ConceptBoard>
{
    Task<List<ConceptBoard>> GetAllConceptBoardsByConcept(int id);
}
