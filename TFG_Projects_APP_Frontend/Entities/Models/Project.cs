namespace TFG_Projects_APP_Frontend.Entities.Models;

public class Project
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public List<UserProjectPermission>? UserPermissions { get; set; }
    public List<ProjectUser>? Users { get; set; }
    public List<Concept>? Concepts { get; set; }
    public List<TaskBoard>? Tasks { get; set; }
}
