using DuAnHoangGia.Sevices;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DuAnHoangGia.ViewModels
{
    public class FeedBackViewmodel : ViewModelBase
    {
        private FeedBackForm _feed;
        private HistoryViewmodel<Models.HelpModel> _his;

        public FeedBackForm FeedbackForm { get => _feed; set => this.SetProperty(ref this._feed, value); }
        public HistoryViewmodel<Models.HelpModel> HistoryForm { get => _his; set => this.SetProperty(ref this._his, value); }
        private bool _tabindex;
        public bool SelectedIndex
        {
            get => this._tabindex;
            set
            {
                if (this._tabindex == value)
                    return;
                this._tabindex = value;
                this.FeedbackForm.isView = value;
                this.HistoryForm.isView = !value;
                RaisePropertyChanged("SelectedIndex");
                RaisePropertyChanged("FirstTabColor");
                RaisePropertyChanged("SecondTabColor");
            }
        }
        public DelegateCommand<string> ChangeTabIndex { get; set; }

        public static Color ActiveColor = new Color(67 / 250f, 197 / 250f, 244 / 250f);
        public static Color NotActiveColor = new Color(164 / 250f, 227 / 250f, 1);

        public Color FirstTabColor { get => (SelectedIndex) ? ActiveColor : NotActiveColor; }
        public Color SecondTabColor { get => (!SelectedIndex) ? ActiveColor : NotActiveColor; }
        private readonly IHttpSevices HTTP;
        public FeedBackViewmodel(INavigationService navigationService, IHttpSevices _http, IPageDialogService _pageDialogService) : base(navigationService)
        {
            HTTP = _http;
            this.FeedbackForm = new FeedBackForm(_http, _pageDialogService);
            this.HistoryForm = new HistoryViewmodel<Models.HelpModel>(_http);
            this.SelectedIndex = true;
            ChangeTabIndex = new DelegateCommand<string>(ChangeTabIndexExcute);
        }
        private void ChangeTabIndexExcute(string index)
        {
            int i = Convert.ToInt32(index);
            this.SelectedIndex = i == 0;
           
        }
        public  override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            this.HistoryForm.LoadPage();
           

        }
    }

}
