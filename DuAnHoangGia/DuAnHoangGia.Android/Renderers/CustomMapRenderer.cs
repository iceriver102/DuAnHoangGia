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
                // Unsubscribe
            }

            if (e.NewElement != null)
            {
                MAP = (CustomMap)e.NewElement;
                MAP.PropertyChanged -= FormsMap_PropertyChanged;
                MAP.PropertyChanged += FormsMap_PropertyChanged;
                MAP.CPins.CollectionChanged -= CPins_CollectionChanged;
                MAP.CPins.CollectionChanged += CPins_CollectionChanged;
                Control.GetMapAsync(this);
                MAP.GetMapCenterLocation = null;
                MAP.GetMapCenterLocation = new Func<Position>(GetCenterMap);
                MAP.RenderEvent += FormsMap_RenderEvent;
            }
        }

        private void CPins_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.Map.Pins.Clear();
            if (MAP.CPins != null)
                foreach (Pin p in MAP.CPins)
                {
                    this.Map.Pins.Add(p);
                }
        }

        private void FormsMap_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
           
            switch (e.PropertyName)
            {
                case "Renderer":
                    this.Map.Pins.Clear();
                    if (MAP.CPins != null)
                        foreach (Pin p in MAP.CPins)
                        {
                            this.Map.Pins.Add(p);
                        }
                    break;
                case "VisibleRegion":
                    MAP.CenterPostion = this.GetCenterMap();
                    MAP.MoveTo(MAP.CenterPostion);
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("---------------" + e.PropertyName);
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
            polylineOptions.InvokeColor(0x6670d3f6);
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

        }

    }
}