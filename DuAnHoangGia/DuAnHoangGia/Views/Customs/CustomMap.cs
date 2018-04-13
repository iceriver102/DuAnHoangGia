using DuAnHoangGia.Helppers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace DuAnHoangGia.Views.Customs
{
    public class CustomMap : Map
    {
        public CustomMap()
        {

        }
        public static readonly BindableProperty RadiusProperty =
          BindablePropertyEx.Create<CustomMap, float>(w => w.Radius, 5);

        public float Radius
        {
            get { return (float)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        //10.772141, 106.698262
        public static readonly BindableProperty FollowPostionProperty =
           BindableProperty.Create(
               nameof(FollowPostion),
               typeof(Position),
               typeof(CustomMap),
               new Position(10.772141, 106.698262),
               BindingMode.TwoWay,
              propertyChanged: (bindable, oldValue, newValue) =>
              {
                  if (newValue is Position p && oldValue != newValue)
                  {
                      CustomMap map = ((CustomMap)bindable);
                      map.MoveToRegion(MapSpan.FromCenterAndRadius(p, Distance.FromKilometers(map.Radius)));
                  }
              }
           );


        public Position FollowPostion
        {
            get
            {
                return (Position)GetValue(FollowPostionProperty);
            }
            set
            {
                SetValue(FollowPostionProperty, value);
            }
        }
        //public static readonly BindableProperty RouteCoordinatesProperty =
        //  BindablePropertyEx.Create<CustomMap, IEnumerable<Position>>(w => w.RouteCoordinates, null);


        public static BindableProperty FlowItemsSourceProperty = BindableProperty.Create(nameof(RouteCoordinates), typeof(ICollection), typeof(CustomMap), default(ICollection<Position>));

        /// <summary>
        /// Gets FlowListView items source.
        /// </summary>
        /// <value>FlowListView items source.</value>
        public ICollection<Position> RouteCoordinates
        {
            get { return (IList<Position>)GetValue(FlowItemsSourceProperty); }
            set { SetValue(FlowItemsSourceProperty, value); }
        }
        public event EventHandler<ICollection<Position>> RenderEvent;

        //public void AddRoute(Position p)
        //{
        //    this.RouteCoordinates.Add(p);

        //}
        internal void Render()
        {
            if (RenderEvent != null)
                RenderEvent(this, this.RouteCoordinates);
        }
    }
}

