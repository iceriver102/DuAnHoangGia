using DuAnHoangGia.Sevices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DuAnHoangGia.ViewModels
{
    public class CompanyDetailViewmodel : ViewModelBase
    {
        private Models.CompanyModel COM;
        public readonly IHttpSevices HTTP;
        public readonly IEventAggregator eventAggregator;
        private string _url, _title, _content;
        public string UrlImage { get=>this._url; set=>this.SetProperty(ref this._url,value); }
        public string Title { get=>this._title; set=>this.SetProperty(ref this._title,value); }
        public string Content { get=>this._content; set=>this.SetProperty(ref this._content, value); }
        public DelegateCommand Goto { get; private set; }
        public CompanyDetailViewmodel(INavigationService navigationService, IHttpSevices _http, IEventAggregator eventAggregator) : base(navigationService)
        {
            HTTP = _http;
            this.eventAggregator = eventAggregator;
            Goto = new DelegateCommand(GotoExcute);
        }

        private async void GotoExcute()
        {
            if (COM != null)
            {
                HTTP.COM = COM;
                var p = new NavigationParameters();
                p.Add("route", "home");
                this.eventAggregator.GetEvent<Events.CompanyEvent>().Publish(COM);
                await   this.NavigationService.GoBackAsync(p);
            }
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);
        }

        public async override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("comp"))
            {
                int comp = parameters.GetValue<int>("comp");
                JObject oResult=  await this.HTTP.GetCompanyAsync(comp);
                if (oResult != null)
                {
                    COM = JsonConvert.DeserializeObject<Models.CompanyModel>(oResult.ToString());
                    this.Title = oResult["name"].Value<string>();
                    this.Content = oResult["description"].Value<string>();
                    this.UrlImage = oResult["avatar"].Value<string>();
                }
            }
        }
    }
}
