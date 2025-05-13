using System.Collections.ObjectModel;
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
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<ObservableCollection<Project>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        var projects = await response.Content.ReadFromJsonAsync<ObservableCollection<ProjectRead>>(restClient._options);
        return new ObservableCollection<Project>(projects.Select(project => {
            return new Project
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description
            };
        }).ToList());
    }

    public async Task<ObservableCollection<Project>> GetAllProjectsByUser(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/user/{id}");
        var projects = await response.Content.ReadFromJsonAsync<ObservableCollection<ProjectRead>>(restClient._options);
        return new ObservableCollection<Project>(projects.Select(project => {
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
