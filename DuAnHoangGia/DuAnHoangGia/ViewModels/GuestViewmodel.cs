using DLToolkit.Forms.Controls;
using DuAnHoangGia.Models;
using DuAnHoangGia.Sevices;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DuAnHoangGia.ViewModels
{
    public class GuestViewmodel : ViewModelBase
    {
       
        private string _key;
        public string Key { get => this._key; set => this.SetProperty(ref this._key, value); }
        public DelegateCommand SearchCommand { get; set; }

        private User _LastTappedItem;
        public User LastTappedItem
        {
            get => _LastTappedItem;
            set => this.SetProperty(ref _LastTappedItem, value);
        }
        public DelegateCommand ItemTappedCommand { get; set; }
        public DelegateCommand LoaddingCommand { get; set; }
        public FlowObservableCollection<User> Models { get; set; }
        private readonly IHttpSevices http;
        private bool _isLoadInfinite;
        public bool IsLoadInfinite { get => this._isLoadInfinite; set => this.SetProperty(ref this._isLoadInfinite, value); }
        public GuestViewmodel(INavigationService navigationService, IHttpSevices _http) : base(navigationService)
        {
            http = _http;
            this.SearchCommand = new DelegateCommand(SearchCommandExcute);
            Models = new FlowObservableCollection<User>();
            ItemTappedCommand = new DelegateCommand(ItemTappedCommandExcute);
#if DEBUG
            this.Key = "H764TT";
#endif

        }

       

        private async void ItemTappedCommandExcute()
        {
            var oResult = await http.AddGuestAsync(LastTappedItem.Id);
           
            if (oResult)
            {
                this.Popup.Show("Thông báo", "Thêm khách hàng thành công", Xamarin.Forms.Color.FromHex("#38c1f3"));
            }
            else
            {
                this.Popup.Show("Lỗi", "Không thể thêm khách hàng. Xin hãy thử lại sau");
            }
           
        }

       

        private async void SearchCommandExcute()
        {
            if (!string.IsNullOrEmpty(this.Key))
            {
                this.Models.Clear();
                var oResult = await http.GetGuestsAsync(Key);
                if (oResult != null)
                {
                    List<User> temp = JsonConvert.DeserializeObject<List<User>>(oResult.ToString());
                    this.Models.AddRange(temp);
                }
            }
            else
            {
                this.Popup.Show("Lỗi", "Mã khách hàng không được để trống", Xamarin.Forms.Color.Red);
            }
        }
    }
}
