using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using DuAnHoangGia.Models;
using DuAnHoangGia.Sevices;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace DuAnHoangGia.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private string _name, _email, _phone, _password, _repassword;
        private bool check = false;
        public bool Allow
        {
            get => this.check;
            set
            {
                if (this.check == value)
                    return;
                RaisePropertyChanged("FakeToogle");
                this.SetProperty(ref this.check, value);
            }
        }

        public string FakeToogle
        {
            get
            {
                if (!this.check)
                    return "uncheck.png";
                return "check.png";
            }
        }

        public string Name { get => this._name; set => this.SetProperty(ref this._name, value); }
        public string Email { get => this._email; set => this.SetProperty(ref this._email, value); }
        public string Phone { get => this._phone; set => this.SetProperty(ref this._phone, value); }
        public string Password { get => this._password; set => this.SetProperty(ref this._password, value); }
        public string RePassword { get => this._repassword; set => this.SetProperty(ref this._repassword, value); }
        public DelegateCommand RegisterCommand { get; set; }
        public DelegateCommand CheckCommand { get; set; }
        private readonly IHttpSevices HTTP;
        private readonly IPageDialogService pageDialogService;
        public RegisterViewModel(INavigationService navigationService, IHttpSevices _http, IPageDialogService _pageDialogService) : base(navigationService)
        {
            HTTP = _http;
            this.pageDialogService = _pageDialogService;
            this.RegisterCommand = new DelegateCommand(RegisterCommandExcute);
            CheckCommand = new DelegateCommand(() => this.Allow ^= true);
        }

        private async void RegisterCommandExcute()
        {
           
            if (!this.check)
            {
                await this.pageDialogService.DisplayAlertAsync("Thông báo", "Bạn phải đồng ý với \"Điều kiện và Điều khoản\" tham gia", "OK");
                return;
            }
            if (string.IsNullOrEmpty(this.Name))
            {
                await this.pageDialogService.DisplayAlertAsync("Thông báo", "Họ và tên không được để trống", "OK");
                return;
            }
            if (string.IsNullOrEmpty(this.Email))
            {
                await this.pageDialogService.DisplayAlertAsync("Thông báo", "Email không được để trống", "OK");
                return;
            }
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(this.Email);
            if (!match.Success)
            {
                await this.pageDialogService.DisplayAlertAsync("Thông báo", "Email không đúng định dạng.", "OK");
                return;
            }
            if (string.IsNullOrEmpty(this.Password))
            {
                await this.pageDialogService.DisplayAlertAsync("Thông báo", "mật khẩu không được để trống.", "OK");
                return;
            }
            if (this.Password.Length < 5)
            {
                await this.pageDialogService.DisplayAlertAsync("Thông báo", "Mật khẩu quá ngắn. Hãy đặt mật khảu tối thiểu 6 ký tự.", "OK");
                return;
            }
            if (this.Password != this.RePassword)
            {
                await this.pageDialogService.DisplayAlertAsync("Thông báo", "Mật khẩu không trùng khớp.", "OK");
                return;
            }
            this.IsLoading = true;
            User u = new User();
            u.Email = this.Email;
            u.Name = this.Name;
            u.Pasword = this.Password;
            u.Phone = this.Phone;
            var oResult = await HTTP.RegisterAsync(u);
            this.IsLoading = false;
            if (oResult.result)
            {
                await this.pageDialogService.DisplayAlertAsync("Thông báo", "Đăng ký thành công. Hãy xác nhận tài khoản để đăng nhập.", "OK");
                await this.NavigationService.NavigateAsync("app:///Login?appModuleRefresh=OnInitialized");
            }
            else
            {
                await this.pageDialogService.DisplayAlertAsync("Thông báo", "Lỗi không tạm thời không thể đăng ký tài khoản", "OK");
            }
        }
    }
}
