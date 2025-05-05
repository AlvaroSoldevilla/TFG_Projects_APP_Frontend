using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.ComponentsService;

interface IComponentsService : IService<Component>
{
    Task<ObservableCollection<Component>> GetAllComponentsByBoard(string query, int id);
}