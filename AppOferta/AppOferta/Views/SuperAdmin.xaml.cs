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
	public partial class SuperAdmin : ContentPage
	{
        public SuperAdmin()
        {
            InitializeComponent();
            //para traer los datos de la persona logueada
            Persona persona = (Persona)Application.Current.Properties["persona"];
        }

        private async void salir(object sender, EventArgs e)
        {
            SecureStorage.Remove("persona");
            App.Current.MainPage = new Login();
            await Navigation.PushAsync(new Login());
        }
    }
}