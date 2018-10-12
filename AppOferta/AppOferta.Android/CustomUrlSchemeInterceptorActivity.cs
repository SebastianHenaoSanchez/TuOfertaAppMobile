using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AppOferta.Services;


namespace AppOferta.Droid
{
    [Activity(Label = "CustomUrlSchemeInterceptorActivity")]
    [
IntentFilter
(
    actions: new[] { Intent.ActionView },
    Categories = new[]
    {
                Intent.CategoryDefault,
                Intent.CategoryBrowsable
    },
    DataSchemes = new[]
    {
            // First part of the redirect url (Package name)
            "com.googleusercontent.apps.1008390643140-jqh236vh7vte6ek6iuhpjr2rtj63jfrc"
    },
    DataPaths = new[]
    {
            // Second part of the redirect url (Path)
            ":/oauth2redirect"
    }
)
]
    class CustomUrlSchemeInterceptorActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Android.Net.Uri uri_android = Intent.Data;

            // Convert Android Url to C#/netxf/BCL System.Uri
            Uri uri_netfx = new Uri(uri_android.ToString());

            AuthenticatorState.Authenticator.OnPageLoading(uri_netfx);

            var intent = new Intent(this, typeof(MainActivity));
            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            StartActivity(intent);
            Finish();
        }
    }

}