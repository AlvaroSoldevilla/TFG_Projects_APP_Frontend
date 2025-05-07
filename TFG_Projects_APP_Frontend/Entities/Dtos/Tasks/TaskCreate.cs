namespace TFG_Projects_APP_Frontend.Entities.Dtos.Tasks;

internal class TaskCreate
{
    public int IdSection { get; set; }
    public int IdProgressSection { get; set; }
    public int IdUserCreated { get; set; }
    public string Title { get; set; } = string.Empty;
    public int? IdUserAssigned { get; set; }
    public int? IdParent { get; set; }
    public int? IdPriority { get; set; }
    public string? Description { get; set; }
    public int? Progress { get; set; } = 0;
    public DateTime? CreationDate { get; set; }
    public DateTime? LimitDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public bool? Finished { get; set; } = false;
}
