using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.ConceptBoardsService;

public interface IConceptBoardsService : IService<ConceptBoard>
{
    Task<ObservableCollection<ConceptBoard>> GetAllConceptBoardsByConcept(int id);
}
