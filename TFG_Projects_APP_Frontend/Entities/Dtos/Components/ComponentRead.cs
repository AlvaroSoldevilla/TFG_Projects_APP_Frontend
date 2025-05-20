namespace TFG_Projects_APP_Frontend.Entities.Dtos.Components;

internal class ComponentRead : ComponentCreate
{
    public int Id { get; set; }
    public int? IdParent { get; set; }
}
