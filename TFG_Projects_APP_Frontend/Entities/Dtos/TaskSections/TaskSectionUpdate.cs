namespace TFG_Projects_APP_Frontend.Entities.Dtos.TaskSections;

internal class TaskSectionUpdate
{
    public int? IdBoard { get; set; }
    public int? IdDefaultProgress { get; set; }
    public string? Title { get; set; }
    public int? Order { get; set; }
}
