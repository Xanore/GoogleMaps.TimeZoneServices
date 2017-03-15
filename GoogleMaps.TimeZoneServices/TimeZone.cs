using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoogleMaps.TimeZoneServices
{
  public class TimeZone
  {

      public double? DstOffset { get; set; }
      public double? RawOffset { get; set; }
      public string TimeZoneId { get; set; }
      public string TimeZoneName { get; set; }


      public override string ToString()
      {
        return String.Format(
            "{0}{1}{2}{3}",
            DstOffset != null ? DstOffset + ", " : "",
            RawOffset != null ? RawOffset + ", " : "",
            TimeZoneId != null ? TimeZoneId + ", " : "",
            TimeZoneName != null ? TimeZoneName + ", " : "").TrimEnd(' ', ',');
      }
    }

}
