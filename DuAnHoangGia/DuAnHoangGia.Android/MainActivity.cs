using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin;
using FFImageLoading.Forms.Droid;
using Android.Content;
using FFImageLoading;
using Plugin.Permissions;
using Plugin.CurrentActivity;

namespace DuAnHoangGia.Droid
{
    [Activity(Label = "Hoàng Gia Map", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity Instance { get; private set; }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
           
            this.Window.AddFlags(WindowManagerFlags.Fullscreen);
            this.Window.AddFlags(WindowManagerFlags.KeepScreenOn);

           // Xamarin.Forms.Forms.SetFlags("FastRenderers_Experimental");
            CachedImageRenderer.Init(true);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            FormsMaps.Init(this, bundle);
            Instance = this;
            CrossCurrentActivity.Current.Activity = this;
            // Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.in
            LoadApplication(new App(new AndroidInitializer()));
           
        }
        public override void OnTrimMemory([GeneratedEnum] TrimMemory level)
        {
            ImageService.Instance.InvalidateMemoryCache();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            base.OnTrimMemory(level);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        //protected override void JavaFinalize()
        //{
        //    SetImageDrawable(null);
        //    SetImageBitmap(null);
        //    ImageService.Instance.InvalidateCacheEntryAsync(this.DataLocationUri, FFImageLoading.Cache.CacheType.Memory);
        //    base.JavaFinalize();
        //}
    }
}

