using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace GoogleMaps.TimeZoneServices
{
  public class GoogleTimeZoneServices : ITimeZoneServices
  {

    #region Constants
    const string API_TIMEZONE_FROM_LATLONG = "maps.googleapis.com/maps/api/timezone/xml?location={0},{1}&timestamp={2}";
    #endregion


    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="GoogleTimeZoneServices"/> class. 
    /// </summary>
    /// <param name="useHttps">Indicates whether to call the Google API over HTTPS or not.</param>
    public GoogleTimeZoneServices(bool useHttps)
    {
      APIKey = "";
      UseHttps = useHttps;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GoogleTimeZoneServices"/> class. Default calling the API over regular
    /// HTTP (not HTTPS).
    /// </summary>
    public GoogleTimeZoneServices()
      : this(false)
    {
      APIKey = "";
    }

    public GoogleTimeZoneServices(string apikey)
    {
      APIKey = apikey;
      UseHttps = true;
    }

    #endregion


    #region Properties

    /// <summary>
    /// Gets a value indicating whether to use the Google API over HTTPS.
    /// </summary>
    /// <value>
    /// <c>true</c> if using the API over HTTPS; otherwise, <c>false</c>.
    /// </value>
    public bool UseHttps { get; private set; }

    private string APIKey { get; set; }

    private string UrlProtocolPrefix
    {
      get
      {
        return UseHttps ? "https://" : "http://";
      }
    }


    protected string APIUrlTimeZoneFromLatLong
    {
      get
      {
        return UrlProtocolPrefix + API_TIMEZONE_FROM_LATLONG;
      }
    }

    #endregion


    #region Public instance methods

    /// <summary>
    /// Translates a Latitude / Longitude into a TimeZone using Google Maps Time Zone api
    /// Does not consider Daylight saving time
    /// </summary>
    /// <param name="latitude">Location latitude</param>
    /// <param name="longitude">Location longitude</param>
    /// <returns></returns>
    public TimeZone GetTimeZoneFromLatLong(double latitude, double longitude)
    {
      return GetTimeZoneWithDstFromLatLong(latitude, longitude, new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
    }

    /// <summary>
    /// Translates a Latitude / Longitude into a TimeZone using Google Maps Time Zone api
    /// Includes the timezone offset and daylight saving time offset
    /// </summary>
    /// <param name="latitude">location latitude</param>
    /// <param name="longitude">location longitude</param>
    /// <param name="dstTime">Time of the offset (can vary beause of daylight saving time</param>
    /// <returns></returns>
    public TimeZone GetTimeZoneWithDstFromLatLong(double latitude, double longitude, DateTime dstTime)
    {
      var timeStamp = CalcTimeStamp(dstTime);
      XDocument doc = XDocument.Load(string.Format(APIUrlTimeZoneFromLatLong, latitude, longitude, timeStamp) + "&key=" + APIKey);

      var els = doc.Descendants("TimeZoneResponse").FirstOrDefault();

      string status = els.Descendants("status").FirstOrDefault().Value;
      if (status != "OK")
      {
        throw new System.Net.WebException(string.Format("Request responed with error '{0}'", status));
      }

      if (null != els)
      {
        return new TimeZone() {
          DstOffset = ParseUS(els.Descendants("dst_offset").First().Value),
          RawOffset = ParseUS(els.Descendants("raw_offset").First().Value),
          TimeZoneId = els.Descendants("time_zone_id").First().Value,
          TimeZoneName = els.Descendants("time_zone_name").First().Value,
        };
      }
      return null;
    }

    #endregion

    /// <summary>
    /// Calculates the timestamp in seconds after 1/1/1970
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private double CalcTimeStamp(DateTime time)
    {
      var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      return time.Subtract(epoch).TotalSeconds;
    }

    private double ParseUS(string value)
    {
      return Double.Parse(value, new CultureInfo("en-US"));
    }
  }
}

