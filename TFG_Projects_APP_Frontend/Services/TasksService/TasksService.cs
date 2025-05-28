using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Dtos.Tasks;
using TFG_Projects_APP_Frontend.Entities.Dtos.Users;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.TasksService;

public class TasksService(RestClient restClient) : ITasksService
{
    private readonly string route = "tasks";
    public async Task<string> Delete(int id)
    {
        HttpResponseMessage response = await restClient.DeleteAsync(route, id);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<List<ProjectTask>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        var tasks = await response.Content.ReadFromJsonAsync<List<TaskRead>>(restClient._options);
        return new List<ProjectTask>(tasks.Select(task =>
        {
            return new ProjectTask
            {
                Id = task.Id,
                IdSection = task.IdSection,
                IdProgressSection = task.IdProgressSection,
                IdUserAssigned = task.IdUserAssigned,
                IdParentTask = task.IdParentTask,
                IdUserCreated = task.IdUserCreated,
                IdPriority = task.IdPriority,
                Title = task.Title,
                Description = task.Description,
                Progress = task.Progress,
                CreationDate = task.CreationDate,
                LimitDate = task.LimitDate,
                CompletionDate = task.CompletionDate,
                Finished = task.Finished,
                IsParent = task.IsParent
            };
        }).ToList());
    }

    public async Task<List<ProjectTask>> GetAllTasksByTaskProgress(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/progress/{id}");
        var tasks = await response.Content.ReadFromJsonAsync<List<TaskRead>>(restClient._options);
        return new List<ProjectTask>(tasks.Select(task =>
        {
            return new ProjectTask
            {
                Id = task.Id,
                IdSection = task.IdSection,
                IdProgressSection = task.IdProgressSection,
                IdUserAssigned = task.IdUserAssigned,
                IdParentTask = task.IdParentTask,
                IdUserCreated = task.IdUserCreated,
                IdPriority = task.IdPriority,
                Title = task.Title,
                Description = task.Description,
                Progress = task.Progress,
                CreationDate = task.CreationDate,
                LimitDate = task.LimitDate,
                CompletionDate = task.CompletionDate,
                Finished = task.Finished,
                IsParent = task.IsParent
            };
        }).ToList());
    }

    public async Task<List<ProjectTask>> GetAllTasksByTaskSection(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/section/{id}");
        var tasks = await response.Content.ReadFromJsonAsync<List<TaskRead>>(restClient._options);
        return new List<ProjectTask>(tasks.Select(task =>
        {
            return new ProjectTask
            {
                Id = task.Id,
                IdSection = task.IdSection,
                IdProgressSection = task.IdProgressSection,
                IdUserAssigned = task.IdUserAssigned,
                IdParentTask = task.IdParentTask,
                IdUserCreated = task.IdUserCreated,
                IdPriority = task.IdPriority,
                Title = task.Title,
                Description = task.Description,
                Progress = task.Progress,
                CreationDate = task.CreationDate,
                LimitDate = task.LimitDate,
                CompletionDate = task.CompletionDate,
                Finished = task.Finished,
                IsParent = task.IsParent
            };
        }).ToList());
    }

    public async Task<ProjectTask> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        var task = await response.Content.ReadFromJsonAsync<TaskRead>(restClient._options);
        return new ProjectTask
        {
            Id = task.Id,
            IdSection = task.IdSection,
            IdProgressSection = task.IdProgressSection,
            IdUserAssigned = task.IdUserAssigned,
            IdParentTask = task.IdParentTask,
            IdUserCreated = task.IdUserCreated,
            IdPriority = task.IdPriority,
            Title = task.Title,
            Description = task.Description,
            Progress = task.Progress,
            CreationDate = task.CreationDate,
            LimitDate = task.LimitDate,
            CompletionDate = task.CompletionDate,
            Finished = task.Finished,
            IsParent = task.IsParent
        };
    }

    public async Task<AppUser> getUserAssigned(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync($"{route}/user/assigned", id);
        var user = await response.Content.ReadFromJsonAsync<UserRead>(restClient._options);
        return new AppUser 
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
        };
    }

    public async Task<AppUser> getUserCreated(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync($"{route}/user/created", id);
        var user = await response.Content.ReadFromJsonAsync<UserRead>(restClient._options);
        return new AppUser
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
        };
    }

    public async Task<string> Patch(int id, object data)
    {
        HttpResponseMessage response = await restClient.PatchAsync(route, id, data);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<ProjectTask> Post(object data)
    {
        HttpResponseMessage response = await restClient.PostAsync(route, data);
        var task = await response.Content.ReadFromJsonAsync<TaskRead>(restClient._options);
        return new ProjectTask
        {
            Id = task.Id,
            IdSection = task.IdSection,
            IdProgressSection = task.IdProgressSection,
            IdUserAssigned = task.IdUserAssigned,
            IdParentTask = task.IdParentTask,
            IdUserCreated = task.IdUserCreated,
            IdPriority = task.IdPriority,
            Title = task.Title,
            Description = task.Description,
            Progress = task.Progress,
            CreationDate = task.CreationDate,
            LimitDate = task.LimitDate,
            CompletionDate = task.CompletionDate,
            Finished = task.Finished,
            IsParent = task.IsParent
        };
    }
}
