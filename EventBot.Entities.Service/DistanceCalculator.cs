using System;
using EventBot.Entities.Service.Models;
using EventBot.Entities.Models;
namespace EventBot.Entities.Service
{
    public static class DistanceCalculator
    {
      
        public static double DistanceTo(this Location baseLocation, LocationModel targetLocation)
        {
            var baseCoordinates = new Coordinates(baseLocation);
            var targetCoordinates = new Coordinates(targetLocation);
            return DistanceTo(baseCoordinates, targetCoordinates, UnitOfLength.Kilometers);
        }
        public static double DistanceTo(this LocationModel baseLocation, LocationModel targetLocation)
        {
            var baseCoordinates = new Coordinates(baseLocation);
            var targetCoordinates = new Coordinates(targetLocation);
            return DistanceTo(baseCoordinates, targetCoordinates, UnitOfLength.Kilometers);
        }

        public static double DistanceTo(this Coordinates baseCoordinates, Coordinates targetCoordinates, UnitOfLength unitOfLength)
        {
            if (baseCoordinates.Latitude == 0 || baseCoordinates.Longitude == 0 || targetCoordinates.Latitude == 0 || targetCoordinates.Longitude == 0) return 0;
            var baseRad = Math.PI * baseCoordinates.Latitude / 180;
            var targetRad = Math.PI * targetCoordinates.Latitude / 180;
            var theta = baseCoordinates.Longitude - targetCoordinates.Longitude;
            var thetaRad = Math.PI * theta / 180;

            double dist =
                Math.Sin(baseRad) * Math.Sin(targetRad) + Math.Cos(baseRad) *
                Math.Cos(targetRad) * Math.Cos(thetaRad);
            dist = Math.Acos(dist);

            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return unitOfLength.ConvertFromMiles(dist);
        }
        public class Coordinates
        {
            public Coordinates(Location location)
            {
                if (location != null)
                {
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                }
            }
            public Coordinates(LocationModel location)
            {
                if (location != null)
                {
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                }
            }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
        public class UnitOfLength
        {
            public static UnitOfLength Kilometers = new UnitOfLength(1.609344);
            public static UnitOfLength NauticalMiles = new UnitOfLength(0.8684);
            public static UnitOfLength Miles = new UnitOfLength(1);

            private readonly double _fromMilesFactor;

            private UnitOfLength(double fromMilesFactor)
            {
                _fromMilesFactor = fromMilesFactor;
            }

            public double ConvertFromMiles(double input)
            {
                return input * _fromMilesFactor;
            }
        }

    }
}
