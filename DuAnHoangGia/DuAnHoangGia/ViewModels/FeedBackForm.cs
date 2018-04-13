using DuAnHoangGia.Sevices;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace DuAnHoangGia.ViewModels
{
    public class FeedBackForm : BindableBase
    {
        private readonly IHttpSevices HTTP;
        private string _title, _content;
        public string Title { get => this._title; set => this.SetProperty(ref this._title, value); }

        private bool _isView;
        public bool isView { get => this._isView; set => this.SetProperty(ref this._isView, value); }

        public string Content { get => this._content; set => this.SetProperty(ref this._content, value); }
        public DelegateCommand SubmitCommand { get; private set; }
        public FeedBackForm(IHttpSevices _http)
        {
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
            }
        }
    }
}
