using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Prism.Commands;

namespace DuAnHoangGia.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        public DelegateCommand ImageLoadCommand { get; set; }
        public ProfileViewModel(INavigationService navigationService) : base(navigationService)
        {
            ImageLoadCommand = new DelegateCommand(ImageLoadCommandExcute);
        }

        public void ImageLoadCommandExcute()
        {

        }
    }
}
