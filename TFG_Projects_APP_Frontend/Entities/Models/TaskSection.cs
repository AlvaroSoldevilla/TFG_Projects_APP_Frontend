namespace TFG_Projects_APP_Frontend.Entities.Models;

public class TaskSection
{
    public int Id { get; set; }
    public int IdBoard { get; set; }
    public int? IdDefaultProgress { get; set; }
    public string Title { get; set; } = String.Empty;
    public int Order { get; set; } = 0;

    public TaskBoard TaskBoard { get; set; }
    public List<TaskProgress>? ProgressSections { get; set; }
    public List<ProjectTask>? Tasks { get; set; }
}
