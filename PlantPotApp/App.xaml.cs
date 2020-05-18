using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PlantPotApp.Services;
using PlantPotApp.Views;
using System.Diagnostics;

namespace PlantPotApp
{
    public partial class App : Application
    {
        public BLEConnection server;
        public App()
        {
            InitializeComponent();
            MainPage = new MainPage(server);
        }

        protected override void OnStart()
        {
            Debug.WriteLine("On start called");
            server = new BLEConnection();
            server.connect();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
