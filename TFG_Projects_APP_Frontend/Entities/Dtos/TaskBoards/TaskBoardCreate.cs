namespace TFG_Projects_APP_Frontend.Entities.Dtos.TaskBoards;

internal class TaskBoardCreate
{
    public int IdProject { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
}
