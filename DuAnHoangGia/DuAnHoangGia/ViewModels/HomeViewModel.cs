using System;
using System.Collections.Generic;
using System.Text;
using DuAnHoangGia.Events;
using DuAnHoangGia.Views.Home;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;

namespace DuAnHoangGia.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly IEventAggregator eventAggregator;
        private bool _IsPresented;
        public bool IsPresented { get => this._IsPresented; set => this.SetProperty(ref this._IsPresented, value); }

        public DelegateCommand ShowMenuCommand { get; set; }

        public HomeViewModel(INavigationService navigationService, IEventAggregator eventAggregator) : base(navigationService)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<MenuEvent>().Subscribe((m) => this.IsPresented = m.IsPresented);
            this.ShowMenuCommand = new DelegateCommand(() => { if (this.IsPresented) RaisePropertyChanged("IsPresented"); else this.IsPresented = true; });
        }


    }
}
