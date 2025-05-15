using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Dtos.Types;
using TFG_Projects_APP_Frontend.Entities.Models;
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

    public async Task<List<ProjectType>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
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
