using DLToolkit.Forms.Controls;
using DuAnHoangGia.Models;
using DuAnHoangGia.Sevices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DuAnHoangGia.ViewModels
{
    public class NotificationViewmodel : ViewModelBase
    {

        private bool _isLoadInfinite;
        public bool IsLoadInfinite { get => this._isLoadInfinite; set => this.SetProperty(ref this._isLoadInfinite, value); }
        public readonly IHttpSevices HTTP;
        public int page = 0;


        public FlowObservableCollection<Models.NotificationModel> Models { get; set; }
        public DelegateCommand LoaddingCommand { get; set; }

        public NotificationViewmodel(INavigationService navigationService, IHttpSevices _http) : base(navigationService)
        {
            HTTP = _http;
            Models = new FlowObservableCollection<NotificationModel>();
            this.LoaddingCommand = new DelegateCommand(LoaddingCommandExcute);
        }

        private void LoaddingCommandExcute()
        {
            this.LoadPage(page + 1);
        }

        public async void LoadPage(int p = 1)
        {
            JObject oResult = await HTTP.GetNotifisAsync(p);
            if (oResult == null) return;
            if (oResult["data"].HasValues && oResult["data"] is JArray datas)
            {
                List<NotificationModel> helps = JsonConvert.DeserializeObject<List<NotificationModel>>(datas.ToString());
                if (helps != null)
                    this.Models.AddRange(helps);
                page = oResult["current_page"].Value<int>();
                if (oResult["last_page"].Value<int>() <= page)
                {
                    this.IsLoadInfinite = false;
                }
                else
                {
                    this.IsLoadInfinite = true;
                }
            }
        }
        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            this.LoadPage();
        }
    }
    
}
