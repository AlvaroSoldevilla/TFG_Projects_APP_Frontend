namespace TFG_Projects_APP_Frontend.Entities.Models;

public class ProjectUser
{
    public int Id { get; set; }
    public int IdUser { get; set; }
    public int IdProject { get; set; }
    public int? IdRole { get; set; }

    public AppUser? User { get; set; }
    public Project? Project { get; set; }
    public Role? Role { get; set; }
}
