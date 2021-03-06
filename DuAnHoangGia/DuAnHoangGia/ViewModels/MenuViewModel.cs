﻿using DuAnHoangGia.Events;
using DuAnHoangGia.Sevices;
using DuAnHoangGia.Views.Home;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DuAnHoangGia.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly IEventAggregator _ea;
        private readonly IHttpSevices HTTP;
        public DelegateCommand<HomePageMenuItem> ItemTapped { get; set; }
        public ObservableCollection<HomePageMenuItem> MenuItems { get; set; }
        public MenuViewModel(INavigationService navigationService, IHttpSevices _http, IPageDialogService _pageDialogService, IEventAggregator eventAggregator) : base(navigationService)
        {
            this._ea = eventAggregator;
            HTTP = _http;
            MenuItems = new ObservableCollection<HomePageMenuItem>(new[]
               {
                new HomePageMenuItem (0){ Title = "Trang Chủ",Icon="home.png", Navs="app:///Home?appModuleRefresh=OnInitialized" },
                    new HomePageMenuItem (1){ Title = "Thông tin cá nhân",Icon="user_icon.png", Navs="Profile" },
                    new HomePageMenuItem (2){  Title = "Danh sách công ty",Icon="danhsach_icon.png", Navs="Companys" },
                    new HomePageMenuItem (3){  Title = "Tin tức",Icon="news_icon.png", Navs="News" },
                    new HomePageMenuItem (4){  Title = "Thông báo",Icon="noti.png", Navs="Noti" },
                    new HomePageMenuItem (5){  Title = "Trợ giúp",Icon="help_icon.png", Navs="Help" },                   
            });
            if (HTTP.User.Is_company==1)
            {
                MenuItems.Add(new HomePageMenuItem(5) { Title = "Tìm khách hàng", Icon = "guest.png", Navs = "Guest" });
                MenuItems.Add(new HomePageMenuItem(6) { Title = "Thoát", Icon = "logout.png", Navs = "Exit" });
            }
            else
            {
                MenuItems.Add(new HomePageMenuItem(5) { Title = "Thoát", Icon = "logout.png", Navs = "Exit" });
            }
            this.ItemTapped = new DelegateCommand<HomePageMenuItem>(this.ItemTappedExcute);
        }

        private async void ItemTappedExcute(HomePageMenuItem m)
        {
            if (m.Navs == "Exit")
            {
                Settings.Current.Auto = false;
                HTTP.User = null;
                Settings.Current.Token = string.Empty;
                this.Navigate("app:///Loadding?appModuleRefresh=OnInitialized");
                return;
            }
            else
            {
                this._ea.GetEvent<MenuEvent>().Publish(new MenuMessage() { IsPresented = false });
                this.Navigate(m.Navs);
            }

        }

    }

}
