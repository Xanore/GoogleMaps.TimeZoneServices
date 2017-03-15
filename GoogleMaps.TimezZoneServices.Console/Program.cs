using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMaps.TimeZoneServices.Console
{
  class Program
  {
    static void Main(string[] args)
    {

      TzLatLong[] mapPoints = new TzLatLong[]
           {
                new TzLatLong // Amsterdam, Netherlands
                {
                    Latitude=52.3702157,
                    Longitude=4.89516790
                },
                new TzLatLong // London, England
                {
                    Latitude=51.5073509,
                    Longitude=-0.12775830
                },
                new TzLatLong // Mexico city, Mexico
                {
                    Latitude=19.4326077,
                    Longitude=-99.133208
                }
           };

      var gtzs = new GoogleTimeZoneServices();

      // Without Daylight saving time
      System.Console.WriteLine("=== DST not taken in conciduration ===");
      foreach (var mapPoint in mapPoints)
      {
        try
        {
          var timeZone = gtzs.GetTimeZoneFromLatLong(mapPoint.Latitude, mapPoint.Longitude);
          System.Console.WriteLine("TimeZone {0} found for point {1},{2}", timeZone, mapPoint.Latitude, mapPoint.Longitude);
        }
        catch (System.Net.WebException ex)
        {
          System.Console.WriteLine("Google Maps API Error {0}", ex.Message);
        } 

      }

      // With Daylight saving time
      System.Console.WriteLine("=== On 26 march 2017 at 3:01:00 AM (London and Amsterdam should have DST enabled ===");
      foreach (var mapPoint in mapPoints)
      {
        try
        {
          var timeZone = gtzs.GetTimeZoneWithDstFromLatLong(mapPoint.Latitude, mapPoint.Longitude, new DateTime(2017, 3, 26, 3, 1, 0));
          System.Console.WriteLine("TimeZone {0} found for point {1},{2}", timeZone, mapPoint.Latitude, mapPoint.Longitude);
        }
        catch (System.Net.WebException ex)
        {
          System.Console.WriteLine("Google Maps API Error {0}", ex.Message);
        }

      }
      System.Console.ReadLine();

    }
  }
}
