﻿using FFImageLoading.Forms;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using DuAnHoangGia.Views;
using DuAnHoangGia.ViewModels;
using DuAnHoangGia.Views.Home;
using DuAnHoangGia.Sevices;

namespace DuAnHoangGia
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected async override void OnInitialized()
        {
            InitializeComponent();
            CachedImage.FixedOnMeasureBehavior = true;
            CachedImage.FixedAndroidMotionEventHandler = true;         
            await NavigationService.NavigateAsync("Loadding",useModalNavigation:true);

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IHttpSevices, HttpSevices>();
            containerRegistry.RegisterForNavigation<NavigationPage>("Navigation");
            containerRegistry.RegisterForNavigation<LoaddingPage, LoaddingViewModel>("Loadding");
            containerRegistry.RegisterForNavigation<LoginPage, LoginViewModel>("Login");
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterViewModel>("Register");
            containerRegistry.RegisterForNavigation<HomePageMaster, MenuViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomeViewModel>("Home");
            containerRegistry.RegisterForNavigation<CompanysPage, CompanysViewModel>("Companys");
            containerRegistry.RegisterForNavigation<ProfilePage, ProfileViewModel>("Profile");
            containerRegistry.RegisterForNavigation<FeedBackPage, FeedBackViewmodel>("Help");
            containerRegistry.RegisterForNavigation<NotificationPage, NotificationViewmodel>("Noti");
            containerRegistry.RegisterForNavigation<NewsPage, NewsViewmodel>("News");
            containerRegistry.RegisterForNavigation<NewDetailsPage, NewDetailsViewmodel>("Detail");
            containerRegistry.RegisterForNavigation<CompanyDetailPage, CompanyDetailViewmodel>("ComDetail");
            containerRegistry.RegisterForNavigation<GuestPage, GuestViewmodel>("Guest");
            containerRegistry.RegisterForNavigation<NotiPage, NotiDetail>("NotiDetail");
            containerRegistry.RegisterForNavigation<HomePageDetail, MapViewModel>();
        }
    }
}
