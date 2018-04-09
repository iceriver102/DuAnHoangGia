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
using System.Diagnostics;

namespace DuAnHoangGia.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePageDetail : ContentPage
    {
        IGeolocator locator;
        Position aPosition;
        Address aAddress;
        //Customs.NoneEntry uiFrom,uiTo;
        public HomePageDetail()
        {
            InitializeComponent();
            locator = CrossGeolocator.Current;
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
            aPosition = new Position(p.Latitude, p.Longitude);
            //this.uiMap.AddRoute(new Position(10.769610, 106.657793));
            this.uiMap.MoveToRegion(MapSpan.FromCenterAndRadius(aPosition, Distance.FromKilometers(2)));
            this.uiFrom.Text = "Vị trí hiện tại";
            aAddress = (await this.locator.GetAddressesForPositionAsync(p)).FirstOrDefault();


            //var request = new DirectionsRequest
            //{
            //    Origin=new Location( p.Latitude , p.Longitude ),
            //    Destination = new Location(10.769610, 106.657793)

            //};
            //var respone = await GoogleApi.GoogleMaps.Directions.QueryAsync(request);
            //var Route = respone.Routes.FirstOrDefault();
            //if (Route != null)
            //{
            //    foreach(Location l in Route.OverviewPath.Points)
            //    {
            //        this.uiMap.AddRoute(new Position(l.Latitude, l.Longitude));
            //    }
            //}
            //this.uiMap.Render();

        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Route();

        }

        public async void Route()
        {

            //var bAddress = await locator.GetPositionsForAddressAsync("hoàng văn thụ");
            var bRequestAddress = new GoogleApi.Entities.Maps.Geocode.Request.GeocodingRequest() { Address = "Hoàng văn thụ hồ chí minh" , Language = GoogleApi.Entities.Common.Enums.Language.Vietnamese };
           

           var bRespone= await  GoogleApi.GoogleMaps.Geocode.QueryAsync(bRequestAddress);
            var bAddress = bRespone.Results.FirstOrDefault();
            if (bAddress != null)
            {
                Pin pin = new Pin()
                {
                    Position = new Position(bAddress.Geometry.Location.Latitude,bAddress.Geometry.Location.Longitude),
                    Label =bAddress.FormattedAddress,
                    Type = PinType.SearchResult,
                };
                this.uiMap.Pins.Add(pin);
                // var b = bAddress.FirstOrDefault();
                var request = new DirectionsRequest
                {
                    Origin = new Location(aPosition.Latitude, aPosition.Longitude),
                    Destination = bAddress.Geometry.Location,

                };

                var respone = await GoogleApi.GoogleMaps.Directions.QueryAsync(request);

                var Route = respone.Routes.FirstOrDefault();
                if (Route != null)
                {
                    Location m = Route.OverviewPath.Points.LastOrDefault();
                    Position center=new Position((m.Latitude + aPosition.Latitude)/2, (m.Longitude+aPosition.Longitude)/2);
                    foreach (Location l in Route.OverviewPath.Points)
                    {
                        this.uiMap.AddRoute(new Position(l.Latitude, l.Longitude));
                    }
                    this.uiMap.MoveToRegion(MapSpan.FromCenterAndRadius(center, Distance.FromMeters(Route.Legs.FirstOrDefault().Distance.Value/2)));
                }
               
                this.uiMap.Render();
            }
        }
    }
}