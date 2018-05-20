using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Prism.Commands;
using System.Diagnostics;
using Plugin.FilePicker.Abstractions;
using Plugin.FilePicker;
using DuAnHoangGia.Sevices;
using DuAnHoangGia.Models;
using System.Text.RegularExpressions;
using Prism.Services;

namespace DuAnHoangGia.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        public DelegateCommand ImageLoadCommand { get; set; }
        public DelegateCommand UpdateCommand { get; set; }
        public string _path=string.Empty;
        public string PathAvatar { get => this._path;set { this.SetProperty(ref this._path, value); } }

        private string _name, _email, _phone, _pass,_address,_id,_code;


        public string Code { get => this._code; set => this.SetProperty(ref this._code, value); }
        public string Name { get => this._name; set => this.SetProperty(ref this._name, value); }
        public string Email { get => this._email; set => this.SetProperty(ref this._email, value); }
        public string Phone { get => this._phone; set => this.SetProperty(ref this._phone, value); }
        public string Pass { get => this._pass; set => this.SetProperty(ref this._pass, value); }
        public string Addr { get => this._address; set => this.SetProperty(ref this._address, value); }
        public string IDCARD { get => this._id; set => this.SetProperty(ref this._id, value); }
        private readonly IPageDialogService pageDialogService;
        public readonly IHttpSevices HTTP;
        public ProfileViewModel(INavigationService navigationService, IHttpSevices _http, IPageDialogService _pageDialogService) : base(navigationService)
        {
            HTTP = _http;
            this.pageDialogService = _pageDialogService;
            if (HTTP.User != null)
            {
                this.Name = HTTP.User.Name ?? "";
                this.Email = HTTP.User.Email ?? "";
                this.Phone = HTTP.User.Phone ?? "";
                this.PathAvatar = HTTP.User.Avatar ?? "";
                this.Addr = HTTP.User.Address ?? "";
                this.IDCARD = HTTP.User.Cmnd ?? "";
                this.Code = HTTP.User.Code ?? "";
            }
            ImageLoadCommand = new DelegateCommand(ImageLoadCommandExcute);
            UpdateCommand = new DelegateCommand(SubmitDataExcute);
        }
        private async void SubmitDataExcute()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                 this.Popup.Show(content: "Họ và tên không được để trống");
                return;
            }
            if (!string.IsNullOrEmpty(this.Pass) && this.Pass.Length < 6)
            {
               this.Popup.Show(content: "Mật khẩu quá ngắn. Hãy nhập mật khẩu tối thiểu 6 ký tự");
                return;
            }
            if (string.IsNullOrEmpty(this.Email))
            {
                 this.Popup.Show(content: "Email không được để trống");
                return;
            }
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(this.Email);
            if (!match.Success)
            {
                 this.Popup.Show(content: "Email không đúng định dạng");
                return;
            }
            if (string.IsNullOrEmpty(this.IDCARD))
            {

            }
            User u = (User)HTTP.User.Clone();
            u.Name = this.Name;
            u.Pasword = this.Pass;
            u.Email = this.Email;
            u.Phone = this.Phone;
            u.Cmnd = this.IDCARD;
            u.Address = this.Addr;
            if (this.PathAvatar != u.Avatar)
            {
                u.Avatar = this.PathAvatar;
            }
            else
            {
                u.Avatar = string.Empty;
            }
            IsLoading = true;
            var oResult = await HTTP.UpdateAsync(u);
            IsLoading = false;
            if (oResult.result)
            {
                 this.Popup.Show(content: "Cập nhật thông tin thành công ",title:"Thành công",red: Xamarin.Forms.Color.FromHex("#38c1f3"));
                HTTP.User = u;
            }
            else
            {
                 this.Popup.Show(content: "Cập nhật thông tin không thành công ");
            }
        }

        public async void ImageLoadCommandExcute()
        {
            FileData filedata = await CrossFilePicker.Current.PickFile();
            if (filedata == null)
                return;
            if (filedata.FileName.EndsWith("jpg", StringComparison.Ordinal) || filedata.FileName.EndsWith("png", StringComparison.Ordinal))
            {
                PathAvatar = filedata.FilePath;
            }
        }
    }
}
