using AppOferta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppOferta.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Usuario : ContentPage
	{
        public Usuario()
        {
            InitializeComponent();

            Persona persona = (Persona)Application.Current.Properties["persona"];
            nombre.Text = nombre.Text + "HOLI - " + persona.nombre;
        }

        private async void salir(object sender, EventArgs e)
        {
            SecureStorage.Remove("persona");
            App.Current.MainPage = new Login();
            await Navigation.PushAsync(new Login());
            // Navigation.PushAsync(new Login());

        }
    }
}