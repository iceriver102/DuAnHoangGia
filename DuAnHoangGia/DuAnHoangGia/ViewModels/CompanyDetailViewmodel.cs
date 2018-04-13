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
                int comp = Convert.ToInt32(parameters["comp"]);
                JObject oResult=  await  this.HTTP.GetCompanyAsync(comp);
                if (oResult != null)
                {
                    this.Title = oResult["name"].Value<string>();
                    this.Content = oResult["description"].Value<string>();
                    string _tmp = oResult["avatar"].Value<string>();
                    if (_tmp.StartsWith("http://"))
                        this.UrlImage = _tmp;
                    this.UrlImage = $"http://{_tmp}";
                }
            }
        }
    }
}
