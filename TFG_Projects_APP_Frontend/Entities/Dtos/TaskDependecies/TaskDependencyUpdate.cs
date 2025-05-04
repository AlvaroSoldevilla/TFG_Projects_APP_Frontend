namespace TFG_Projects_APP_Frontend.Entities.Dtos.TaskDependecies;

internal class TaskDependencyUpdate
{
    public int? IdTask { get; set; }
    public int? IdDependsOn { get; set; }
    public int? UnlockAt { get; set; }
}
