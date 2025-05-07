using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.TypesService
{
    internal class TypesService(RestClient restClient) : ITypesService
    {
        public Task<string> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<Type>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Type> GetById(int id)
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
