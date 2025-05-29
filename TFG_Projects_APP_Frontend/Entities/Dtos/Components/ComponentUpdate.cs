namespace TFG_Projects_APP_Frontend.Entities.Dtos.Components;

internal class ComponentUpdate
{
    public int? IdBoard { get; set; }
    public int? IdType { get; set; }
    public string? Title { get; set; }
    public int? IdParent { get; set; }
    public double? PosX { get; set; }
    public double? PosY { get; set; }
    public string? Content { get; set; }
}
