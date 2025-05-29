using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.ComponentsService;

public interface IComponentsService : IService<ConceptComponent>
{
    Task<List<ConceptComponent>> GetAllComponentsByBoard(int id);
}