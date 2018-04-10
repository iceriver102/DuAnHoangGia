using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace DuAnHoangGia.ViewModels
{
    public class FeedBackViewmodel : ViewModelBase
    {
        private int _tabindex = 0;
        public int SelectedIndex
        {
            get => this._tabindex; set
            {
                if (this._tabindex == value)
                    return;
                this._tabindex = value;
                RaisePropertyChanged("SelectedIndex");
                RaisePropertyChanged("FirstTabColor");
                RaisePropertyChanged("SecondTabColor");
            }
        }
        public ObservableCollection<ViewModel> Models { get; set; }
        public DelegateCommand<string> ChangeTabIndex { get; set; }
        public static Color ActiveColor = new Color(67 / 250f, 197 / 250f, 244 / 250f);
        public static Color NotActiveColor = new Color(164 / 250f, 227 / 250f, 1);


        public Color FirstTabColor { get => (SelectedIndex == 0) ? ActiveColor : NotActiveColor; }
        public Color SecondTabColor { get => (SelectedIndex == 1) ? ActiveColor : NotActiveColor; }

        public FeedBackViewmodel(INavigationService navigationService) : base(navigationService)
        {
            this.Models = new ObservableCollection<ViewModel>();
            this.Models.Add(new ViewModel(new DuAnHoangGia.Views.FeedbackView()));
            this.Models.Add(new ViewModel(new DuAnHoangGia.Views.HistoryView()));
            ChangeTabIndex = new DelegateCommand<string>(ChangeTabIndexExcute);
        }

        private void ChangeTabIndexExcute(string index)
        {
            //#43c5f4
            int i = Convert.ToInt32(index);
            if (this.SelectedIndex != i)
            {
                this.SelectedIndex = i;

            }
        }
    }
    public class ViewModel
    {
        public View View { get; set; }

        public ViewModel(View v)
        {
            this.View = v;
        }
    }
}
