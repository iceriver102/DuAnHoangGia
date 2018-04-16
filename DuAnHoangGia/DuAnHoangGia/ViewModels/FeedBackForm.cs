using DuAnHoangGia.Sevices;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DuAnHoangGia.ViewModels
{
    public class FeedBackForm : BindableBase
    {
        private readonly IHttpSevices HTTP;
        private string _title, _content;
        public string Title { get => this._title; set => this.SetProperty(ref this._title, value); }

        private bool _isView, _focus = true;
        
        public bool isView { get => this._isView; set { this.Forcus = value; this.SetProperty(ref this._isView, value); } }
        public bool Forcus { get => this._focus; set => this.SetProperty(ref this._focus, value); }

        public string Content { get => this._content; set => this.SetProperty(ref this._content, value); }
        public DelegateCommand SubmitCommand { get; private set; }
        private readonly IPageDialogService pageDialogService;
        public FeedBackForm(IHttpSevices _http, IPageDialogService _pageDialogService)
        {
            pageDialogService = _pageDialogService;
            this.HTTP = _http;
            SubmitCommand = new DelegateCommand(SubmitExcute);
        }
        private async void SubmitExcute()
        {
            var oResult = await HTTP.PostHelpAsync(this.Title, this.Content);
            if (oResult != null)
            {
                this.Title = "";
                this.Content = "";
                await pageDialogService.DisplayAlertAsync("Thông báo", "Gửi trợ giúp thành công, Chúng tôi sẽ liên hệ sau.", "OK");
            }
            else
            {
                await pageDialogService.DisplayAlertAsync("Thông báo", "Gửi trợ giúp không thành công, Hãy thử lại sau.", "OK");
            }
        }
    }
}
