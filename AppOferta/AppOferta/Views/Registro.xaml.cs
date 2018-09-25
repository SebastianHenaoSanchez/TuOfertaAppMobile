using AppOferta.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppOferta.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Registro : ContentPage
	{
		public Registro ()
		{
			InitializeComponent ();
		}

        HttpClient cliente = new HttpClient();
        ListaPersonas persona = new ListaPersonas();

        Persona person = new Persona();

        private async void registrar(Object sender, EventArgs e)
        {

            //se capturan los datos ingresados
            person.nombre = entryNombre.Text;
            person.apellidos = entryApellidos.Text;
            person.correo = entryCorreo.Text;
            person.ciudad = entryCiudad.Text;
            person.telefono = entryTelefono.Text;
            person.genero = pickerGenero.SelectedItem.ToString();
            person.rol = pickerRol.SelectedItem.ToString();
            person.estado = "activo";
            person.token = "1234";
            person.contrasena = entryCiudad.Text;

            persona.persona = new List<Persona>();
            persona.persona.Add(person);
            //convertimos el string a json se y crean los headers y la url para el request
            var jsonRequest = JsonConvert.SerializeObject(persona);
            var body = new StringContent(jsonRequest, Encoding.UTF8, "application/json");


            cliente.DefaultRequestHeaders.Add("Accept", "application/json");
            string url = "http://192.168.6.46:8090/personas/registrar";
            var uri = new Uri(url);

            var respuesta = await cliente.PostAsync(uri, body);

            //se captura la respuesta devuelta por el microservicio
            if (respuesta.IsSuccessStatusCode)
            {


                DisplayAlert("Registro", "Persona correctamente", "OK");
                App.Current.MainPage = new Login();
                await Navigation.PushAsync(new Login());
            }
        }
    }
}