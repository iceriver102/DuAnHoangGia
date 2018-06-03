using DuAnHoangGia.Helppers;
using GoogleApi.Entities.Common;
using GoogleApi.Entities.Maps.Directions.Request;
using GoogleApi.Entities.Maps.Directions.Response;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace DuAnHoangGia.Views.Customs
{
    public class CustomMap : Map
    {
        public CustomMap()
        {
            Instructions = string.Empty;
           // this.RouteCoordinates.CollectionChanged += RouteCoordinates_CollectionChanged;
        }

        public static readonly BindableProperty InstructionsProperty =
         BindablePropertyEx.Create<CustomMap, string>(w => w.Instructions, string.Empty, BindingMode.TwoWay);

        public string Instructions
        {
            get { return (string)GetValue(InstructionsProperty); }
            set { SetValue(InstructionsProperty, value); }
        }

        public void RouteCoordinates_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (this.MapRouteRender != null)
            {
                this.MapRouteRender(Color.FromRgba(255,186,0,255));
            }
        }

        public static readonly BindableProperty RenderRouteTrigerProperty =
         BindablePropertyEx.Create<CustomMap, Pin>(w => w.RenderRouteTriger, null,BindingMode.TwoWay,propertyChanged:(b,o,n)=> {
             if(b is CustomMap m && m.MapRouteRender!=null)
             {
                 if(n is Pin f && f!=null)
                 {
                     m.Pins.Clear();
                     m.Pins.Add(m.RenderRouteTriger);
                     m.MapRouteRender(Color.FromRgba(255, 186, 0, 255));
                 }
             }
         });



        public Pin RenderRouteTriger
        {
            get { return (Pin)GetValue(RenderRouteTrigerProperty); }
            set { SetValue(RenderRouteTrigerProperty, value); }
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
               nameof(CenterPostion),
               typeof(Position),
               typeof(CustomMap),
               new Position(0, 0),
               BindingMode.TwoWay
           );

        public static readonly BindableProperty UserPostionProperty =
           BindableProperty.Create(
               nameof(UserPostion),
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
        } public Position UserPostion
        {
            get
            {
                return (Position)GetValue(UserPostionProperty);
            }
            set
            {
                SetValue(UserPostionProperty, value);
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
        public static readonly BindableProperty isLoaddingProperty =
         BindablePropertyEx.Create<CustomMap, bool>(w => w.isLoadding,false, BindingMode.TwoWay);
        public bool isLoadding {
            get
            {
                return (bool)GetValue(isLoaddingProperty);
            }
            set
            {
                SetValue(isLoaddingProperty, value);
            }
        }

        public void MoveTo(Position to)
        {
            double d= APostion.Haversine(to);
            if (d > 1 && !isLoadding)
            {
                APostion = to;
                if (this.MapLoadingCommand != null)
                {
                    isLoadding = true;
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

        public static readonly BindableProperty RenderPinTrigerProperty =
       BindablePropertyEx.Create<CustomMap, bool>(w => w.RenderPinTriger, false, BindingMode.TwoWay, propertyChanged: (b, o, n) => {
           if (b is CustomMap m && m.MapRouteRender != null)
           {
               m.Pins.Clear();
               if (n is bool f && f)
               {
                   if(m.CleanRoute!=null)
                        m.CleanRoute();
                   foreach (var p in m.CPins)
                   {
                       m.Pins.Add(p);
                   }
               }
           }
       });

        public async void RouteTo(Pin from, Pin to)
        {
            this.RenderPinTriger = false;
            if (RouteCoordinates == null)
            {
                RouteCoordinates = new ObservableCollection<Position>();
            }
            else
            {
                RouteCoordinates.Clear();
            }
            this.Instructions = string.Empty;
            Position a = UserPostion;
            if (from != null)
            {
                a = from.Position;
            }
            var request = new DirectionsRequest
            {
                Origin = new Location(a.Latitude,a.Longitude),
                Destination = new Location(to.Position.Latitude, to.Position.Longitude),
                Language= GoogleApi.Entities.Common.Enums.Language.Vietnamese

            };
            Route Route = null;
            try
            {
                var respone = await GoogleApi.GoogleMaps.Directions.QueryAsync(request);
                StringBuilder stringBuilder = new StringBuilder();
                Route = respone.Routes.FirstOrDefault();
                foreach(var Leg in Route.Legs)
                {
                    stringBuilder.Clear();
                    foreach (var s in Leg.Steps)
                    {
                        stringBuilder.Append(Regex.Replace(s.HtmlInstructions, @"<[^>]*>", " "));
                        stringBuilder.Append(". ");
                    }
                }
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex("[ ]{2,}", options);
                this.Instructions = regex.Replace(stringBuilder.ToString(), " ");
                //this.instructions = Regex.Replace(stringBuilder.ToString(), @"<[^>]*>", " ");
            }catch(Exception ex)
            {
                Console.WriteLine(ex.GetBaseException());
            }
            if (Route != null)
            {
                this.Pins.Clear();
                this.Pins.Add(to);
                
                Location m = Route.OverviewPath.Points.LastOrDefault();
                Position center = new Position((m.Latitude + a.Latitude) / 2, (m.Longitude + a.Longitude) / 2);
                foreach (Location l in Route.OverviewPath.Points)
                {
                    RouteCoordinates.Add(new Position(l.Latitude, l.Longitude));
                }
                this.MoveToRegion(MapSpan.FromCenterAndRadius(center, Distance.FromMeters(Route.Legs.FirstOrDefault().Distance.Value / 2)));
                MapRouteRender(Color.FromRgba(255, 186, 0, 255));

            }
        }

        public bool RenderPinTriger
        {
            get { return (bool)GetValue(RenderPinTrigerProperty); }
            set { SetValue(RenderPinTrigerProperty, value); }
        }

        public readonly static BindableProperty CPinsProperty = BindableProperty.Create(nameof(CPins), typeof(ObservableCollection<Pin>), typeof(CustomMap), default(ObservableCollection<Pin>), BindingMode.OneWay);

        public ObservableCollection<Pin> CPins
        {
            get { return (ObservableCollection<Pin>)GetValue(CPinsProperty); }
            set { SetValue(CPinsProperty, value); }
        }

        public readonly static BindableProperty RouteCoordinatesProperty = BindableProperty.Create(nameof(RouteCoordinates), typeof(ObservableCollection<Position>), typeof(CustomMap), default(ObservableCollection<Position>), BindingMode.TwoWay);

        /// <summary>
        /// Gets FlowListView items source.
        /// </summary>
        /// <value>FlowListView items source.</value>
        public ObservableCollection<Position> RouteCoordinates
        {
            get { return (ObservableCollection<Position>)GetValue(RouteCoordinatesProperty); }
            set { SetValue(RouteCoordinatesProperty, value); }
        }
        public Func<Color, bool> MapRouteRender { get; set; }
        public Func<bool> CleanRoute { get; set; }
    }
}

