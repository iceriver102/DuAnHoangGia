using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DuAnHoangGia.Droid.Dependencys;
using DuAnHoangGia.Sevices;
using Xamarin.Forms;

[assembly: Dependency(typeof(NetworkImplementation))]
namespace DuAnHoangGia.Droid.Dependencys
{
    public class NetworkImplementation : Java.Lang.Object, INetwork
    {
        ConnectivityManager cm;
        public bool IsOnline()
        {
            if (cm == null)
            {
                cm = ConnectivityManager.FromContext(MainActivity.Instance);
            }
            NetworkInfo netInfo = cm.ActiveNetworkInfo;
            return netInfo != null && netInfo.IsConnectedOrConnecting;
        }
    }
}