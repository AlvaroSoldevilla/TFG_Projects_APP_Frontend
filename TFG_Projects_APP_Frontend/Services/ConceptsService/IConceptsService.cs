using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.ConceptsService;

/*Inherits IService and adds model specific methods*/
public interface IConceptsService : IService<Concept>
{
    /*Get every concept from a project*/
    Task<List<Concept>> GetAllConceptsByProject(int id);
}
