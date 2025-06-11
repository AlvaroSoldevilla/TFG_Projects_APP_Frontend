using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Dtos.Types;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.TypesService;

/*Implementation of the TypeService Interface*/
public class TypesService(RestClient restClient) : ITypesService
{
    private readonly string route = "types";
    public async Task<string> Delete(int id)
    {
        HttpResponseMessage response = await restClient.DeleteAsync(route, id);
        if (response == null)
        {
            return null;
        }
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<List<ProjectType>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        if (response == null)
        {
            return null;
        }
        var types = await response.Content.ReadFromJsonAsync<List<TypeRead>>(restClient._options);
        return new List<ProjectType>(types.Select(type =>
        {
            return new ProjectType
            {
                Id = type.Id,
                Name = type.Name
            };
        }).ToList());
    }

    public async Task<ProjectType> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        if (response == null)
        {
            return null;
        }
        var type = await response.Content.ReadFromJsonAsync<TypeRead>(restClient._options);
        return new ProjectType
        {
            Id = type.Id,
            Name = type.Name
        };
    }

    public async Task<string> Patch(int id, object data)
    {
        HttpResponseMessage response = await restClient.PatchAsync(route, id, data);
        if (response == null)
        {
            return null;
        }
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<ProjectType> Post(object data)
    {
        HttpResponseMessage response = await restClient.PostAsync(route, data);
        if (response == null)
        {
            return null;
        }
        var type = await response.Content.ReadFromJsonAsync<TypeRead>(restClient._options);
        return new ProjectType
        {
            Id = type.Id,
            Name = type.Name
        };
    }
}
