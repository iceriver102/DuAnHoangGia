using DuAnHoangGia.Helppers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace DuAnHoangGia.Views.Customs
{
    public class CustomMap : Map
    {
        public CustomMap()
        {
            this.RouteCoordinates = new List<Position>();
        }
        public List<Position> RouteCoordinates
        {
            get;private set;
        }
        public event EventHandler<List<Position>> RenderEvent;

        public void AddRoute(Position p)
        {
            this.RouteCoordinates.Add(p);
            
        }

        internal void Render()
        {
            if (RenderEvent != null)
                RenderEvent(this, this.RouteCoordinates);
        }
    }
}

