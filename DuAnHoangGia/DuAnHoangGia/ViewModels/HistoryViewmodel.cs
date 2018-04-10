using DuAnHoangGia.Models;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DuAnHoangGia.ViewModels
{
    public class HistoryViewmodel : BindableBase, IDestructible
    {
        public ObservableCollection<HelpModel> Models { get; set; }
        public HistoryViewmodel() 
        {
            Models = new ObservableCollection<HelpModel>();
            for (int i = 0; i <= 10; i++)
            {
                Models.Add(new HelpModel() { Title = $"Công ty {i + 1}", Mota = " mo ta cua cong ty ", Thumbnail = "a1.png", Date = " 09/09/2018" });
            }
        }

        public void Destroy()
        {
            
        }
    }
}
