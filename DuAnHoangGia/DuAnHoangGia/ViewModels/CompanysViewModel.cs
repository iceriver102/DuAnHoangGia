using DLToolkit.Forms.Controls;
using DuAnHoangGia.Models;
using DuAnHoangGia.Sevices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;

namespace DuAnHoangGia.ViewModels
{
    public class CompanysViewModel : ViewModelBase
    {
        private readonly IHttpSevices HTTP;
        private int page = 0;
        private CompanyModel _LastTappedItem;
        public CompanyModel LastTappedItem
        {
            get=> _LastTappedItem;
            set =>this.SetProperty(ref _LastTappedItem,value);
        }
        private bool _isLoadInfinite;
        public bool IsLoadInfinite { get=>this._isLoadInfinite; set=>this.SetProperty(ref this._isLoadInfinite,value); }

        public DelegateCommand ItemTappedCommand { get; set; }
        public DelegateCommand LoaddingCommand { get; set; }
        public FlowObservableCollection<CompanyModel> Models { get; set; }
      
        public CompanysViewModel(INavigationService navigationService, IHttpSevices _http) : base(navigationService)
        {
            HTTP = _http;
            Models = new FlowObservableCollection<CompanyModel>();
            LoaddingCommand = new DelegateCommand(LoaddingCommandExcute);
            ItemTappedCommand = new DelegateCommand(ItemTappedCommandExcute);
            
        }

        public async void LoadPage(int p=1)
        {
            JObject oResult = await HTTP.GetCompanysAsync(p);
            if (oResult == null) return;
            if (oResult["data"].HasValues && oResult["data"] is JArray datas)
            {
                List<CompanyModel> comps = JsonConvert.DeserializeObject<List<CompanyModel>>(datas.ToString());
                if (comps != null)
                    this.Models.AddRange(comps);
                page = oResult["current_page"].Value<int>();
                if (oResult["last_page"].Value<int>() <= page)
                {
                    this.IsLoadInfinite = false;
                }
            }
        }

        private void LoaddingCommandExcute()
        {
            LoadPage(page + 1);
        }

        private void ItemTappedCommandExcute()
        {
            this.Navigate($"ComDetail?comp={this.LastTappedItem.id}");
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            LoadPage();
        }
    }
}
