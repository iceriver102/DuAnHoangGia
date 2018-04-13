using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Prism.Commands;
using System.Diagnostics;
using Plugin.FilePicker.Abstractions;
using Plugin.FilePicker;
using DuAnHoangGia.Sevices;

namespace DuAnHoangGia.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        public DelegateCommand ImageLoadCommand { get; set; }
        public DelegateCommand SubmitData { get; set; }
        public string _path=string.Empty;
        public string PathAvatar { get => this._path;set { this.SetProperty(ref this._path, value); } }

        private string _name, _email, _phone, _pass,_address,_id;

        public string Name { get => this._name; set => this.SetProperty(ref this._name, value); }
        public string Email { get => this._email; set => this.SetProperty(ref this._email, value); }
        public string Phone { get => this._phone; set => this.SetProperty(ref this._phone, value); }
        public string Pass { get => this._pass; set => this.SetProperty(ref this._pass, value); }
        public string Addr { get => this._address; set => this.SetProperty(ref this._address, value); }
        public string IDCARD { get => this._id; set => this.SetProperty(ref this._id, value); }

        public readonly IHttpSevices HTTP;

        public ProfileViewModel(INavigationService navigationService, IHttpSevices _http) : base(navigationService)
        {
            HTTP = _http;
            this.Name = HTTP.User.Name;
            this.Email = HTTP.User.Email;
            this.Phone = HTTP.User.Phone;
            this.PathAvatar = HTTP.User.Avatar;
            this.Addr = HTTP.User.Address;
            this.IDCARD = HTTP.User.Cmnd;
             ImageLoadCommand = new DelegateCommand(ImageLoadCommandExcute);
            SubmitData = new DelegateCommand(SubmitDataExcute);
        }

        private async void SubmitDataExcute()
        {
           
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
