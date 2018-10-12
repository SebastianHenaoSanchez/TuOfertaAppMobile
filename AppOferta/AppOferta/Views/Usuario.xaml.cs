using AppOferta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
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

            var map = new Map(
            MapSpan.FromCenterAndRadius(
            new Position(6.2215477, -75.5722723), Distance.FromMiles(3)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            var titulo = new Label
            {
                Text = "Bienvenido " +persona.nombre,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontSize = 18,
                VerticalOptions = LayoutOptions.Start
            };

            var boton = new Button
            {
                Text = "logout"
              
            };

          

            boton.Clicked += salir;
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(titulo);
            stack.Children.Add(map);
            stack.Children.Add(boton);
            Content = stack;

       
            
            //nombre.Text = nombre.Text + "HOLI - " + persona.nombre;
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