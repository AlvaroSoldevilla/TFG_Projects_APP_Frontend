namespace TFG_Projects_APP_Frontend.Entities.Dtos.Priorities;

internal class PriorityCreate
{
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int PriorityValue { get; set; } = 1;
}
