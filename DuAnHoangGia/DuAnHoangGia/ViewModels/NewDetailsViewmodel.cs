using System;
using System.Collections.Generic;
using System.Text;
using DuAnHoangGia.Sevices;
using Prism.Navigation;
using Xamarin.Forms;

namespace DuAnHoangGia.ViewModels
{
    public class NewDetailsViewmodel : ViewModelBase
    {
        public IHttpSevices HTTP;
        private string _image ;
        private string _html;
        private WebView _WebView;
        public WebView WebView { get =>this._WebView; set=>this.SetProperty(ref _WebView,value); }
        public string Image { get => this._image; set => this.SetProperty(ref this._image, value); }
        public string HTML { get => this._html; set => this.SetProperty(ref this._html, value); }
        public NewDetailsViewmodel(INavigationService navigationService, IHttpSevices _http) : base(navigationService)
        {
            HTTP = _http;
        }

        public async override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            int newID =parameters.GetValue<int>("news");
            var oResult = await HTTP.GetNewAsync(newID);
            if (oResult != null)
            {
                this.Image = oResult["image"].ToString();
                //  this.HTML = new HtmlWebViewSource();
                this.HTML = oResult["description_long"].ToString();
            }
        }

        public  override void OnNavigatingTo(NavigationParameters parameters)
        {
            
            base.OnNavigatingTo(parameters);
            
            if (HTTP.News != null)
            {
                this.Image = HTTP.News.Image;

            }
           
           
        }


    }
}
