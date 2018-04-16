using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace DuAnHoangGia.Helppers
{
    public static class MapHelpper
    {
        private const double ApproxEarthRadius = 6371d;
        public static double Haversine(this Position coordinate1, Position coordinate2)
        {
            double latDelta = (coordinate1.Latitude - coordinate2.Latitude) * (Math.PI / 180d);
            double lonDelta = (coordinate1.Longitude - coordinate2.Longitude) * (Math.PI / 180d);

            double a = Math.Sin(latDelta / 2) * Math.Sin(latDelta / 2) +
                       Math.Cos(coordinate1.Latitude * (Math.PI / 180d)) * Math.Cos(coordinate2.Latitude * (Math.PI / 180d)) *
                       Math.Sin(lonDelta / 2) * Math.Sin(lonDelta / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return ApproxEarthRadius * c;
        }

        public static double Spherical(this Position coordinate1, Position coordinate2)
        {
            double d =
                Math.Acos(Math.Sin(coordinate1.Latitude * (Math.PI / 180d)) * Math.Sin(coordinate2.Latitude * (Math.PI / 180d)) +
                          Math.Cos(coordinate1.Latitude * (Math.PI / 180d)) * Math.Cos(coordinate2.Latitude * (Math.PI / 180d)) *
                          Math.Cos(coordinate2.Longitude * (Math.PI / 180d) - coordinate1.Longitude * (Math.PI / 180d)));
            return d * ApproxEarthRadius;
        }

        public static Position CoordFromDistance(this Position start, double bearing, double distance)
        {
            distance = distance / ApproxEarthRadius;
            bearing *= Math.PI / 180d;

            double latStart = start.Latitude.ToRadians();
            double lonStart = start.Longitude.ToRadians();

            var latEnd = Math.Asin(Math.Sin(latStart) * Math.Cos(distance) +
                                  Math.Cos(latStart) * Math.Sin(distance) * Math.Cos(bearing));
            var lonEnd = lonStart + Math.Atan2(Math.Sin(bearing) * Math.Sin(distance) * Math.Cos(latStart),
                                         Math.Cos(distance) - Math.Sin(latStart) * Math.Sin(latEnd));
            lonEnd = (lonEnd + 3 * Math.PI) % (2 * Math.PI) - Math.PI;  // normalise to -180...+180

            return new Position(latEnd.ToDegrees(), lonEnd.ToDegrees());
        }
        public static double ToDegrees(this double d)
        {
            return d * (180d / Math.PI);
        }

        /// <summary>
        /// Converts from degrees to radians.
        /// </summary>
        public static double ToRadians(this double d)
        {
            return d * (Math.PI / 180d);
        }
    }
}
