using System;
using System.Collections.Generic;
using System.Text;
using DuAnHoangGia.Models;
using DuAnHoangGia.Sevices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Navigation;

namespace DuAnHoangGia.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IHttpSevices HTTP;
        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand CheckCommand { get; set; }
        private bool check = false;

        public bool Allow
        {
            get => this.check;
            set
            {
                if (this.check == value)
                    return;
                RaisePropertyChanged("FakeToogle");
                this.SetProperty(ref this.check, value);
            }
        }

        public string FakeToogle
        {
            get
            {
                if (!this.check)
                    return "uncheck.png";
                return "check.png";
            }
        }
        private string _username, _pass;
        
        public string UserName { get=>this._username;  set=>this.SetProperty(ref this._username,value); }
        public string Password { get=>this._pass;  set=>this.SetProperty(ref this._pass,value); }

        public LoginViewModel(INavigationService navigationService, IHttpSevices _http) : base(navigationService) {
            HTTP = _http;
#if DEBUG
            this.UserName = "superadmin@gmail.com";
            this.Password = "123456";
#endif
            this.Popup = new PopupViewModel() { MainColor = Xamarin.Forms.Color.Red, Title = "Lỗi", Content = "Tên đăng nhập hoặc mật khẩu không đúng" };
            this.LoginCommand = new DelegateCommand(LoginCommandExcute);
            this.CheckCommand = new DelegateCommand(() => this.Allow ^= true);
        }

        private async void LoginCommandExcute()
        {
            var oResult = await this.HTTP.LoginAsync(this.UserName, this.Password);
            if(oResult.status== System.Net.HttpStatusCode.OK && oResult.data!=null)
            {
                if (oResult.data["user"] is JObject u)
                {
                    this.HTTP.User = JsonConvert.DeserializeObject<User>(u["customer"].ToString());
                    //if (u["is_company"].HasValues)
                        this.HTTP.User.Is_company = u["is_company"].Value<int>();
                    Settings.Current.Token = oResult.data["token"].ToString();
                    Settings.Current.Auto = this.Allow;
                    this.Navigate("app:///Home?appModuleRefresh=OnInitialized");
                }
            }
            else if(oResult.status ==System.Net.HttpStatusCode.GatewayTimeout)
            {
                this.Popup.Show(content: "Lỗi mạng không thể kết nối đến máy chủ. Hãy kiểm tra lại wifi hoặc 3G.");
                return;
            }
          
            this.Popup.IsVisible = true;
          
        }
    }
}
