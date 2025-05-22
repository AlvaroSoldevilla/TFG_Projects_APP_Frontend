using TFG_Projects_APP_Frontend.Pages;

namespace TFG_Projects_APP_Frontend
{
    public partial class App : Application
    {
        public App(LoginPage loginPage)
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}