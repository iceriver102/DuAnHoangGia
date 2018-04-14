using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DuAnHoangGia.ViewModels
{
    public class DialogViewModel: BindableBase, IDestructible
    {
        private string _title, _content;
        public string Title { get => this._title; set => this.SetProperty(ref this._title, value); }
        public string Content { get => this._content; set => this.SetProperty(ref this._content, value); }
        private readonly IEventAggregator eventAggregator;
        public DialogViewModel(IEventAggregator _eventAggregator)
        {
            this.eventAggregator = _eventAggregator;
        }

        public void Destroy()
        {
           
        }
    }
}
