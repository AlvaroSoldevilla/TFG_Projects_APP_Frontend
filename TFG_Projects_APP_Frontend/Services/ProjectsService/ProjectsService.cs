using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Dtos.Projects;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.ProjectsService;

public class ProjectsService(RestClient restClient) : IProjectsService
{
    private readonly string route = "projects";
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

    public async Task<List<Project>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        if (response == null)
        {
            return null;
        }
        var projects = await response.Content.ReadFromJsonAsync<List<ProjectRead>>(restClient._options);
        return new List<Project>(projects.Select(project => {
            return new Project
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description
            };
        }).ToList());
    }

    public async Task<List<Project>> GetAllProjectsByUser(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/user/{id}");
        if (response == null)
        {
            return null;
        }
        var projects = await response.Content.ReadFromJsonAsync<List<ProjectRead>>(restClient._options);
        return new List<Project>(projects.Select(project => {
            return new Project
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description
            };
        }).ToList());
    }

    public async Task<Project> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        if (response == null)
        {
            return null;
        }
        var project = await response.Content.ReadFromJsonAsync<ProjectRead>(restClient._options);
        return new Project
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description
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

    public async Task<Project> Post(object data)
    {
        HttpResponseMessage response = await restClient.PostAsync(route, data);
        if (response == null)
        {
            return null;
        }
        var result = await response.Content.ReadAsStringAsync();
        var project = await response.Content.ReadFromJsonAsync<ProjectRead>(restClient._options);
        return new Project
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description
        };
    }
}
