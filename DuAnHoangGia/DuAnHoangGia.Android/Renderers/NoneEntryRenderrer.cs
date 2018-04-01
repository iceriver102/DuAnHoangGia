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
using DuAnHoangGia.Views.Customs;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(NoneEntry), typeof(NoneEntryRenderrer))]
namespace DuAnHoangGia.Droid.Renderers
{
   public class NoneEntryRenderrer: EntryRenderer
    {
        public NoneEntryRenderrer(Context c) : base(c)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
                return;
            Control.Background = null;
            
        }
    }
}