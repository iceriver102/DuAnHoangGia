using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using DLToolkit.Forms.Controls;
using DuAnHoangGia.Models;
using DuAnHoangGia.Sevices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Navigation;

namespace DuAnHoangGia.ViewModels
{
    public class NewsViewmodel : ViewModelBase
    {
        private bool _isLoadInfinite;
        public bool IsLoadInfinite { get => this._isLoadInfinite; set => this.SetProperty(ref this._isLoadInfinite, value); }
        public readonly IHttpSevices HTTP;
        public int page = 0;

        private HelpModel _LastTappedItem;
        public HelpModel LastTappedItem
        {
            get => _LastTappedItem;
            set => this.SetProperty(ref _LastTappedItem, value);
        }
        public DelegateCommand ItemTappedCommand { get; set; }
        public FlowObservableCollection<NewsModel> Models { get; set; }
        public DelegateCommand LoaddingCommand { get; set; }
        public NewsViewmodel(INavigationService navigationService, IHttpSevices _http) : base(navigationService)
        {
            HTTP = _http;
            Models = new FlowObservableCollection<NewsModel>();
            this.IsLoadInfinite = true;
            ItemTappedCommand = new DelegateCommand(ItemTappedExcute);
            this.LoaddingCommand = new DelegateCommand(LoaddingCommandExcute);
        }

        private void LoaddingCommandExcute()
        {
            this.LoadPage(page + 1);
        }

        private void ItemTappedExcute()
        {
            this.Navigate("Detail");
        }

        public async void LoadPage(int p = 1)
        {
            JObject oResult = await HTTP.GetNewsAsync(p);
            if (oResult == null) return;
            if (oResult["data"].HasValues && oResult["data"] is JArray datas)
            {
                List<NewsModel> helps = JsonConvert.DeserializeObject<List<NewsModel>>(datas.ToString());
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
