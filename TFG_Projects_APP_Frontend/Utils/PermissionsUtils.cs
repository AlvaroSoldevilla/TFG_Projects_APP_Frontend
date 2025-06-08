using TFG_Projects_APP_Frontend.Services.UsersService;

namespace TFG_Projects_APP_Frontend.Utils;

public class PermissionsUtils(UserSession userSession)
{
    public enum Permissions
    {
        FullPermissions = 1,
        ReadTasksBoards = 2,
        CreateTaskBoards = 3,
        CreateTaskBoardSections = 4,
        CreateTasks = 5,
        EditTaskBoards = 6,
        EditTaskBoardSections = 7,
        EditTasks = 8,
        DeleteTasksBoards = 9,
        DeleteTaskBoardSections = 10,
        DeleteTasks = 11,
        FullTaskPermissions = 12,
        ReadConcepts = 13,
        CreateConcepts = 14,
        EditConcepts = 15,
        DeleteConcepts = 16,
        CreateComponents = 17,
        EditComponents = 18,
        DeleteComponents = 19,
        FullConceptPermissions = 20,
        ReadUsers = 21,
        AddUsers = 22,
        EditUsers = 23,
        RemoveUsers = 24,
        FullUserPermissions = 25

    }

    public bool HasAllPermissions(List<Permissions> permissions)
    {
        bool hasAllPermissions = true;
        foreach (int permission in permissions)
        {
            if (!userSession.User.ProjectPermissions.Any(x=> x.IdPermission == permission))
            {
                hasAllPermissions = false; 
                break;
            }
        }

        return hasAllPermissions;
    }

    public bool HasOnePermission(List<Permissions> permissions)
    {
        bool hasOnePermission = false;

        foreach (int permission in permissions)
        {
            if (!userSession.User.ProjectPermissions.Any(x => x.IdPermission == permission))
            {
                hasOnePermission = true;
                break;
            }
        }

        return hasOnePermission;
    }

    public bool HasOneOfRequiredPermissionsOrAllOptional(List<Permissions> required, List<Permissions> optional)
    {
        bool hasOneOfRequiredPermissionsOrAllOptional = false;

        foreach (int permission in required)
        {
            if (!userSession.User.ProjectPermissions.Any(x => x.IdPermission == permission))
            {
                hasOneOfRequiredPermissionsOrAllOptional = true;
                break;
            }
        }

        if (hasOneOfRequiredPermissionsOrAllOptional)
        {
            return true;
        } else
        {

            foreach (int permission in optional)
            {
                if (!userSession.User.ProjectPermissions.Any(x => x.IdPermission == permission))
                {
                    hasOneOfRequiredPermissionsOrAllOptional = false;
                    break;
                }
            }

            return hasOneOfRequiredPermissionsOrAllOptional;
        }
    }
}
