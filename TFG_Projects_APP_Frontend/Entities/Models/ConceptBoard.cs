namespace TFG_Projects_APP_Frontend.Entities.Models;

class ConceptBoard
{
    public int Id { get; set; }
    public int IdConcept { get; set; }
    public int? IdParent { get; set; }

    public Concept Concept { get; set; }
    public ConceptBoard? Parent { get; set; }
    public List<ConceptBoard>? Children { get; set; }
    public List<Component>? Components { get; set; }

}
