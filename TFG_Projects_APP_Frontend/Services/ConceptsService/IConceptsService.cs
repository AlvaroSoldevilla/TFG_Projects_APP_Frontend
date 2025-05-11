using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.ConceptsService;

public interface IConceptsService : IService<Concept>
{
    Task<ObservableCollection<Concept>> GetAllConceptsByProject(int id);
}
