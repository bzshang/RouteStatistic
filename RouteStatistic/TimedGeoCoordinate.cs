using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

using OSIsoft.AF.Time;

namespace RouteStatistic
{
    public class TimedGeoCoordinate
    {
        public AFTime Time { get; set; }
        public GeoCoordinate Geo { get; set; }
    }
}
