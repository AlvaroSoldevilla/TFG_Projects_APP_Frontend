using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.TaskSectionsService
{
    internal class TaskSectionsService(RestClient restClient) : ITaskSectionsService
    {
        public Task<string> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<TaskSection>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<TaskSection>> GetAllTaskSectionsByTaskBoard(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<TaskSection>> getAllTaskSectionsByTaskBoard(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TaskSection> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<string> Patch(int id, object data)
        {
            throw new NotImplementedException();
        }

        public Task<string> Post(object data)
        {
            throw new NotImplementedException();
        }
    }
}
