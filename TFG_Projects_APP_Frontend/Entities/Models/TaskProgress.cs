namespace TFG_Projects_APP_Frontend.Entities.Models;

class TaskProgress
{
    public int Id { get; set; }
    public int IdSection { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool ModifiesProgrees { get; set; } = false;
    public int? ProgressValue { get; set; }

    public TaskSection? TaskSection { get; set; }
    public List<ProjectTask>? Tasks { get; set; }
}
