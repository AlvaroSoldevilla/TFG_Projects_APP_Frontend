using System.Collections.ObjectModel;

namespace TFG_Projects_APP_Frontend.Services;

public interface IService<T>
{
    Task<ObservableCollection<T>> GetAll();
    Task<T> GetById(int id);
    Task<string> Post(object data);
    Task<string> Patch(int id, object data);
    Task<string> Delete(int id);
}
