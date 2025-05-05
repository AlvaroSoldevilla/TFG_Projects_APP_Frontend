using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.TypesService
{
    internal class TypesService(RestClient restClient) : ITypesService
    {
        public Task<string> Delete(string query, int id)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<Type>> GetAll(string query)
        {
            throw new NotImplementedException();
        }

        public Task<Type> GetById(string query, int id)
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
