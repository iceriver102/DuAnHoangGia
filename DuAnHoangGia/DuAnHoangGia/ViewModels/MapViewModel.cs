﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DuAnHoangGia.Models;
using DuAnHoangGia.Sevices;
using GoogleApi.Entities.Maps.Directions.Request;
using GoogleApi.Entities.Common;
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
        public ObservableCollection<Position> RouteCoordinates { get; set; }
        private bool _isLoadding;
        private bool _rPin = false;
        public bool RenderPINTriger { get => _rPin; set => SetProperty(ref _rPin, value); }
        public bool isLoadding { get => this._isLoadding; set => this.SetProperty(ref this._isLoadding, value); }

        private string _from, _to;
        private float _radius;
        public float Radius { get => this._radius; set => this.SetProperty(ref this._radius, value); }
        public string FromLabel { get => this._from; set => this.SetProperty(ref this._from, value); }
        public string ToLabel { get => this._to; set => this.SetProperty(ref this._to, value); }
        private Position _follow, aPosition, _centerPostion, _userPostion;
        public Position FollowPostion { get => _follow; set => this.SetProperty(ref this._follow, value); }
        public Position CenterPostion { get => _centerPostion; set => this.SetProperty(ref this._centerPostion, value); }
        public Position UserPostion { get => _userPostion; set => this.SetProperty(ref this._userPostion, value); }
        private readonly IGeolocator locator;
        private readonly IEventAggregator eventAggregator;
        private readonly IPageDialogService IPageDialogService;
        private readonly IHttpSevices HTTP;
        private List<int> caches;
        public DelegateCommand LoadMoreCommand { get; set; }
        public DelegateCommand SearchCommand { get; set; }

        public MapViewModel(INavigationService navigationService, IHttpSevices _http, IPageDialogService _pageDialogService, IEventAggregator eventAggregator) : base(navigationService)
        {
            this.IPageDialogService = _pageDialogService;
            RouteCoordinates = new ObservableCollection<Position>();
            caches = new List<int>();
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
            LoadMoreCommand = new DelegateCommand(LoadMoreCommandExcute);
            this.SearchCommand = new DelegateCommand(GotoCommandExcute);
            this.JumpToCurrent();
            this.LoadData(this.FollowPostion);
        }

        private async void GotoCommandExcute()
        {
            RouteCoordinates.Clear();
            if (string.IsNullOrEmpty(this.ToLabel))
            {
                await this.IPageDialogService.DisplayAlertAsync("Thông báo", "Xin hãy nhập địa chỉ công ty cần tới", "OK");
                this.RenderTriger = true;
                RaisePropertyChanged("RenderTriger");
                return;
            }
            var oResult = await HTTP.GetCompanysByNameAsync(this.ToLabel);
            if (oResult.result && oResult.data != null)
            {
                this.ToLabel = oResult.data["address"].Value<string>();
                var request = new DirectionsRequest
                {
                    Origin = new Location(this.aPosition.Latitude, this.aPosition.Longitude),
                    Destination = new Location(oResult.data["latitude"].Value<double>(), oResult.data["longitude"].Value<double>())

                };
                var respone = await GoogleApi.GoogleMaps.Directions.QueryAsync(request);
                var Route = respone.Routes.FirstOrDefault();
                if (Route != null)
                {

                    Location m = Route.OverviewPath.Points.LastOrDefault();
                    Position center = new Position((m.Latitude + aPosition.Latitude) / 2, (m.Longitude + aPosition.Longitude) / 2);
                    foreach (Location l in Route.OverviewPath.Points)
                    {
                        RouteCoordinates.Add(new Position(l.Latitude, l.Longitude));
                        //this.uiMap.AddRoute(new Position(l.Latitude, l.Longitude));
                    }
                    this.Radius = Route.Legs.FirstOrDefault().Distance.Value / 2000f;
                    this.FollowPostion = center;
                    //this.RenderPINTriger = false;
                    //RaisePropertyChanged("RenderPINTriger");
                }
                this.RenderTriger = true;
                RaisePropertyChanged("RenderTriger");
            }
            else
            {
                await this.IPageDialogService.DisplayAlertAsync("Thông báo", "Không tìm thấy công ty", "OK");
            }

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
            this.UserPostion = aPosition;
            // aAddress = (await this.locator.GetAddressesForPositionAsync(p)).FirstOrDefault();
        }

        public async void LoadData(Position p)
        {
            var oResult = await this.HTTP.GetCompanysOnMapAsync(p.Latitude, p.Longitude);

            if (oResult.data == null)
            {
                this.isLoadding = false;
                return;
            }
            if (oResult.data.HasValues)
            {
                List<CompanyModel> comps = JsonConvert.DeserializeObject<List<CompanyModel>>(oResult.data.ToString());

                if (comps != null)
                    foreach (var c in comps)
                    {
                        if (caches.Any(w => w == c.id))
                            continue;
                        caches.Add(c.id);
                        Pin pin = new Pin();
                        pin.BindingContext = c;
                        pin.Clicked += Pin_Clicked;
                        pin.Address = c.Address;
                        pin.Label = c.Title;
                        pin.Position = new Position(c.Latitude, c.Longitude);
                        this.PINS.Add(pin);
                    }
            }
            RenderPINTriger = true;
            RaisePropertyChanged("RenderPINTriger");
            this.isLoadding = false;
        }

        private void Pin_Clicked(object sender, EventArgs e)
        {
            if (sender is Pin p)
            {
                if (p.BindingContext is CompanyModel com)
                {
                    this.Navigate($"ComDetail?comp={com.id}");
                }
            }
        }

        public DelegateCommand ShowMenuCommand { get; private set; }
        private bool _rendertriger = false;
        public bool RenderTriger { get => _rendertriger; set => this.SetProperty(ref _rendertriger, value); }

        private void ShowMenuCommandExcute()
        {
            this.eventAggregator.GetEvent<Events.MenuEvent>().Publish(new Events.MenuMessage() { IsPresented = true });
        }
    }
}
