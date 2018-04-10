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
[assembly: ExportRenderer(typeof(NoneEditor), typeof(NoneEditorRenderer))]
namespace DuAnHoangGia.Droid.Renderers
{
    public class NoneEditorRenderer : EditorRenderer
    {
        public NoneEditorRenderer(Context ctx) : base(ctx) { }
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
                return;
            if (Element is NoneEditor uiE)
            {
                Control.Background = null;
                Control.Hint = uiE.Hint;
                Control.SetHintTextColor(uiE.HintColor.ToAndroid());
            }
        }
    }
}