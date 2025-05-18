namespace TFG_Projects_APP_Frontend.Entities.Dtos.TaskSections;

internal class TaskSectionCreate
{
    public int IdBoard { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Order { get; set; } = 0;
}
