namespace TFG_Projects_APP_Frontend.Entities.Models;

public class TaskDependency
{
    public int Id { get; set; }
    public int IdTask { get; set; }
    public int IdDependsOn { get; set; }
    public int UnlockAt { get; set; } = 100;

    public ProjectTask? Task { get; set; }
    public ProjectTask? DependsOn { get; set; }
}
