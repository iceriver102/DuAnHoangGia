using System;
using System.Collections.Generic;
using System.Text;
using DuAnHoangGia.Sevices;
using Prism.Navigation;

namespace DuAnHoangGia.ViewModels
{
    public class NotiDetail : ViewModelBase
    {
        private string _title, _content;
        public string Title { get => this._title; set => this.SetProperty(ref this._title, value); }
        public string Content { get => this._content; set => this.SetProperty(ref this._content, value); }
        public NotiDetail(INavigationService navigationService, IHttpSevices _http) : base(navigationService)
        {
            this.Title = _http.Noti.Name;
            this.Content = _http.Noti.Description;
        }
    }
}
