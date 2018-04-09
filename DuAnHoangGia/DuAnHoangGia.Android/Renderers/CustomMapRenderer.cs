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
                var formsMap = (CustomMap)e.NewElement;

                Control.GetMapAsync(this);
                formsMap.RenderEvent += FormsMap_RenderEvent;
            }
        }

        private void FormsMap_RenderEvent(object sender, List<Position> e)
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

          
        }

    }
}