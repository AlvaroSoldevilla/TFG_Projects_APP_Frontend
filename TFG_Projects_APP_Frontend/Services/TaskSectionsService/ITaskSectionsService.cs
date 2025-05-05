using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.TaskSectionsService
{
    internal interface ITaskSectionsService : IService<TaskSection>
    {
        Task<ObservableCollection<TaskSection>> getAllTaskSectionsByTaskBoard(string query, int id);
    }
}
