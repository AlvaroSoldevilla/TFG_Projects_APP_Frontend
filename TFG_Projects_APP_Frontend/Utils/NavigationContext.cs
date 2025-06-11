using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Utils;

/*Util class to keep track of the navigation. Mainly used for back navigation*/
public static class NavigationContext
{
    /*Variable to check if the user has just logged in*/
    public static bool Startup { get; set; } = true;
    /*Holds the current project*/
    public static Project CurrentProject { get; set; }
    /*Holds the current task board*/
    public static TaskBoard CurrentTaskBoard { get; set; }
    /*Holds the current concept*/
    public static Concept CurrentConcept { get; set; }
    /*Holds the current task section*/
    public static TaskSection CurrentTaskSection { get; set; }
    /*A stack to navigate through concept boards*/
    public static Stack<ConceptBoard> CurrentConceptBoards { get; set; } = new Stack<ConceptBoard>();
}
