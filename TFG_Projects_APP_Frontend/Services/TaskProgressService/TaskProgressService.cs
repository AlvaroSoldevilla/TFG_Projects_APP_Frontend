using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.TaskProgressService
{
    internal class TaskProgressService(RestClient restClient) : ITaskProgressService
    {
        public Task<string> Delete(string query, int id)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<TaskProgress>> GetAll(string query)
        {
            throw new NotImplementedException();
        }

        public Task<TaskProgress> GetById(string query, int id)
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
