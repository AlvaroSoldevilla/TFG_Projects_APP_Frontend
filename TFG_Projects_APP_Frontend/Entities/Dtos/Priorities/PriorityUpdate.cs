namespace TFG_Projects_APP_Frontend.Entities.Dtos.Priorities;

internal class PriorityUpdate
{
    public string? Name { get; set; }
    public string? Color { get; set; }
    public int? PriorityValue { get; set; } = 1;
}
