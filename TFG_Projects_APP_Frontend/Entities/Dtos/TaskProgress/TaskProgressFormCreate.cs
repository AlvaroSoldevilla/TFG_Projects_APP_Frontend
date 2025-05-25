namespace TFG_Projects_APP_Frontend.Entities.Dtos.TaskProgress;

internal class TaskProgressFormCreate
{
    public string Name { get; set; } = string.Empty;
    public bool ModifiesProgress { get; set; } = false;
    public int? ProgressValue { get; set; } = 0;
}
