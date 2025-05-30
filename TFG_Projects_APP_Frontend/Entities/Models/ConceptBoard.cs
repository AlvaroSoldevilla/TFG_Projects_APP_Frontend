﻿namespace TFG_Projects_APP_Frontend.Entities.Models;

public class ConceptBoard
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int IdConcept { get; set; }
    public int? IdParent { get; set; }

    public Concept Concept { get; set; }
    public ConceptBoard? Parent { get; set; }
    public List<ConceptBoard>? Children { get; set; }
    public List<ConceptComponent>? Components { get; set; }

}
