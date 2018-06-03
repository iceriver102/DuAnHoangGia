using System;
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
using GoogleApi.Entities.Maps.Directions.Response;
using Plugin.Permissions;
using DuAnHoangGia.Events;
using DependencyService = Xamarin.Forms.DependencyService;
using System.Text.RegularExpressions;

namespace DuAnHoangGia.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        public ObservableCollection<Pin> PINS { get; set; }
        public ObservableCollection<Position> RouteCoordinates { get; set; }
        private bool _rPin = false, _isLoadding, _flagNoCom, _showVoiceCommand;
        public bool showVoiceCommand
        {
            get => this._showVoiceCommand;
            set => SetProperty(ref this._showVoiceCommand, value);
        }
        public bool RenderPINTriger { get => _rPin; set => SetProperty(ref _rPin, value); }
        public bool isLoadding { get => this._isLoadding; set => this.SetProperty(ref this._isLoadding, value); }
        public bool FlagNoCom { get => this._flagNoCom; set => this.SetProperty(ref _flagNoCom, value); }

        private string _from, _to, _instructions;
        public string Instructions { get => this._instructions;
            set {
                this.SetProperty(ref this._instructions, value);
                showVoiceCommand = !string.IsNullOrEmpty(value);
            }
        }
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
        public DelegateCommand LoadMoreCommand { get; private set; }
        public DelegateCommand SearchCommand { get; set; }
        public DelegateCommand LoadCompanyCommand { get; private set; }
        public DelegateCommand DissmissCommand { get; private set; }
        public PlaceViewModel Place { get; set; }
        public DelegateCommand VoiceIntrustion { get; set; }
        public MapViewModel(INavigationService navigationService, IHttpSevices _http, IPageDialogService _pageDialogService, IEventAggregator eventAggregator) : base(navigationService)
        {
            this.showVoiceCommand = false;
            this.IPageDialogService = _pageDialogService;
            this.Place = new PlaceViewModel(this.ToLabel,_http);
            this.Place.Goto = new DelegateCommand<CompanyModel>(GotoExcute);
            RouteCoordinates = new ObservableCollection<Position>();
            caches = new List<int>();
            HTTP = _http;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<Events.CompanyEvent>().Subscribe((c) =>
            {
                this.Route(c);
            });
            this.PINS = new ObservableCollection<Pin>();
            (var lat, var log) = Sevices.Settings.Current.Position;
            if (lat >= 0 && log >= 0)
            {
                this.FollowPostion = new Position(lat, log);
            }
            ToLabel = "Bạn muốn đến công ty nào";

            locator = CrossGeolocator.Current;
            this.ShowMenuCommand = new DelegateCommand(ShowMenuCommandExcute);
            LoadMoreCommand = new DelegateCommand(LoadMoreCommandExcute);
            this.SearchCommand = new DelegateCommand(()=> { GotoCommandExcute(this.ToLabel); });
            this.Place.SearchCommand = new DelegateCommand(()=> { GotoCommandExcute(this.Place.ToLabel); });
           
            LoadCompanyCommand = new DelegateCommand(()=> {
                Place.Models.Clear();
                Place.LoadPage(1);
                Place.IsVisible = true;
                //JObject oResult = await HTTP.GetCompanysAsync(p);
            });
            DissmissCommand = new DelegateCommand(() =>
            {
                this.FlagNoCom = false;
            });
            this.JumpToCurrent();
            VoiceIntrustion = new DelegateCommand(() =>
            {
                if (!string.IsNullOrEmpty(Instructions))
                {
                    DependencyService.Get<ITextToSpeech>().Speak(Instructions);
                }
            });
        }

        private void GotoExcute(CompanyModel obj)
        {
            if (obj == null)
            {
                this.Popup.Show("Lỗi", "Không tìm thấy thông tin công ty");
                return;
            }
            this.Route(obj);
        }

        public async void Route(Models.CompanyModel comp)
        {
            this.Instructions = string.Empty;
            RouteCoordinates.Clear();
            this.ToLabel = comp.Address;
            var request = new DirectionsRequest
            {
                Origin = new Location(this.aPosition.Latitude, this.aPosition.Longitude),
                Destination = new Location(comp.Latitude, comp.Longitude),
                Language = GoogleApi.Entities.Common.Enums.Language.Vietnamese,

            };
            Route route = null;
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                var respone = await GoogleApi.GoogleMaps.Directions.QueryAsync(request);
                route = respone.Routes.FirstOrDefault();
                foreach (var Leg in route.Legs)
                {
                    stringBuilder.Clear();
                    foreach (var s in Leg.Steps)
                    {
                        stringBuilder.Append(Regex.Replace(s.HtmlInstructions, @"<[^>]*>", " "));
                        stringBuilder.Append(". ");
                    }
                }
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex("[ ]{2,}", options);
                this.Instructions = regex.Replace(stringBuilder.ToString(), " ");
            }
            catch (Exception ex)
            {

            }
            if (route != null)
            {
                Location m = route.OverviewPath.Points.LastOrDefault();
                Position center = new Position((m.Latitude + aPosition.Latitude) / 2, (m.Longitude + aPosition.Longitude) / 2);
                foreach (Location l in route.OverviewPath.Points)
                {
                    RouteCoordinates.Add(new Position(l.Latitude, l.Longitude));
                }
                this.Radius = route.Legs.FirstOrDefault().Distance.Value / 2000f;
                this.FollowPostion = center;
            }
            Pin p = new Pin();
            p.Position = new Position(comp.Latitude, comp.Longitude);
            p.Label = comp.Title;
            p.Address = comp.Address;
            this.RenderTriger = p;
            RaisePropertyChanged("RenderTriger");
            this.RenderPINTriger = false;
        }

        public async void GotoCommandExcute(string gotoLabel)
        {
            RouteCoordinates.Clear();
            this.Place.Models.Clear();
            if (string.IsNullOrEmpty(gotoLabel))
            {
                this.Popup.Show("Lỗi", "Hãy nhập thông tin tìm kiếm", Xamarin.Forms.Color.Red);
                this.Place.IsVisible = false;
                return;
            }
            this.ToLabel = gotoLabel;
            this.Place.ToLabel = gotoLabel;
            var oResult = await HTTP.GetCompanysByNameAsync(gotoLabel, this.aPosition.Latitude, this.aPosition.Longitude);
            if (oResult.result && oResult.data != null)
            {
                List<CompanyModel> comps = JsonConvert.DeserializeObject<List<CompanyModel>>(oResult.data.ToString());
                this.Place.Models.AddRange(comps);
                this.Place.IsVisible = true;
            }
            else
            {
                FlagNoCom = true;

            }

        }

        private void LoadMoreCommandExcute()
        {
            if(this.RenderPINTriger)
                this.LoadData(this.CenterPostion);
        }

        public async void JumpToCurrent()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Location);
            if (status == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
            {
                try
                {
                    var p = await this.locator.GetPositionAsync(TimeSpan.FromSeconds(10));
                    aPosition = new Position(p.Latitude, p.Longitude);
                  
                }
                catch(Exception ex)
                {
                    (double lat, double longi) = Settings.Current.Position;
                    aPosition = new Position(lat, longi);

                }

            }
            else
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Plugin.Permissions.Abstractions.Permission.Location))
                {
                    this.Popup.Show(content: "Xin Hãy cấp quyền truy cập vị trí để app có thể hoạt động");
                }
                await CrossPermissions.Current.RequestPermissionsAsync(Plugin.Permissions.Abstractions.Permission.Location);
                (double lat, double longi) = Settings.Current.Position;
                aPosition = new Position(lat, longi);

            }
            Place.Position = aPosition;
            Address aAddress = (await this.locator.GetAddressesForPositionAsync(new Plugin.Geolocator.Abstractions.Position(aPosition.Latitude,aPosition.Longitude))).FirstOrDefault();
            if (aAddress != null)
            {
                FromLabel = $"{aAddress.FeatureName}, {aAddress.SubLocality}, {aAddress.SubAdminArea}";
            }
            else
            {
                FromLabel = "Vị trí hiện tại";
            }
            this.Radius = 2;
            this.FollowPostion = aPosition;
            Settings.Current.Position = (this.FollowPostion.Latitude,this.FollowPostion.Longitude);
            this.UserPostion = aPosition;
            this.LoadData(this.FollowPostion);
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
        private Pin _rendertriger;
        public Pin RenderTriger { get => _rendertriger; set => this.SetProperty(ref _rendertriger, value); }

        private void ShowMenuCommandExcute()
        {
            this.eventAggregator.GetEvent<Events.MenuEvent>().Publish(new Events.MenuMessage() { IsPresented = true });
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            this.eventAggregator.GetEvent<MenuEvent>().Publish(new MenuMessage() { IsPresented = false });
            if (parameters.ContainsKey("route"))
                return;
            this.RenderTriger = null;
            this.Place.IsVisible = false;
            this.caches.Clear();
            this.JumpToCurrent();            
            this.RenderPINTriger = true;
            this.Instructions = string.Empty;
            RaisePropertyChanged("RenderPINTriger");
            RaisePropertyChanged("RenderTriger");

        }

        public override bool OnBackButtonPressed()
        {
            this.RenderTriger = null;
            this.Place.IsVisible = false;
            this.caches.Clear();
            this.JumpToCurrent();
            this.RenderPINTriger = true;
            this.Instructions = string.Empty;
            RaisePropertyChanged("RenderPINTriger");
            RaisePropertyChanged("RenderTriger");
            return true;
        }
    }
}
