using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Prism.Navigation;
using Xamarin.Forms.Maps;
using Position = Xamarin.Forms.Maps.Position;
namespace DuAnHoangGia.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        Address aAddress;
        private string _from, _to;
        private float _radius;
        public float Radius { get => this._radius; set => this.SetProperty(ref this._radius, value); }
        public string FromLabel { get => this._from; set => this.SetProperty(ref this._from, value); }
        private Position _follow, aPosition;
        public Position FollowPostion { get => _follow; set => this.SetProperty(ref this._follow, value); }
        private readonly IGeolocator locator;

        public MapViewModel(INavigationService navigationService) : base(navigationService)
        {
            (var lat, var log) = Sevices.Settings.Current.Position;
            if (lat >= 0 && log >= 0)
            {
                this.FollowPostion = new Position(lat, log);
            }
            FromLabel = "Vị trí hiện tại";
            locator = CrossGeolocator.Current;
        }

        public async void JumpToCurrent()
        {
            var p = await this.locator.GetPositionAsync(TimeSpan.FromSeconds(10));
            aPosition = new Position(p.Latitude, p.Longitude);
            this.Radius = 2;
            this.FollowPostion = aPosition;
            //this.uiMap.AddRoute(new Position(10.769610, 106.657793));

            aAddress = (await this.locator.GetAddressesForPositionAsync(p)).FirstOrDefault();
        }


        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            this.JumpToCurrent();
        }
    }
}
