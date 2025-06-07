using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.Utils;

public static class NavigationContext
{
    public static bool Startup { get; set; } = true;
    public static Project CurrentProject { get; set; }
    public static TaskBoard CurrentTaskBoard { get; set; }
    public static Concept CurrentConcept { get; set; }
    public static TaskSection CurrentTaskSection { get; set; }
    public static Stack<ConceptBoard> CurrentConceptBoards { get; set; } = new Stack<ConceptBoard>();
}
