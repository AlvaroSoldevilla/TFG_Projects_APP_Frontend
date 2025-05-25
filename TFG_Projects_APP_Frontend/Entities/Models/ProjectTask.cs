namespace TFG_Projects_APP_Frontend.Entities.Models;

public class ProjectTask
{
    public int Id { get; set; }
    public int IdSection { get; set; }
    public int? IdProgressSection { get; set; }
    public int? IdUserAssigned { get; set; }
    public int? IdParentTask { get; set; }
    public int IdUserCreated { get; set; }
    public int? IdPriority { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Progress { get; set; } = 0;
    public DateTime CreationDate { get; set; }
    public DateTime? LimitDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public bool Finished { get; set; } = false;
    public bool IsParent { get; set; } = false;

    public TaskSection? TaskSection { get; set; }
    public TaskProgress? ProgressSection { get; set; }
    public ProjectTask? Parent { get; set; }
    public List<ProjectTask>? Children { get; set; }
    public AppUser UserCreated { get; set; }
    public AppUser? UserAssigned { get; set; }
    public Priority? Priority { get; set; }
    public List<TaskDependency>? Dependecies { get; set; }
    public List<TaskDependency>? Dependents { get; set; }
}
