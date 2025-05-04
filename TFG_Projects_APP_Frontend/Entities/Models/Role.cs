namespace TFG_Projects_APP_Frontend.Entities.Models;

class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public List<ProjectUser>? ProjectUsers { get; set; }
}
