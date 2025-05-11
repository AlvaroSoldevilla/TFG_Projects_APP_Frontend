using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.TaskSectionsService;

public interface ITaskSectionsService : IService<TaskSection>
{
    Task<ObservableCollection<TaskSection>> getAllTaskSectionsByTaskBoard(int id);
}
