namespace TFG_Projects_APP_Frontend.Entities.Models;

class Concept
{
    public int Id { get; set; }
    public int IdProject { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public Project Project { get; set; }
    public List<ConceptBoard>? Boards { get; set; }
}
