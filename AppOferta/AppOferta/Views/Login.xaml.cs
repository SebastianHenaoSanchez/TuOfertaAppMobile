using AppOferta.Models;
using AppOferta.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppOferta.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{
        //login google
        Account account;
        AccountStore store;
        
         
        Persona person = new Persona();

        HttpClient cliente = new HttpClient();
        ListaPersonas personas = new ListaPersonas();
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
                personas = JsonConvert.DeserializeObject<ListaPersonas>(contenido);
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
                Application.Current.Properties["persona"] = personas.persona[0];
                App.Current.MainPage = new Usuario();
                await Navigation.PushAsync(new Usuario());
                
                DisplayAlert("Login Correcto", personas.persona[0].nombre + " se ha logueado con exito", "OK");

                if (personas.persona[0].rol == "usuario")
                {
                    App.Current.MainPage = new Usuario();
                    await Navigation.PushAsync(new Usuario());
                }
                else if (personas.persona[0].rol == "administrador")
                {
                    App.Current.MainPage = new MainPage();
                    await Navigation.PushAsync(new MainPage());
                }
                else if (personas.persona[0].rol == "super administrador")
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

//login google
        public Login()
        {
            InitializeComponent();

            var google = botonGoogle;
            store = AccountStore.Create();
            account = store.FindAccountsForService("AppOferta").FirstOrDefault();

            google.GestureRecognizers.Add(new TapGestureRecognizer(ClickEnGoogle));
        }

        private void ClickEnGoogle(View arg1, object arg2)
        {
            var authenticator =
            new OAuth2Authenticator("1008390643140-jqh236vh7vte6ek6iuhpjr2rtj63jfrc.apps.googleusercontent.com",
                                    string.Empty,
                                    "https://www.googleapis.com/auth/userinfo.email",
                                    new Uri("https://accounts.google.com/o/oauth2/auth"),
                                    new Uri("com.googleusercontent.apps.1008390643140-jqh236vh7vte6ek6iuhpjr2rtj63jfrc:/oauth2redirect"),
                                    new Uri("https://www.googleapis.com/oauth2/v3/token"),
                                    null,
                                    true);

            authenticator.Completed += OnAuthenticationCompleted;
            authenticator.Error += OnAuthenticationFailed;

            AuthenticatorState.Authenticator = authenticator;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
    }

        private void OnAuthenticationFailed(object sender, AuthenticatorErrorEventArgs e)
        {
            DisplayAlert("error", "lgo no funcionó, vuelve a intentar", "Accept");
        }

        private async void OnAuthenticationCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            UserGoogle user;
            if (e.IsAuthenticated)
            {
                // The user is authenticated
                // Extract the OAuth token
                var request = new OAuth2Request("GET", new Uri("https://www.googleapis.com/oauth2/v2/userinfo"), null, e.Account);
                var response = await request.GetResponseAsync();
                if (response != null)
                {
                    string userJson = await response.GetResponseTextAsync();
                    user = JsonConvert.DeserializeObject<UserGoogle>(userJson);


                    // desde aqui pregunto si ya esta registrado
                    cliente.DefaultRequestHeaders.Add("Accept", "application/json");
                    string url = "http://192.168.6.46:8090/personas/login?correo=" + user.Email + "&contrasena=" + user.Id;
                    var uri = new Uri(url);

                    var respuesta = await cliente.GetAsync(uri);

                    //se captura la respuesta devuelta por el microservicio

                    if (respuesta.IsSuccessStatusCode)
                    {
                        var contenido = await respuesta.Content.ReadAsStringAsync();
                        personas = JsonConvert.DeserializeObject<ListaPersonas>(contenido);
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
                        Application.Current.Properties["persona"] = personas.persona[0];
                        App.Current.MainPage = new Usuario();
                        await Navigation.PushAsync(new Usuario());

                        DisplayAlert("Login Correcto", personas.persona[0].nombre + " se ha logueado con exito", "OK");

                        App.Current.MainPage = new Usuario();
                        await Navigation.PushAsync(new Usuario());
                      
                  

                    }
                    else
                    {
                        Registro(user);
                    }
                   //holiiiiiiiiiii

                   // DisplayAlert("persona Logueada", user.Email, "OK");
                    
                   
                }

                if (account != null)
                {
                    store.Delete(account, "AppOferta");
                }
                // Do something
            }
            else
            {
                //the user is not authenticated
            }
        }
        

        private async void Registro(UserGoogle usuario)
        {
            Console.WriteLine("ENTRO AL REGISTRO DESPUES DEL LOGIN GOOGLE");
            //se capturan los datos de google
            person.nombre = usuario.Name;
            person.apellidos = "na";
            person.correo = usuario.Email;
            person.ciudad = "na";
            person.telefono = "na";
            person.genero = "na";
            person.rol = "usuario";
            person.estado = "activo";
            person.token = "1234";
            person.contrasena = usuario.Id;

            personas.persona = new List<Persona>();
            personas.persona.Add(person);
            //convertimos el string a json se y crean los headers y la url para el request
            var jsonRequest = JsonConvert.SerializeObject(personas);
            var body = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            
            //se llama el microservicio para registrar
            cliente.DefaultRequestHeaders.Add("Accept", "application/json");
            string url = "http://192.168.6.46:8090/personas/registrar";
            var uri = new Uri(url);

            var response = await cliente.PostAsync(uri, body);
            Console.WriteLine("VA A CAPTURAR LA REPUESTA DE MICROSERVICIO");

            //se captura la respuesta devuelta por el microservicio
            if (response.IsSuccessStatusCode)
            {

                Console.WriteLine("ENTRO AL SUCCESS");
                DisplayAlert("Registro", "Persona correctamente", "OK");


            


                string url2 = "http://192.168.6.46:8090/personas/listar/correo/"+usuario.Email+"/";

                var uri2 = new Uri(url2);

                var respuesta1 = await cliente.GetAsync(uri2);

                if (respuesta1.IsSuccessStatusCode)
                {
                    var contenido = await respuesta1.Content.ReadAsStringAsync();
                    personas = JsonConvert.DeserializeObject<ListaPersonas>(contenido);

                    try
                    {
                        await SecureStorage.SetAsync("persona", contenido);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("entro al catch");
                        // Possible that device doesn't support secure storage on device.
                    }

                    Application.Current.Properties["persona"] = personas.persona[0];

                    App.Current.MainPage = new Usuario();
                    await Navigation.PushAsync(new Usuario());
                }
                else
                {
                    DisplayAlert("error", "no encontro el correo", "OK");
                }
                
                

              
              
                
            }
            else
            {
                Console.WriteLine("ENTRO AL ERROR");
                var contenido = await response.Content.ReadAsStringAsync();
                error = JsonConvert.DeserializeObject<Error>(contenido);

                DisplayAlert("error", error.detalle, "Accept");
                this.entryContrasena.Text = string.Empty;
            }
            
        }

        //Buscar una persona
        private async void obtener()
        {
           
        }




        //metodo para pasar de una pagina a otra
        private async void registrar(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Registro());
        }
    }
}