namespace TFG_Projects_APP_Frontend.Entities.Dtos.TaskDependecies;

internal class TaskDependencyCreate
{
    public int IdTask { get; set; }
    public int IdDependsOn { get; set; }
    public int UnlockAt { get; set; }
}
