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
        public int page = 0, cur = -1, total = 0;
        public int Total { get => this.total; set => this.SetProperty(ref this.total, value); }

        private NotificationModel _LastTappedItem;
        public NotificationModel LastTappedItem
        {
            get => _LastTappedItem;
            set => this.SetProperty(ref _LastTappedItem, value);
        }
        public DelegateCommand ItemTappedCommand { get; set; }
        public FlowObservableCollection<Models.NotificationModel> Models { get; set; }
        public DelegateCommand LoaddingCommand { get; set; }

        public NotificationViewmodel(INavigationService navigationService, IHttpSevices _http) : base(navigationService)
        {
            HTTP = _http;
            Models = new FlowObservableCollection<NotificationModel>();
            this.LoaddingCommand = new DelegateCommand(LoaddingCommandExcute);
            this.ItemTappedCommand = new DelegateCommand(ItemTappedCommandExcute);
        }

        private void ItemTappedCommandExcute()
        {
            HTTP.Noti = LastTappedItem;
            this.Navigate("NotiDetail");
        }

        private void LoaddingCommandExcute()
        {
            this.LoadPage(page + 1);
        }

        public async void LoadPage(int p = 1)
        {
            this.IsLoadInfinite = true; 
            JObject oResult = await HTTP.GetNotifisAsync(p);
            if (oResult == null) return;
            this.Total = oResult["total"].Value<int>();
            if (oResult["data"].HasValues && oResult["data"] is JArray datas)
            {
                this.cur = oResult["to"].Value<int>();
                List<NotificationModel> helps = JsonConvert.DeserializeObject<List<NotificationModel>>(datas.ToString());
                if (helps != null)
                    this.Models.AddRange(helps);
            }
            else
            {
                this.cur = this.total;
            }
            page = oResult["current_page"].Value<int>();
            if (this.cur != this.total)
            {
                this.IsLoadInfinite = false;
            }


        }
        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if(parameters.GetNavigationMode()== NavigationMode.Back)
            {
                this.IsLoadInfinite = true;
                return;
            }
            this.LoadPage();
        }
    }

}
