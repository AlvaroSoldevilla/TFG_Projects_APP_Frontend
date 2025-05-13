namespace TFG_Projects_APP_Frontend.Entities.Models;

public class UserProjectPermission
{
    public int Id { get; set; }
    public int IdPermission { get; set; }
    public int IdUser { get; set; }
    public int IdProject { get; set; }

    public Permission? Permission { get; set; }
    public AppUser? User { get; set; }
    public Project? Project { get; set; }
}
