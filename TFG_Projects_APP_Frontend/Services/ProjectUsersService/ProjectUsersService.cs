﻿using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Dtos.ProjectUsers;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.ProjectUsersService;

public class ProjectUsersService(RestClient restClient) : IProjectUsersService
{
    private readonly string route = "project_users";
    public async Task<string> Delete(int id)
    {
        HttpResponseMessage response = await restClient.DeleteAsync(route, id);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<List<ProjectUser>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        var projectUsers = await response.Content.ReadFromJsonAsync<List<ProjectUserRead>>(restClient._options);
        return new List<ProjectUser>(projectUsers.Select(projectUser =>
        {
            return new ProjectUser
            {
                Id = projectUser.Id,
                IdUser = projectUser.IdUser,
                IdProject = projectUser.IdProject,
                IdRole = projectUser.IdRole
            };
        }).ToList());
    }

    public async Task<List<ProjectUser>> GetAllProjectUsersByProject(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/project/{id}");
        var projectUsers = await response.Content.ReadFromJsonAsync<List<ProjectUserRead>>(restClient._options);
        return new List<ProjectUser>(projectUsers.Select(projectUser =>
        {
            return new ProjectUser
            {
                Id = projectUser.Id,
                IdUser = projectUser.IdUser,
                IdProject = projectUser.IdProject,
                IdRole = projectUser.IdRole
            };
        }).ToList());
    }

    public async Task<List<ProjectUser>> GetAllProjectUsersByUser(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/user/{id}");
        var projectUsers = await response.Content.ReadFromJsonAsync<List<ProjectUserRead>>(restClient._options);
        return new List<ProjectUser>(projectUsers.Select(projectUser =>
        {
            return new ProjectUser
            {
                Id = projectUser.Id,
                IdUser = projectUser.IdUser,
                IdProject = projectUser.IdProject,
                IdRole = projectUser.IdRole
            };
        }).ToList());
    }

    public async Task<ProjectUser> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        var projectUser = await response.Content.ReadFromJsonAsync<ProjectUserRead>(restClient._options);
        return new ProjectUser
        {
            Id = projectUser.Id,
            IdUser = projectUser.IdUser,
            IdProject = projectUser.IdProject,
            IdRole = projectUser.IdRole
        };
    }

    public async Task<ProjectUser> GetProjectUserByUserAndProject(int userId, int projectId)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync($"{route}/project/{projectId}/user", userId);
        var projectUser = await response.Content.ReadFromJsonAsync<ProjectUserRead>(restClient._options);
        return new ProjectUser
        {
            Id = projectUser.Id,
            IdUser = projectUser.IdUser,
            IdProject = projectUser.IdProject,
            IdRole = projectUser.IdRole
        };
    }

    public async Task<string> Patch(int id, object data)
    {
        HttpResponseMessage response = await restClient.PatchAsync(route, id, data);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<ProjectUser> Post(object data)
    {
        HttpResponseMessage response = await restClient.PostAsync(route, data);
        var projectUser = await response.Content.ReadFromJsonAsync<ProjectUserRead>(restClient._options);
        return new ProjectUser
        {
            Id = projectUser.Id,
            IdUser = projectUser.IdUser,
            IdProject = projectUser.IdProject,
            IdRole = projectUser.IdRole
        };
    }
}
