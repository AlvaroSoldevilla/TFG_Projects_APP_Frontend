namespace TFG_Projects_APP_Frontend.Entities.Models;

public class TaskSection
{
    public int Id { get; set; }
    public int IdBoard { get; set; }
    public string Title { get; set; }

    public TaskBoard TaskBoard { get; set; }
    public List<TaskProgress>? ProgressSections { get; set; }
    public List<Task>? Tasks { get; set; }
}
