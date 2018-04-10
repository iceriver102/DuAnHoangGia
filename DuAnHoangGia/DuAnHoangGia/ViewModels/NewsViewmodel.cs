using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using DuAnHoangGia.Models;
using Prism.Commands;
using Prism.Navigation;

namespace DuAnHoangGia.ViewModels
{
    public class NewsViewmodel : ViewModelBase
    {
        private HelpModel _LastTappedItem;
        public HelpModel LastTappedItem
        {
            get => _LastTappedItem;
            set => this.SetProperty(ref _LastTappedItem, value);
        }
        public DelegateCommand ItemTapped { get; set; }
        public ObservableCollection<HelpModel> Models { get; set; }
        public NewsViewmodel(INavigationService navigationService) : base(navigationService)
        {
            Models = new ObservableCollection<HelpModel>();
            for (int i = 0; i <= 10; i++)
            {
                Models.Add(new HelpModel() { Title = "CÔNG TY CỔ PHẦN SX TM XNK VIỄN THÔNG A", Mota = "Được thành lập vào tháng 11 năm 1997, công ty Viễn Thông A đã khẳng định được thương hiệu của mình, và trở thành \"Sự lựa chọn tốt nhất của bạn\", với 100 trung tâm bảo hành, gần 200 cửa hàng tại hệ thống siêu thị BigC, CoopMart và Trung Tâm Smartphone – Tablet – Laptop trên toàn quốc. Đến với Viễn Thông A, khách hàng sẽ có những trải nghiệm tuyệt vời bởi những cam kết của Viễn Thông A đối với khách hàng:", Thumbnail = "a1.png" });
            }
            ItemTapped = new DelegateCommand(ItemTappedExcute);
        }

        private void ItemTappedExcute()
        {
            this.Navigate("Detail");
        }
    }
}
