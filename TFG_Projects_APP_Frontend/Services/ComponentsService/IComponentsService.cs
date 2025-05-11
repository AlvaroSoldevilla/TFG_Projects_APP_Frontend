using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.ComponentsService;

public interface IComponentsService : IService<Component>
{
    Task<ObservableCollection<Component>> GetAllComponentsByBoard(int id);
}