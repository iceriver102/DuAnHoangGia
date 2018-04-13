using DuAnHoangGia.Events;
using DuAnHoangGia.Sevices;
using DuAnHoangGia.Views.Home;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DuAnHoangGia.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly IEventAggregator _ea;
        public DelegateCommand<HomePageMenuItem> ItemTapped { get; set; }
        public ObservableCollection<HomePageMenuItem> MenuItems { get; set; }
        public MenuViewModel(INavigationService navigationService, IHttpSevices _http, IEventAggregator eventAggregator) : base(navigationService)
        {

            this._ea = eventAggregator;

            MenuItems = new ObservableCollection<HomePageMenuItem>(new[]
            {
                    new HomePageMenuItem (0){ Title = "Thông tin cá nhân",Icon="user_icon.png", Navs="Profile" },
                    new HomePageMenuItem (1){ Title = "Danh sách công ty",Icon="danhsach_icon.png", Navs="Companys" },
                    new HomePageMenuItem (2){  Title = "Tin tức",Icon="news_icon.png", Navs="News" },
                    new HomePageMenuItem (3){  Title = "Thông báo",Icon="noti.png", Navs="Noti" },
                  new HomePageMenuItem (4){  Title = "Trợ giúp",Icon="help_icon.png", Navs="Help" },
                });
            this.ItemTapped = new DelegateCommand<HomePageMenuItem>(this.ItemTappedExcute);
        }

        private async void ItemTappedExcute(HomePageMenuItem m)
        {
            this._ea.GetEvent<MenuEvent>().Publish(new MenuMessage() { IsPresented = false });
           await this.NavigationService.NavigateAsync(m.Navs);
            
        }

    }

}
