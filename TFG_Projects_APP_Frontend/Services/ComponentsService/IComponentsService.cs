using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.ComponentsService;

/*Inherits IService and adds model specific methods*/
public interface IComponentsService : IService<ConceptComponent>
{
    /*Get every component from a concept board*/
    Task<List<ConceptComponent>> GetAllComponentsByBoard(int id);
}