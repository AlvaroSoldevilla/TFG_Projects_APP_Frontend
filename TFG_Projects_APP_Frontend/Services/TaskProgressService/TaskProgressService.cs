using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.TaskProgressService
{
    internal class TaskProgressService(RestClient restClient) : ITaskProgressService
    {
        public Task<string> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<TaskProgress>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<TaskProgress> GetById(int id)
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
