using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.TasksService
{
    internal class TasksService(RestClient restClient) : ITasksService
    {
        public Task<string> Delete(string query, int id)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<ProjectTask>> GetAll(string query)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<ProjectTask>> GetAllTasksByTaskProgress(string query, int id)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<ProjectTask>> GetAllTasksByTaskSection(string query, int id)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectTask> GetById(string query, int id)
        {
            throw new NotImplementedException();
        }

        public Task<AppUser> getUserAssigned(string query, int id)
        {
            throw new NotImplementedException();
        }

        public Task<AppUser> getUserCreated(string query, int id)
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
