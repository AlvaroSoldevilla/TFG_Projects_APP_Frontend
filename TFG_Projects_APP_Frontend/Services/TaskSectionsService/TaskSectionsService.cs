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
        public Task<string> Delete(string query, int id)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<TaskSection>> GetAll(string query)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<TaskSection>> GetAllTaskSectionsByTaskBoard(string query, int id)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<TaskSection>> getAllTaskSectionsByTaskBoard(string query, int id)
        {
            throw new NotImplementedException();
        }

        public Task<TaskSection> GetById(string query, int id)
        {
            throw new NotImplementedException();
        }

        public Task<string> Patch(string query, object data)
        {
            throw new NotImplementedException();
        }

        public Task<string> Post(string query, object data)
        {
            throw new NotImplementedException();
        }
    }
}
