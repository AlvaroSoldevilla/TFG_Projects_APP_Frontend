namespace TFG_Projects_APP_Frontend.Entities.Dtos.TaskProgress;

internal class TaskProgressCreate
{
    public int IdSection { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool? ModifiesProgress { get; set; } = false;
    public int? ProgressValue { get; set; } = 0;
}
