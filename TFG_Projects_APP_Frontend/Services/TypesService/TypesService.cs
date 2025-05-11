using System.Collections.ObjectModel;
using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.TypesService;

public class TypesService(RestClient restClient) : ITypesService
{
    private readonly string route = "types";
    public async Task<string> Delete(int id)
    {
        HttpResponseMessage response = await restClient.DeleteAsync(route, id);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<ObservableCollection<Type>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        var types = await response.Content.ReadFromJsonAsync<ObservableCollection<Type>>(restClient._options);
        return types;
    }

    public async Task<Type> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        var type = await response.Content.ReadFromJsonAsync<Type>(restClient._options);
        return type;
    }

    public async Task<string> Patch(int id, object data)
    {
        HttpResponseMessage response = await restClient.PatchAsync(route, id, data);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<string> Post(object data)
    {
        HttpResponseMessage response = await restClient.PostAsync(route, data);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }
}
