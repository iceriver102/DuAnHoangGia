﻿using DuAnHoangGia.Models;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DuAnHoangGia.ViewModels
{
   public class NotificationViewmodel:ViewModelBase
    {
        public ObservableCollection<HelpModel> Models { get; set; }
        public NotificationViewmodel(INavigationService navigationService) : base(navigationService)
        {
            Models = new ObservableCollection<HelpModel>();
            for (int i = 0; i <= 10; i++)
            {
                Models.Add(new HelpModel() { Title = $"Công ty {i + 1}", Mota = " thong bao cua cong ty ", Thumbnail = "a1.png", Date = " 09/09/2018" });
            }
        }
    }
}