﻿using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace AppOferta.Droid
{
    [Activity(Label = "AppOferta", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            //para login de google
            global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, savedInstanceState);
            // Para el mapa
            Xamarin.FormsMaps.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
    }
}