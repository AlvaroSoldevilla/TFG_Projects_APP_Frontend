﻿namespace TFG_Projects_APP_Frontend.Entities.Models;

public class Concept
{
    public int Id { get; set; }
    public int IdProject { get; set; }
    public int IdFirstBoard { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public Project Project { get; set; }
    public List<ConceptBoard>? Boards { get; set; }
}
