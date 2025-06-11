namespace TFG_Projects_APP_Frontend.Services;

/*General interface for all services that connect to the API through RestClient*/
public interface IService<T>
{
    /*Used to call GetAll*/
    Task<List<T>> GetAll();
    /*Used to call GetById*/
    Task<T> GetById(int id);
    /*Used to call Post*/
    Task<T> Post(object data);
    /*Used to call Patch*/
    Task<string> Patch(int id, object data);
    /*Used to call Delete*/
    Task<string> Delete(int id);
}
