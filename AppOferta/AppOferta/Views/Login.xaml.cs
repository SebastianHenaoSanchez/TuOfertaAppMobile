using AppOferta.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppOferta.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{
        HttpClient cliente = new HttpClient();
        ListaPersonas persona = new ListaPersonas();
        Error error = new Error();
        
        private async void login(Object sender, EventArgs e)
        {
            //se capturan los datos ingresados
            string email = entryCorreo.Text;
            string contrasena = entryContrasena.Text;
            if (string.IsNullOrEmpty(email))
            {
                await Application.Current.MainPage.DisplayAlert("error", "you must enter an email", "Accept");
               
                return;
            }

            if (string.IsNullOrEmpty(contrasena))
            {
                await Application.Current.MainPage.DisplayAlert("error", "you must enter a password", "Accept");
                return;
            }
            //se crean los headers y la url para el request
            cliente.DefaultRequestHeaders.Add("Accept", "application/json");
            string url = "http://192.168.6.46:8090/personas/login?correo=" + email + "&contrasena=" + contrasena;
            var uri = new Uri(url);

            var respuesta = await cliente.GetAsync(uri);

            //se captura la respuesta devuelta por el microservicio
      
            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                persona = JsonConvert.DeserializeObject<ListaPersonas>(contenido);
                //   DisplayAlert("persona Logueada", persona.persona[0].ToString(), "OK");
                try
                {
                    await SecureStorage.SetAsync("persona", contenido);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("entro al catch");
                    // Possible that device doesn't support secure storage on device.
                }
                //

                Console.WriteLine("va por aqui");
                Application.Current.Properties["persona"] = persona.persona[0];
                App.Current.MainPage = new Usuario();
                await Navigation.PushAsync(new Usuario());

                DisplayAlert("Login Correcto", persona.persona[0].nombre + " se ha logueado con exito", "OK");

                if (persona.persona[0].rol == "usuario")
                {
                    App.Current.MainPage = new Usuario();
                    await Navigation.PushAsync(new Usuario());
                }
                else if (persona.persona[0].rol == "administrador")
                {
                    App.Current.MainPage = new MainPage();
                    await Navigation.PushAsync(new MainPage());
                }
                else if (persona.persona[0].rol == "super administrador")
                {
                    App.Current.MainPage = new SuperAdmin();
                    await Navigation.PushAsync(new SuperAdmin());
                }

            }
            else
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                error = JsonConvert.DeserializeObject<Error>(contenido);
                
                DisplayAlert("error", error.detalle, "Accept");
                this.entryContrasena.Text = string.Empty;
            }
        }

        public Login()
        {
            InitializeComponent();
        }

        //metodo para pasar de una pagina a otra
        private async void registrar(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Registro());
        }
    }
}