using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Position = Xamarin.Forms.Maps.Position;
using Xamarin.Forms.Xaml;
using Plugin.ExternalMaps;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using GoogleApi.Entities.Interfaces;
using GoogleApi;
using GoogleApi.Entities.Common;
using GoogleApi.Entities.Maps.Directions.Request;

namespace DuAnHoangGia.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePageDetail : ContentPage
    {
        Map map;
        IGeolocator locator;
        public HomePageDetail()
        {
            InitializeComponent();
            locator = CrossGeolocator.Current;
            //this.map = this.FindByName<Map>("uiMap");
            Pin pin = new Pin()
            {
                Position = new Position(10.769610, 106.657793),
                Label = "Nha Thi dau",
                Type = PinType.SearchResult,
            };
            
            this.uiMap.Pins.Add(pin);

            this.JumpToCurrent();
            //this.map.MoveToRegion(new MapSpan());

        }

        public async void JumpToCurrent()
        {
            var p = await this.locator.GetPositionAsync(TimeSpan.FromSeconds(10));
            //this.uiMap.AddRoute(new Position(10.769610, 106.657793));
            this.uiMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(p.Latitude, p.Longitude), Distance.FromKilometers(2)));
            var request = new DirectionsRequest
            {
                Origin=new Location( p.Latitude , p.Longitude ),
                Destination = new Location(10.769610, 106.657793)
                
            };
            var respone = await GoogleApi.GoogleMaps.Directions.QueryAsync(request);
            var Route = respone.Routes.FirstOrDefault();
            if (Route != null)
            {
                foreach(Location l in Route.OverviewPath.Points)
                {
                    this.uiMap.AddRoute(new Position(l.Latitude, l.Longitude));
                }
            }
            this.uiMap.Render();

        }

    }
}