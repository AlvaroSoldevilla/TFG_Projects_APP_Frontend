using TFG_Projects_APP_Frontend.Pages;
using TFG_Projects_APP_Frontend.Pages.Concepts;
using TFG_Projects_APP_Frontend.Pages.Tasks;

namespace TFG_Projects_APP_Frontend
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            InitializeRouting();
        }

        private static void InitializeRouting()
        {
            Routing.RegisterRoute("ProjectmanagementPage", typeof(ProjectManagementPage));
            Routing.RegisterRoute("TaskBoardPage", typeof(TaskBoardPage));
            Routing.RegisterRoute("TaskProgressPage", typeof(TaskProgressPage));
            Routing.RegisterRoute("ConceptBoardPage", typeof(ConceptBoardPage));
        }
    }
}
