using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DuAnHoangGia.Droid.Renderers;
using DuAnHoangGia.Views.Customs;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace DuAnHoangGia.Droid.Renderers
{
    public class CustomMapRenderer : MapRenderer
    {
        Polyline line;
        private CustomMap MAP;
        private GoogleMap nativeMap;
        public CustomMapRenderer(Context context) : base(context)
        {
            line = null;
        }
        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var marker = new MarkerOptions();
            //if (pin.BindingContext is Models.CompanyModel com)
            //    marker.InvokeZIndex(com.id);
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pin.Label);
            marker.SetSnippet(pin.Address);
            marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.marker));
            return marker;
        }


        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                if (e.OldElement != null)
                {
                    nativeMap.MarkerClick -= NativeMap_MarkerClick;
                    MAP.PropertyChanged -= FormsMap_PropertyChanged;
                }
            }

            if (e.NewElement != null)
            {
                MAP = (CustomMap)e.NewElement;               
                MAP.PropertyChanged += FormsMap_PropertyChanged;
                Control.GetMapAsync(this);
                MAP.MapRouteRender = null;
                MAP.MapRouteRender = new Func<Color,bool>(MapRouteRender);
                MAP.CleanRoute = null;
                MAP.CleanRoute = new Func<bool>(() =>
                {
                    if (line != null)
                    {
                        line.Remove();
                    }
                    return true;
                });
            }
        }

       

        private bool MapRouteRender(Color colorPath)
        {
            if (line != null)
            {
                line.Remove();
            }
            if (MAP.RouteCoordinates == null && MAP.RouteCoordinates.Count==0)
                return true;
            var polylineOptions = new PolylineOptions();
            polylineOptions.InvokeColor(colorPath.ToAndroid());
            //polylineOptions.InvokeColor(0x6670d3f6);
            polylineOptions.InvokeWidth(polylineOptions.Width + 5);

            foreach (var position in MAP.RouteCoordinates)
            {
                polylineOptions.Add(new LatLng(position.Latitude, position.Longitude));
            }
            line = NativeMap.AddPolyline(polylineOptions);
            return true;
        }
        

        private void FormsMap_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
           
            switch (e.PropertyName)
            {
                case "Renderer":
                    //this.Map.Pins.Clear();
                    //if (MAP.CPins != null)
                    //    foreach (Pin p in MAP.CPins)
                    //    {
                    //        this.Map.Pins.Add(p);
                    //    }
                    break;
                case "VisibleRegion":
                    MAP.CenterPostion = this.GetCenterMap();
                    MAP.MoveTo(MAP.CenterPostion);
                    break;
              
            }
        }


        public Position GetCenterMap()
        {
            if (nativeMap == null)
                return new Position(0, 0);
            var centerPosition = nativeMap.CameraPosition.Target;
            return new Position(centerPosition.Latitude, centerPosition.Longitude);
        }

        private void FormsMap_RenderEvent(object sender, ICollection<Position> e)
        {
            if (line != null)
            {
                line.Remove();
            }
            var polylineOptions = new PolylineOptions();
            polylineOptions.InvokeColor(0xffba00);
            polylineOptions.InvokeWidth(polylineOptions.Width + 4);


            foreach (var position in e)
            {
                polylineOptions.Add(new LatLng(position.Latitude, position.Longitude));
            }
            line = NativeMap.AddPolyline(polylineOptions);
        }

        protected override void OnMapReady(Android.Gms.Maps.GoogleMap map)
        {
            base.OnMapReady(map);
            nativeMap = map;
            nativeMap.MarkerClick += NativeMap_MarkerClick;

        }
        private Marker m;
        private void NativeMap_MarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            e.Handled = false;
            if (m==null||m.Id != e.Marker.Id)
            {
                m = e.Marker;
                return;
            }
            Pin p = GetCustomPin(e.Marker);
            if (p!=null)
            {
                MAP.RouteTo(null,p);
            }
        }
        Pin GetCustomPin(Marker annotation)
        {
            var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
            foreach (var pin in MAP.CPins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }
    }
}