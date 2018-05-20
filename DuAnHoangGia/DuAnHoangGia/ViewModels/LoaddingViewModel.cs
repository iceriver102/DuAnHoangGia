using DuAnHoangGia.Models;
using DuAnHoangGia.Sevices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

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
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Location);    
            if (status== Plugin.Permissions.Abstractions.PermissionStatus.Granted)
            {
                try
                {
                    var task = this.locator.GetPositionAsync(TimeSpan.FromSeconds(10));
                    await Task.WhenAll(task, Task.Delay(500));
                    var p = task.Result;
                    Settings.Current.Position = (lat: p.Latitude, log: p.Longitude);
                }
                catch (Exception ex)
                {
                    this.Popup.Show(content: "Xin hãy bật dịch vụ GPS để app có thể hoạt động");
                    return;
                }
            }
            else
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Plugin.Permissions.Abstractions.Permission.Location))
                {
                    this.Popup.Show(content: "Xin Hãy cấp quyền truy cập vị trí để app có thể hoạt động");
                }
                await CrossPermissions.Current.RequestPermissionsAsync(Plugin.Permissions.Abstractions.Permission.Location);
                
                //if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Plugin.Permissions.Abstractions.Permission.Location))
                //{
                //    this.Popup.Show(content: "Xin Hãy cấp quyền truy cập vị trí để app có thể hoạt động");
                //    //Toast.MakeText(this, "Toestemming is nodig voor het downloaden van de update.", ToastLength.Short).Show();
                //}
                //await CrossPermissions.Current.RequestPermissionsAsync(Plugin.Permissions.Abstractions.Permission.Location);
                //return;
            }
            bool flag = DependencyService.Get<INetwork>().IsOnline();
            if (!flag)
            {
                this.Popup.Show(content: "Hiện tại bạn không online. xin hãy kiểm tra lại mạng và thử lại sau");
            }
            if (Settings.Current.Auto && flag)
            {
                var TaskoResult = this.HTTP.GetUser();
                await Task.WhenAll(TaskoResult, Task.Delay(800));
                var oResult = TaskoResult.Result;
                if (oResult != null && oResult["user"] is JObject u)
                {
                    this.HTTP.User = JsonConvert.DeserializeObject<User>(u["customer"].ToString());
                    //if (u["is_company"].HasValues)
                    this.HTTP.User.Is_company = u["is_company"].Value<int>();
                    this.Navigate("app:///Home?appModuleRefresh=OnInitialized");
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
