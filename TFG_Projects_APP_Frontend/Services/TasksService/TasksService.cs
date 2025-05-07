using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.TasksService
{
    internal class TasksService(RestClient restClient) : ITasksService
    {
        public Task<string> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<ProjectTask>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<ProjectTask>> GetAllTasksByTaskProgress(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<ProjectTask>> GetAllTasksByTaskSection(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectTask> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AppUser> getUserAssigned(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AppUser> getUserCreated(int id)
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
