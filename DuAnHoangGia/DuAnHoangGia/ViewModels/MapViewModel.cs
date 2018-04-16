using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DuAnHoangGia.Models;
using DuAnHoangGia.Sevices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms.Maps;
using Position = Xamarin.Forms.Maps.Position;
namespace DuAnHoangGia.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        public ObservableCollection<Pin> PINS { get; set; }
        Address aAddress;
        private string _from, _to;
        private float _radius;
        public float Radius { get => this._radius; set => this.SetProperty(ref this._radius, value); }
        public string FromLabel { get => this._from; set => this.SetProperty(ref this._from, value); }
        private Position _follow, aPosition, _centerPostion;
        public Position FollowPostion { get => _follow; set => this.SetProperty(ref this._follow, value); }
        public Position CenterPostion { get => _centerPostion; set => this.SetProperty(ref this._centerPostion, value); }
        private readonly IGeolocator locator;
        private readonly IEventAggregator eventAggregator;
        private readonly IHttpSevices HTTP;
        private List<CompanyModel> caches;
        public DelegateCommand LoadMoreCommand { get; set; }

        public MapViewModel(INavigationService navigationService, IHttpSevices _http, IPageDialogService _pageDialogService, IEventAggregator eventAggregator) : base(navigationService)
        {
            caches = new List<CompanyModel>();
            HTTP = _http;
            this.eventAggregator = eventAggregator;
            this.PINS = new ObservableCollection<Pin>();
            (var lat, var log) = Sevices.Settings.Current.Position;
            if (lat >= 0 && log >= 0)
            {
                this.FollowPostion = new Position(lat, log);
            }
            FromLabel = "Vị trí hiện tại";
            locator = CrossGeolocator.Current;
            this.ShowMenuCommand = new DelegateCommand(ShowMenuCommandExcute);
            this.JumpToCurrent();
            LoadMoreCommand = new DelegateCommand(LoadMoreCommandExcute);
            this.LoadData(this.FollowPostion);
        }

        private void LoadMoreCommandExcute()
        {
            this.LoadData(this.CenterPostion);
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

        public async void LoadData(Position p)
        {
            var oResult = await this.HTTP.GetCompanysOnMapAsync(p.Latitude, p.Longitude);
            if (oResult["data"].HasValues && oResult["data"] is JArray datas)
            {
                List<CompanyModel> comps = JsonConvert.DeserializeObject<List<CompanyModel>>(datas.ToString());
                Debug.WriteLine("---::: "+comps.Count);
                if (comps != null)
                    foreach (var c in comps)
                    {
                        if (caches.Any(w => w.id == c.id))
                            continue;
                        caches.Add(c);
                        Pin pin = new Pin();
                        pin.Label = c.Title;                        
                        pin.Position = new Position(c.Latitude, c.Longitude);
                        this.PINS.Add(pin);
                    }
            }
        }


      

        public DelegateCommand ShowMenuCommand { get; private set; }
        private void ShowMenuCommandExcute()
        {
            this.eventAggregator.GetEvent<Events.MenuEvent>().Publish(new Events.MenuMessage() { IsPresented = true });
        }

    }
}
