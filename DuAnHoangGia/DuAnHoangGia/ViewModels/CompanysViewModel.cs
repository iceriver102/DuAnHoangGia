using DuAnHoangGia.Models;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace DuAnHoangGia.ViewModels
{
    public class CompanysViewModel : ViewModelBase
    {
        private CompanyModel _LastTappedItem;
        public CompanyModel LastTappedItem
        {
            get=> _LastTappedItem;
            set =>this.SetProperty(ref _LastTappedItem,value);
        }
        public DelegateCommand ItemTappedCommand { get; set; }
        public ObservableCollection<CompanyModel> Models { get; set; }
        public CompanysViewModel(INavigationService navigationService) : base(navigationService)
        {
            Models = new ObservableCollection<CompanyModel>();
            for (int i = 0; i <= 10; i++)
            {
                Models.Add(new CompanyModel() { Title = $"Công ty {i + 1}",  Url = "c1.jpg" });
            }
            ItemTappedCommand = new DelegateCommand(ItemTappedCommandExcute);
        }

        private void ItemTappedCommandExcute()
        {
            Debug.WriteLine(this.LastTappedItem.Title);
            this.Navigate("ComDetail");

        }
    }
}
