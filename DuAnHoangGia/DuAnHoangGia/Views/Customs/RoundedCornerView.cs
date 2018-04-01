using DuAnHoangGia.Helppers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DuAnHoangGia.Views.Customs
{
    public class RoundedCornerView : Grid
    {
        public static readonly BindableProperty FillColorProperty =
            BindablePropertyEx.Create<RoundedCornerView, Color>(w => w.FillColor, Color.White);
        public Color FillColor
        {
            get { return (Color)GetValue(FillColorProperty); }
            set { SetValue(FillColorProperty, value); }
        }

        public static readonly BindableProperty RoundedCornerRadiusProperty =
        BindablePropertyEx.Create<RoundedCornerView, double>(w => w.RoundedCornerRadius, 3);
        //public static readonly BindableProperty RoundedCornerRadiusProperty =
        //  BindableProperty.Create(nameof(RoundedCornerRadius),
        //      typeof(double), typeof(RoundedCornerView), 3);
        public double RoundedCornerRadius
        {
            get { return (double)GetValue(RoundedCornerRadiusProperty); }
            set { SetValue(RoundedCornerRadiusProperty, value); }
        }

        public static readonly BindableProperty MakeCircleProperty =
            BindablePropertyEx.Create<RoundedCornerView, Boolean>(w => w.MakeCircle, false);
        public Boolean MakeCircle
        {
            get { return (Boolean)GetValue(MakeCircleProperty); }
            set { SetValue(MakeCircleProperty, value); }
        }

        public static readonly BindableProperty BorderColorProperty =
            BindablePropertyEx.Create<RoundedCornerView, Color>(w => w.BorderColor, Color.Transparent);
        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public static readonly BindableProperty BorderWidthProperty =
            BindablePropertyEx.Create<RoundedCornerView, int>(w => w.BorderWidth, 0);
        public int BorderWidth
        {
            get { return (int)GetValue(BorderWidthProperty); }
            set { SetValue(BorderWidthProperty, value); }
        }




    }
}
