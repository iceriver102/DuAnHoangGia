using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using DuAnHoangGia.Droid.Dependencys;
using DuAnHoangGia.Sevices;
using Xamarin.Forms;

[assembly: Dependency(typeof(PermissionImplementation))]
namespace DuAnHoangGia.Droid.Dependencys
{
    public class PermissionImplementation : Java.Lang.Object, IPermission
    {
        public bool CheckSelfPermission(Permission permission)
        {
            string namePermission = GetManifestNames(permission)[0];
            var targetsMOrHigher = MainActivity.Instance.ApplicationInfo.TargetSdkVersion >= Android.OS.BuildVersionCodes.M;
            if(targetsMOrHigher)
            return ContextCompat.CheckSelfPermission(MainActivity.Instance,namePermission) == Android.Content.PM.Permission.Granted;
            return PermissionChecker.CheckSelfPermission(MainActivity.Instance, namePermission) == (int)Android.Content.PM.Permission.Granted;
        }

        public void RequestPermission(params Permission[] permissions)
        {
            List<string> namePermissions = new List<string>();
            foreach (var n in permissions)
            {
                namePermissions.AddRange(GetManifestNames(n));
            }
            if (ActivityCompat.ShouldShowRequestPermissionRationale(MainActivity.Instance, Manifest.Permission.Camera))
            {
                Snackbar.Make(null,"demo",
                    Snackbar.LengthIndefinite).SetAction("OK", new Action<Android.Views.View>(delegate (Android.Views.View obj) {
                        ActivityCompat.RequestPermissions(MainActivity.Instance, namePermissions.ToArray(), 25);
                    })).Show();
            }
            else
                
                ActivityCompat.RequestPermissions(MainActivity.Instance, namePermissions.ToArray(), 25);
        }

        List<string> GetManifestNames(Permission permission)
        {
            var permissionNames = new List<string>();
            switch (permission)
            {
                case Permission.Calendar:
                    {
                        permissionNames.Add(Manifest.Permission.ReadCalendar);
                        permissionNames.Add(Manifest.Permission.WriteCalendar);

                    }
                    break;
                case Permission.Camera:
                    {
                        permissionNames.Add(Manifest.Permission.Camera);

                    }
                    break;
                case Permission.Contacts:
                    {

                        permissionNames.Add(Manifest.Permission.WriteContacts);
                        permissionNames.Add(Manifest.Permission.ReadContacts);
                        permissionNames.Add(Manifest.Permission.GetAccounts);
                    }
                    break;
                case Permission.LocationAlways:
                case Permission.LocationWhenInUse:
                case Permission.Location:
                    {
                        permissionNames.Add(Manifest.Permission.AccessFineLocation);
                        permissionNames.Add(Manifest.Permission.AccessCoarseLocation);
                       
                    }
                    break;
                case Permission.Speech:
                case Permission.Microphone:
                    {
                        permissionNames.Add(Manifest.Permission.RecordAudio);


                    }
                    break;
                case Permission.Sensors:
                    {
                        permissionNames.Add(Manifest.Permission.BodySensors);

                    }
                    break;
                case Permission.Storage:
                    {
                        permissionNames.Add(Manifest.Permission.ReadExternalStorage);
                        permissionNames.Add(Manifest.Permission.WriteExternalStorage);
                    }
                    break;
                default:
                    return null;
            }

            return permissionNames;
        }

    }
}