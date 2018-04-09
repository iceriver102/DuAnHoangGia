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
        protected INavigationService NavigationService { get; }
        public DelegateCommand<string> NavigateCommand { get; set; }
        public DelegateCommand NavigateBack { get; set; }
        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            NavigateBack = new DelegateCommand(NavigateBackExcute);
        }

        public async void NavigateBackExcute()
        {
            await this.NavigationService.GoBackAsync();
        }

        public async void Navigate(string name)
        {
            await NavigationService.NavigateAsync(name);
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