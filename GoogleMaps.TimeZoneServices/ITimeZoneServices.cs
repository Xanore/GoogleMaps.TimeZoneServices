using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoogleMaps.TimeZoneServices
{
  public interface ITimeZoneServices
  {
    /// <summary>
    /// Translates a Latitude / Longitude into a TimeZone using Google Maps Time Zone api
    /// Does not consider Daylight saving time
    /// </summary>
    /// <param name="latitude">Location latitude</param>
    /// <param name="longitude">Location longitude</param>
    /// <returns></returns>
    TimeZone GetTimeZoneFromLatLong(double latitude, double longitude);

    /// <summary>
    /// Translates a Latitude / Longitude into a TimeZone using Google Maps Time Zone api
    /// Includes the timezone offset and daylight saving time offset
    /// </summary>
    /// <param name="latitude">location latitude</param>
    /// <param name="longitude">location longitude</param>B
    /// <param name="dstTime">Time of the offset (can vary beause of daylight saving time</param>
    /// <returns></returns>
    TimeZone GetTimeZoneWithDstFromLatLong(double latitude, double longitude, DateTime dstTime);
  }
}

