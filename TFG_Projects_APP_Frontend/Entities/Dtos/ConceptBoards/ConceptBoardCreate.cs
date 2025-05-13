namespace TFG_Projects_APP_Frontend.Entities.Dtos.ConceptBoards;

internal class ConceptBoardCreate
{
    public int IdConcept { get; set; }
    public int IdParent { get; set; }
    public string Name { get; set; } = string.Empty;
}
