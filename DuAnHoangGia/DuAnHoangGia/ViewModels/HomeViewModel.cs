using System;
using System.Collections.Generic;
using System.Text;
using DuAnHoangGia.Events;
using DuAnHoangGia.Sevices;
using DuAnHoangGia.Views.Home;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;

namespace DuAnHoangGia.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IHttpSevices HTTP;
        private bool _IsPresented;
        public bool IsPresented { get => this._IsPresented; set => this.SetProperty(ref this._IsPresented, value); }

        public DelegateCommand ShowMenuCommand { get; set; }

        public HomeViewModel(INavigationService navigationService, IHttpSevices _http, IPageDialogService _pageDialogService,IEventAggregator eventAggregator) : base(navigationService)
        {
            this.HTTP = _http;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<MenuEvent>().Subscribe((m) => this.IsPresented = m.IsPresented);
            this.ShowMenuCommand = new DelegateCommand(() => { if (this.IsPresented) RaisePropertyChanged("IsPresented"); else this.IsPresented = true; });
        }


    }
}
