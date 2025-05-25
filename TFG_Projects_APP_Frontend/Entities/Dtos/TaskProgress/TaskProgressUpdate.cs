namespace TFG_Projects_APP_Frontend.Entities.Dtos.TaskProgress;

internal class TaskProgressUpdate
{
    public int? IdSection { get; set; }
    public string? Title { get; set; }
    public bool? ModifiesProgress { get; set; }
    public int? ProgressValue { get; set; }
    public int Order { get; set; }
}
