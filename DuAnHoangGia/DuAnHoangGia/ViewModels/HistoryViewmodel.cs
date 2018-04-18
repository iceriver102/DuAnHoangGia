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
using System.Threading.Tasks;

namespace DuAnHoangGia.ViewModels
{
    public class HistoryViewmodel<T> : BindableBase, IDestructible
    {
        private bool _isLoadInfinite, _isview;
        public bool IsLoadInfinite { get => this._isLoadInfinite; set => this.SetProperty(ref this._isLoadInfinite, value); }
        public readonly IHttpSevices HTTP;
        public int page = 0;
        private int nums = 10;
        private int total = 1;
        public int Total { get => this.total; set => this.SetProperty(ref this.total, value); }
        private int cur = -1;
        public bool isView
        {
            get => this._isview; set
            {
                this.SetProperty(ref this._isview, value);
                if (value)
                {
                    if (this.IsLoadInfinite)
                    {
                        this.IsLoadInfinite = false;
                        this.Total = this.nums + 1;
                        page = 1;
                    }
                }
            }
        }

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
            this.IsLoadInfinite = true;
            JObject oResult = await HTTP.GetHelpsAsync(p, this.nums);

            if (oResult != null)
            {
                this.Total = oResult["total"].Value<int>();
                if (oResult["data"].HasValues && oResult["data"] is JArray datas)
                {
                    this.cur = oResult["to"].Value<int>();
                    List<T> helps = JsonConvert.DeserializeObject<List<T>>(datas.ToString());
                    if (helps != null)
                        this.Models.AddRange(helps);
                }
                else
                {
                    this.cur = this.total;
                }

                page = oResult["current_page"].Value<int>();

                if (this.cur == this.total)
                {
                    this.nums = this.total;
                    this.IsLoadInfinite = true;
                }
                else
                {
                    this.IsLoadInfinite = false;
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