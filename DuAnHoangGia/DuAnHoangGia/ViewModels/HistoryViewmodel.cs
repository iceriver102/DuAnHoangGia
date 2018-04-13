using DLToolkit.Forms.Controls;
using DuAnHoangGia.Models;
using DuAnHoangGia.Sevices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DuAnHoangGia.ViewModels
{
    public class HistoryViewmodel<T> : BindableBase, IDestructible
    {
        private bool _isLoadInfinite, _isview;
        public bool IsLoadInfinite { get => this._isLoadInfinite; set => this.SetProperty(ref this._isLoadInfinite, value); }
        public readonly IHttpSevices HTTP;
        public int page = 0;

        public bool isView { get => this._isview; set => this.SetProperty(ref this._isview, value); }

        public FlowObservableCollection<T> Models { get; set; }
        public DelegateCommand LoaddingCommand { get; set; }
        public HistoryViewmodel(IHttpSevices _http)
        {
            HTTP = _http;
            this.isView = true;
            Models = new FlowObservableCollection<T>();
            this.LoaddingCommand = new DelegateCommand(LoaddingCommandExcute);
        }

        public virtual async void LoadPage(int p = 1)
        {
            JObject oResult = await HTTP.GetHelpsAsync(p);
            if (oResult == null) return;
            if (oResult["data"].HasValues && oResult["data"] is JArray datas)
            {
                List<T> helps = JsonConvert.DeserializeObject<List<T>>(datas.ToString());
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

        private void LoaddingCommandExcute()
        {
            LoadPage(page + 1);
        }

        public void Destroy()
        {

        }
    }
}