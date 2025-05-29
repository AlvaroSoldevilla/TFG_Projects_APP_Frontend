namespace TFG_Projects_APP_Frontend.Entities.Models;

public class ProjectType
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;

    public List<ConceptComponent>? Components { get; set; }
}
