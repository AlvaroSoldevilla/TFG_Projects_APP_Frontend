using TFG_Projects_APP_Frontend.Pages;

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
        }
    }
}
