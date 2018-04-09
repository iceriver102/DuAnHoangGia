using System;
using System.Collections.Generic;
using System.Text;
using Prism.Commands;
using Prism.Navigation;

namespace DuAnHoangGia.ViewModels
{
    public class RegisterViewModel:ViewModelBase
    {
        public DelegateCommand RegisterCommand { get; set; }
        public RegisterViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.RegisterCommand = new DelegateCommand(RegisterCommandExcute);

        }

        private async void RegisterCommandExcute()
        {
            await this.NavigationService.NavigateAsync("app:///Login?appModuleRefresh=OnInitialized");
        }
    }
}
