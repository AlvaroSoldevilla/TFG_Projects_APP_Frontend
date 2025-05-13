namespace TFG_Projects_APP_Frontend.Entities.Models;

public class Component
{
    public int Id { get; set; }
    public int IdBoard { get; set; }
    public int? IdParent { get; set; }
    public int IdType { get; set; }
    public int? PosX { get; set; }
    public int? PosY { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }

    public ConceptBoard Board { get; set; }
    public Component? Parent { get; set; }
    public List<Component>? Children { get; set; }
    public Type Type { get; set; }
}
