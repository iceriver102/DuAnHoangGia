using DuAnHoangGia.Sevices;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DuAnHoangGia.ViewModels
{
    public class LoaddingViewModel : ViewModelBase
    {
        IGeolocator locator;
        private readonly IHttpSevices HTTP;
        public LoaddingViewModel(INavigationService navigationService, IHttpSevices _http) : base(navigationService) {

            this.HTTP = _http;
            locator = CrossGeolocator.Current;
        }

        public async override void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);
            var p=  await this.locator.GetPositionAsync(TimeSpan.FromSeconds(10));
            Settings.Current.Position = (lat: p.Latitude, log: p.Longitude);
        }

       

    }
}
