namespace TFG_Projects_APP_Frontend.Entities.Models;

public class Priority
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int PriorityValue { get; set; } = 1;

    public List<ProjectTask>? Tasks { get; set; }
}
