namespace TFG_Projects_APP_Frontend.Entities.Dtos.Concepts;

internal class ConceptCreate
{
    public int IdProject { get; set; }
    public int IdFirstBoard { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
}
