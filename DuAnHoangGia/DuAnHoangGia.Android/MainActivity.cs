using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin;
using FFImageLoading.Forms.Droid;

namespace DuAnHoangGia.Droid
{
    [Activity(Label = "DuAnHoangGia", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            this.Window.AddFlags(WindowManagerFlags.Fullscreen);
            this.Window.AddFlags(WindowManagerFlags.KeepScreenOn);

            Xamarin.Forms.Forms.SetFlags("FastRenderers_Experimental");
            CachedImageRenderer.Init(true);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            FormsMaps.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer()));
        }
    }
}

