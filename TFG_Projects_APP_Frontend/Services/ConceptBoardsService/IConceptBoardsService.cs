using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.ConceptBoardsService;

/*Inherits IService and adds model specific methods*/
public interface IConceptBoardsService : IService<ConceptBoard>
{
    /*Get every board from a concept*/
    Task<List<ConceptBoard>> GetAllConceptBoardsByConcept(int id);
}
