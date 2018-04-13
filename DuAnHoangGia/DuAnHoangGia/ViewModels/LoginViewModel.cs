using System;
using System.Collections.Generic;
using System.Text;
using DuAnHoangGia.Models;
using DuAnHoangGia.Sevices;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;

namespace DuAnHoangGia.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IHttpSevices HTTP;
        public DelegateCommand LoginCommand { get; set; }

        private string _username, _pass;
        public string UserName { get=>this._username;  set=>this.SetProperty(ref this._username,value); }
        public string Password { get=>this._pass;  set=>this.SetProperty(ref this._pass,value); }

        public LoginViewModel(INavigationService navigationService, IHttpSevices _http) : base(navigationService) {
            HTTP = _http;
            this.UserName = "superadmin@gmail.com";
            this.Password = "123456";
            this.LoginCommand = new DelegateCommand(LoginCommandExcute);
        }

        private async void LoginCommandExcute()
        {
            var oResult = await this.HTTP.LoginAsync(this.UserName, this.Password);
            if(oResult.status== System.Net.HttpStatusCode.OK)
            {
                if (oResult.data == null)
                    return;
                this.HTTP.User = JsonConvert.DeserializeObject<User>(oResult.data["customer"][0].ToString());
                Settings.Current.Token = oResult.data["token"].ToString();
                await this.NavigationService.NavigateAsync("app:///Home?appModuleRefresh=OnInitialized");
            }
          
        }
    }
}
