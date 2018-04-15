using DuAnHoangGia.Sevices;
using Newtonsoft.Json.Linq;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DuAnHoangGia.ViewModels
{
    public class CompanyDetailViewmodel : ViewModelBase
    {
        public readonly IHttpSevices HTTP;
        private string _url, _title, _content;
        public string UrlImage { get=>this._url; set=>this.SetProperty(ref this._url,value); }
        public string Title { get=>this._title; set=>this.SetProperty(ref this._title,value); }
        public string Content { get=>this._content; set=>this.SetProperty(ref this._content, value); }
        public CompanyDetailViewmodel(INavigationService navigationService, IHttpSevices _http) : base(navigationService)
        {
            HTTP = _http;
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
                    this.Title = oResult["name"].Value<string>();
                    this.Content = oResult["description"].Value<string>();
                    this.UrlImage = oResult["avatar"].Value<string>();
                }
            }
        }
    }
}
