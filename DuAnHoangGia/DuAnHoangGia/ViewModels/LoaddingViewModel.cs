using DuAnHoangGia.Models;
using DuAnHoangGia.Sevices;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DuAnHoangGia.ViewModels
{
    public class LoaddingViewModel : ViewModelBase
    {
        IGeolocator locator;
        private readonly IHttpSevices HTTP;

        private bool _ishow, _runAnim;

        public bool Ishow { get => this._ishow; set => this.SetProperty(ref this._ishow, value); }
        public bool RunAnim { get => this._runAnim; set => this.SetProperty(ref this._runAnim, value); }

        public LoaddingViewModel(INavigationService navigationService, IHttpSevices _http) : base(navigationService)
        {
            this.Ishow = false;
            this.HTTP = _http;
            locator = CrossGeolocator.Current;
        }

        public async override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            var task = this.locator.GetPositionAsync(TimeSpan.FromSeconds(10));
            await Task.WhenAll(task, Task.Delay(500));
            var p = task.Result;
            Settings.Current.Position = (lat: p.Latitude, log: p.Longitude);
            if (Settings.Current.Auto)
            {
                var TaskoResult = this.HTTP.GetUser();
                await Task.WhenAll(TaskoResult, Task.Delay(800));
                var oResult = TaskoResult.Result;
                if (oResult != null)
                {
                    this.HTTP.User = JsonConvert.DeserializeObject<User>(oResult["customer"].ToString());
                    await this.NavigationService.NavigateAsync("app:///Home?appModuleRefresh=OnInitialized");

                }
                else
                {                   
                    this.Ishow = true;
                    Settings.Current.Auto = false;
                    Settings.Current.Token = string.Empty;
                }
                //
            }
            else
            {
                await Task.Delay(800);
                this.Ishow = true;
            }
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);
            this.RunAnim = true;

        }
    }
}
