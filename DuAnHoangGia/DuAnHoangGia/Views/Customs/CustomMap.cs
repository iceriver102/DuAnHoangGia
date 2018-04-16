using DuAnHoangGia.Helppers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
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
        public static readonly BindableProperty CenterPostionProperty =
           BindableProperty.Create(
               nameof(FollowPostion),
               typeof(Position),
               typeof(CustomMap),
               new Position(0, 0),
               BindingMode.TwoWay
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

        public Position CenterPostion
        {
            get
            {
                return (Position)GetValue(CenterPostionProperty);
            }
            set
            {
                SetValue(CenterPostionProperty, value);
            }
        }
        public Position APostion;

        public void MoveTo(Position to)
        {
            double d= APostion.Haversine(to);
            if (d > 0.5)
            {
                APostion = to;
                if (this.MapLoadingCommand != null)
                {
                    this.MapLoadingCommand.Execute(null);
                }
            }
        }

        public static BindableProperty MapLoadingCommandProperty = BindableProperty.Create(nameof(MapLoadingCommand), typeof(ICommand), typeof(CustomMap), null);

        /// <summary>
        /// Gets or sets FlowLoadingCommand loading execute command.
        /// </summary>
        /// <value>FlowLoadingCommand loading execute command.</value>
        public ICommand MapLoadingCommand
        {
            get { return (ICommand)GetValue(MapLoadingCommandProperty); }
            set { SetValue(MapLoadingCommandProperty, value); }
        }

        //public static readonly BindableProperty RouteCoordinatesProperty =
        //  BindablePropertyEx.Create<CustomMap, IEnumerable<Position>>(w => w.RouteCoordinates, null);


        public readonly static BindableProperty CPinsProperty = BindableProperty.Create(nameof(CPins), typeof(ObservableCollection<Pin>), typeof(CustomMap), default(ObservableCollection<Pin>));

        public ObservableCollection<Pin> CPins
        {
            get { return (ObservableCollection<Pin>)GetValue(CPinsProperty); }
            set { SetValue(CPinsProperty, value); }
        }

        public readonly static BindableProperty FlowItemsSourceProperty = BindableProperty.Create(nameof(RouteCoordinates), typeof(ICollection), typeof(CustomMap), default(ICollection<Position>),propertyChanged:async (b,o,n)=> {

            Debug.WriteLine("Change");
        });

        /// <summary>
        /// Gets FlowListView items source.
        /// </summary>
        /// <value>FlowListView items source.</value>
        public ICollection<Position> RouteCoordinates
        {
            get { return (IList<Position>)GetValue(FlowItemsSourceProperty); }
            set { SetValue(FlowItemsSourceProperty, value); }
        }
        public Func<Position> GetMapCenterLocation { get; set; }

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

