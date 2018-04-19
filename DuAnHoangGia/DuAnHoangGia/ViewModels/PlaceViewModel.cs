using System;
using System.Collections.Generic;
using System.Text;
using DLToolkit.Forms.Controls;
using DuAnHoangGia.Models;
using DuAnHoangGia.Sevices;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace DuAnHoangGia.ViewModels
{
    public class PlaceViewModel : BindableBase
    {
        public DelegateCommand DismissCommand { get; private set; }
        public FlowObservableCollection<CompanyModel> Models { get; set; }
        private CompanyModel _LastTappedItem;
        public CompanyModel LastTappedItem
        {
            get => _LastTappedItem;
            set => this.SetProperty(ref _LastTappedItem, value);
        }

        public DelegateCommand<CompanyModel> Goto { get; set; }
        public DelegateCommand ItemTappedCommand { get; private set; }

        private bool isView = false;
        public bool IsVisible { get => this.isView; set => this.SetProperty(ref this.isView, value); }
        public PlaceViewModel()
        {
            this.Models = new FlowObservableCollection<CompanyModel>();
            this.IsVisible = false;
            DismissCommand = new DelegateCommand(() => { this.IsVisible = false; this.Models.Clear(); });
            ItemTappedCommand = new DelegateCommand(() => {
                this.IsVisible = false; this.Models.Clear();
                if (this.Goto != null) this.Goto.Execute(this.LastTappedItem); });
        }
    }
}
