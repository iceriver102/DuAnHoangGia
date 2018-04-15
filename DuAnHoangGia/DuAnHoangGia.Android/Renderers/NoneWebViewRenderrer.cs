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
using DuAnHoangGia.Droid.Renderers;
using Xamarin.Forms;
using DuAnHoangGia.Views.Customs;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NoneWebView), typeof(NoneWebViewRenderrer))]
namespace DuAnHoangGia.Droid.Renderers
{
   public class NoneWebViewRenderrer:WebViewRenderer
         
    {
        public NoneWebViewRenderrer(Context ctx) : base(ctx)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
                return;
            Control.Background = null;
            Control.SetBackgroundColor(Color.Transparent.ToAndroid());

        }
    }
}