using CommunityToolkit.Mvvm.ComponentModel;
using TFG_Projects_APP_Frontend.Services.UsersService;

namespace TFG_Projects_APP_Frontend.PageModels;

public partial class LoginPageModel(UsersService usersService) : ObservableObject
{
    private readonly UsersService usersService = usersService;
}
