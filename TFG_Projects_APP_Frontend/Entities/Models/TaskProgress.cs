namespace TFG_Projects_APP_Frontend.Entities.Models;

public class TaskProgress
{
    public int Id { get; set; }
    public int IdSection { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool ModifiesProgress { get; set; } = false;
    public int ProgressValue { get; set; }
    public int Order { get; set; } = 0;

    public TaskSection? TaskSection { get; set; }
    public List<ProjectTask>? Tasks { get; set; }
}
