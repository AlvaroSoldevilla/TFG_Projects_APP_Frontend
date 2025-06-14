﻿using System.Globalization;
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
            string savedLanguage = Preferences.Get("AppLanguage", "en");
            var culture = new CultureInfo(savedLanguage);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            var window = new Window(new AppShell());
            window.MinimumWidth = 750;
            window.MinimumHeight = 800;
            return window;
        }
    }
}