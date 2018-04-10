using DuAnHoangGia.Helppers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DuAnHoangGia.Views.Customs
{
    public class NoneEditor : Editor
    {
        public static readonly BindableProperty HintProperty =
           BindablePropertyEx.Create<NoneEditor, string>(w => w.Hint, string.Empty);

        public string Hint
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }

        public static readonly BindableProperty HintColorProperty =
               BindablePropertyEx.Create<NoneEditor, Color>(w => w.HintColor, Color.White);

        public Color HintColor
        {
            get { return (Color)GetValue(HintColorProperty); }
            set { SetValue(HintColorProperty, value); }
        }
    }
}
