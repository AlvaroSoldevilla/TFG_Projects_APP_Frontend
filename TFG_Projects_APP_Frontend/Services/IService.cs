using System.Collections.ObjectModel;

namespace TFG_Projects_APP_Frontend.Services;

internal interface IService<T>
{
    Task<ObservableCollection<T>> GetAll(string query);
    Task<T> GetById(string query, int id);
    Task<string> Post(string query, object data);
    Task<string> Patch(string query, object data);
    Task<string> Delete(string query, int id);
}
