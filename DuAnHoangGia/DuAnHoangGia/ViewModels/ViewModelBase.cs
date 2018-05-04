using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DuAnHoangGia.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        public PopupViewModel Popup { get; set; }
        protected INavigationService NavigationService { get; }
        public DelegateCommand<string> NavigateCommand { get; set; }
        public DelegateCommand NavigateBack { get; set; }
        private bool _IsLoading = false;
        public bool IsLoading { get => this._IsLoading; set => this.SetProperty(ref this._IsLoading, value); }
        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            NavigateBack = new DelegateCommand(NavigateBackExcute);
            this.Popup = new PopupViewModel();
        }

        public async void NavigateBackExcute()
        {
            await this.NavigationService.GoBackAsync();
        }

        public async void Navigate(string name)
        {
            await NavigationService.NavigateAsync(name, useModalNavigation: true);
        }

        public void Destroy()
        {

        }

        public virtual void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(NavigationParameters parameters)
        {

        }

        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {

        }
    }
}