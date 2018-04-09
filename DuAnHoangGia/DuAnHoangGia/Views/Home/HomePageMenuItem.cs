using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DuAnHoangGia.Views.Home
{

    public class HomePageMenuItem
    {
        public HomePageMenuItem(int id)
        {
            this.Id = id;
            if (this.Id % 2 == 0)
            {
                this.BackgroundColor = new Color(0, 0, 0, 0);
            }
            else
            {
                this.BackgroundColor = new Color(0.94,0.94,0.94,1);
            }
        }
        public int Id { get; private set; }
        public string Title { get; set; }
        public Color BackgroundColor { get; private set; }
        public string Icon { get; set; }
        public string Navs { get; set; }
    }
}