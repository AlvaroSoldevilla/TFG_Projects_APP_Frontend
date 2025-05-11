namespace TFG_Projects_APP_Frontend.Entities.Models;

public class Permission
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<UserProjectPermission>? UserProjects { get; set; }
}
