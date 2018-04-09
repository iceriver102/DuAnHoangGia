using System;
using System.Collections.Generic;
using System.Text;
using Prism.Commands;
using Prism.Navigation;

namespace DuAnHoangGia.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public DelegateCommand LoginCommand { get; set; }
        public LoginViewModel(INavigationService navigationService) : base(navigationService) {
            this.LoginCommand = new DelegateCommand(LoginCommandExcute);
        }

        private async void LoginCommandExcute()
        {
           await this.NavigationService.NavigateAsync("Home");
        }
    }
}
