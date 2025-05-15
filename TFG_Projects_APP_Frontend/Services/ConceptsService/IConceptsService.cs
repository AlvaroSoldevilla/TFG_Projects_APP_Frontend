using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.ConceptsService;

public interface IConceptsService : IService<Concept>
{
    Task<List<Concept>> GetAllConceptsByProject(int id);
}
