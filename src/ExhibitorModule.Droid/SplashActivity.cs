using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using ExhibitorModule.Helpers;

namespace ExhibitorModule.Droid
{
    [Activity(Label = "@string/ApplicationName",
              Icon = "@mipmap/ic_launcher",
              Theme = "@style/SplashTheme",
              MainLauncher = true,
              ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var intent = new Intent(Application.Context, typeof(MainActivity));
            if (Intent.Extras != null)
                intent.PutExtras(Intent.Extras);
            StartActivity(intent);
            Finish();
        }
    }
}
