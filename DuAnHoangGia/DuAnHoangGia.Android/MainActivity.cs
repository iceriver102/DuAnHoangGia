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
using Android.Graphics;

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
            AndroidBug5497WorkaroundForXamarinAndroid.assistActivity(this);
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
    public class AndroidBug5497WorkaroundForXamarinAndroid
    {

        // For more information, see https://code.google.com/p/android/issues/detail?id=5497
        // To use this class, simply invoke assistActivity() on an Activity that already has its content view set.

        // CREDIT TO Joseph Johnson (http://stackoverflow.com/users/341631/joseph-johnson) for publishing the original Android solution on stackoverflow.com

        public static void assistActivity(Activity activity)
        {
            new AndroidBug5497WorkaroundForXamarinAndroid(activity);
        }

        private Android.Views.View mChildOfContent;
        private int usableHeightPrevious;
        private FrameLayout.LayoutParams frameLayoutParams;

        private AndroidBug5497WorkaroundForXamarinAndroid(Activity activity)
        {
            FrameLayout content = (FrameLayout)activity.FindViewById(Android.Resource.Id.Content);
            mChildOfContent = content.GetChildAt(0);
            ViewTreeObserver vto = mChildOfContent.ViewTreeObserver;
            vto.GlobalLayout += (object sender, EventArgs e) => {
                possiblyResizeChildOfContent();
            };
            frameLayoutParams = (FrameLayout.LayoutParams)mChildOfContent.LayoutParameters;
        }

        private void possiblyResizeChildOfContent()
        {
            int usableHeightNow = computeUsableHeight();
            if (usableHeightNow != usableHeightPrevious)
            {
                int usableHeightSansKeyboard = mChildOfContent.RootView.Height;
                int heightDifference = usableHeightSansKeyboard - usableHeightNow;

                frameLayoutParams.Height = usableHeightSansKeyboard - heightDifference;

                mChildOfContent.RequestLayout();
                usableHeightPrevious = usableHeightNow;
            }
        }

        private int computeUsableHeight()
        {
            Rect r = new Rect();
            mChildOfContent.GetWindowVisibleDisplayFrame(r);
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
            {
                return (r.Bottom - r.Top);
            }
            return r.Bottom;
        }
    }
}

