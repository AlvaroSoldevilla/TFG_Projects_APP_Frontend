namespace TFG_Projects_APP_Frontend.Entities.Models;

class UserProjectPermission
{
    public int Id { get; set; }
    public int IdPermission { get; set; }
    public int UserId { get; set; }
    public int ProjectId { get; set; }

    public Permission? Permission { get; set; }
    public AppUser? User { get; set; }
    public Project? Project { get; set; }
}
