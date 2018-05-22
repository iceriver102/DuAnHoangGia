using System;
using System.Collections.Generic;
using System.Text;
using DLToolkit.Forms.Controls;
using DuAnHoangGia.Models;
using DuAnHoangGia.Sevices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms.Maps;

namespace DuAnHoangGia.ViewModels
{
    public class PlaceViewModel : BindableBase
    {
        private string _tolabel;
        public string ToLabel { get=>this._tolabel; set=>this.SetProperty(ref _tolabel, value); }
        public DelegateCommand DismissCommand { get; private set; }
        public FlowObservableCollection<CompanyModel> Models { get; set; }
        private CompanyModel _LastTappedItem;
        public CompanyModel LastTappedItem
        {
            get => _LastTappedItem;
            set => this.SetProperty(ref _LastTappedItem, value);
        }

        public DelegateCommand<CompanyModel> Goto { get; set; }
        public DelegateCommand ItemTappedCommand { get; private set; }
        public DelegateCommand SearchCommand { get;  set; }
        public DelegateCommand LoaddingCommand { get; set; }
        private bool isView = false, _isLoadInfinite;
        public int total, cur, page;
        public bool IsVisible { get => this.isView; set => this.SetProperty(ref this.isView, value); }
        
        public bool IsLoadInfinite { get=>this._isLoadInfinite; set=> this.SetProperty(ref this._isLoadInfinite, value); }
        public int Total { get=> total; set=> this.SetProperty(ref total,value); }
        public Position Position { get; set; }

        public readonly IHttpSevices HTTP;
        public PlaceViewModel(string to, IHttpSevices _http)
        {
            this.ToLabel = to;
            HTTP = _http;
            this.Models = new FlowObservableCollection<CompanyModel>();
            this.IsVisible = false;
            
            DismissCommand = new DelegateCommand(() => { this.IsVisible = false; this.Models.Clear(); });
            LoaddingCommand = new DelegateCommand(() =>
            {
                if (string.IsNullOrEmpty(this.ToLabel))
                {
                    this.LoadPage(page + 1);
                }
            });

            ItemTappedCommand = new DelegateCommand(() => {
                this.IsVisible = false; this.Models.Clear();
                if (this.Goto != null) this.Goto.Execute(this.LastTappedItem); });
        }
        public async void LoadPage(int p = 1)
        {
            this.IsLoadInfinite = true;
#if DEBUG
            JObject oResult = await HTTP.GetCompanysAsync(page: p);
#else
            JObject oResult = await HTTP.GetCompanysAsync(page:p,lat: Position.Latitude,log:Position.Longitude);
#endif
            if (oResult == null) return;
            this.Total = oResult["total"].Value<int>();
            page = oResult["current_page"].Value<int>();
            if (oResult["data"].HasValues && oResult["data"] is JArray datas)
            {
                this.cur = oResult["to"].Value<int>();
                List<CompanyModel> comps = JsonConvert.DeserializeObject<List<CompanyModel>>(datas.ToString());
                if (comps != null)
                    this.Models.AddRange(comps);
            }
            else
            {
                this.cur = this.total;
            }
            if (this.cur == this.total)
            {
                this.IsLoadInfinite = true;
            }
            else
            {
                this.IsLoadInfinite = false;
            }
        }

    }
}
