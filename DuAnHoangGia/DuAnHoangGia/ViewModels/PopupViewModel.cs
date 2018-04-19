using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
namespace DuAnHoangGia.ViewModels
{
    public class PopupViewModel : BindableBase
    {
        private bool isVisible = false;
        public bool IsVisible { get => this.isVisible; set => this.SetProperty(ref isVisible, value); }
        private string title, content;
        private Color c;
        public Color MainColor { get => this.c; set { this.SetProperty(ref this.c, value); RaisePropertyChanged("Background"); } }
        public string Title { get => this.title; set => this.SetProperty(ref this.title, value); }
        public string Content { get => this.content; set => this.SetProperty(ref this.content, value); }
        public string Background
        {
            get
            {
                if (this.c == Color.Red)
                    return "err_bg.png";
                return "ss_bg.png";
            }
        }
        public DelegateCommand FuncDismiss { get; private set; }
        public DelegateCommand DismissView { get; set; }
        public PopupViewModel()
        {
            this.IsVisible = false;
            this.DismissView = new DelegateCommand(() =>
            {
                if (this.FuncDismiss != null)
                    this.FuncDismiss.Execute();
                this.IsVisible = false;
                this.FuncDismiss = null;
            });
        }

        public void Show(string title = "Lỗi", string content = "", Color? red = null, DelegateCommand dismiss=null)
        {
            if (red == null)
            {
                red = Color.Red;
            }
            this.Title = title;
            this.Content = content;
            this.MainColor = red.Value;
            this.IsVisible = true;
            this.FuncDismiss = dismiss;

        }
    }
}
