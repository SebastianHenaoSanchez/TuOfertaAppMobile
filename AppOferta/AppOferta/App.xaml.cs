using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppOferta.Views;
using Xamarin.Essentials;
using Newtonsoft.Json;
using AppOferta.Models;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AppOferta
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Login());
        }

        protected async override void OnStart()
        {
            try
            {
                var persona = await SecureStorage.GetAsync("persona");
                if (persona != "" && persona != null)
                {
                    var person = JsonConvert.DeserializeObject<ListaPersonas>(persona);
                    Application.Current.Properties["persona"] = person.persona[0];

                    if (person.persona[0].rol == "usuario")
                    {
                        MainPage = new NavigationPage(new Usuario());
                    }
                    else if (person.persona[0].rol == "administrador")
                    {
                        MainPage = new MainPage();
                    }
                    else if (person.persona[0].rol == "super administrador")
                    {
                        MainPage = new NavigationPage(new SuperAdmin());
                    }
                }
                else
                {
                    MainPage = new NavigationPage(new Login());
                }

            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
