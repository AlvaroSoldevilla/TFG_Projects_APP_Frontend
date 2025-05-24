namespace TFG_Projects_APP_Frontend.Entities.Models;

public class AppUser
{
    public int Id { get; set; }
    public string? Username { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public Role? Role { get; set; }
    public List<UserProjectPermission>? ProjectPermissions { get; set; }
    public List<ProjectUser>? Projects { get; set; }
    public List<ProjectTask>? TasksCreated { get; set; }
    public List<ProjectTask>? TasksAssigned { get; set; }
}
