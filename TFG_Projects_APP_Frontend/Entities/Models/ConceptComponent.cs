namespace TFG_Projects_APP_Frontend.Entities.Models;

public class ConceptComponent
{
    public int Id { get; set; }
    public int IdBoard { get; set; }
    public int? IdParent { get; set; }
    public int IdType { get; set; }
    public double? PosX { get; set; }
    public double? PosY { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }

    public ConceptBoard Board { get; set; }
    public ConceptComponent? Parent { get; set; }
    public List<ConceptComponent>? Children { get; set; }
    public Type Type { get; set; }
}
