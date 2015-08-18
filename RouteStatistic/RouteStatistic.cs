using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Device.Location;

using OSIsoft.AF.Asset;
using OSIsoft.AF.Time;
using OSIsoft.AF.Data;
using OSIsoft.AF.PI;
using OSIsoft.AF.UnitsOfMeasure;

namespace RouteStatistic
{
    [Guid("D4CD0BF8-98BD-448B-9E70-4A12801510D5"),
    Serializable(),
    Description("Route Statistic;Calculates route statistics from lat/long coordinates and associated event frame")]
    public class RouteStatistic : AFDataReference
    {
        private string _summaryType;

        private UOM _uomMeter;
        private UOM _uomMPH;

        private const double METER_TO_MILE = 0.000621371;

        public string SummaryType
        {
            get
            {
                return _summaryType;
            }
            set
            {
                if (_summaryType != value)
                {
                    _summaryType = value;
                    SaveConfigChanges();
                }
            }
        }


        public RouteStatistic() : base()
        {
            _summaryType = null;
            

        }

        public override string ConfigString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}={1};", "summaryType", _summaryType);
                return sb.ToString();
            }
            set
            {
                if (ConfigString != value)
                {
                    // reset Properties to defaults
                    SummaryType = "";

                    if (value != null)
                    {
                        var tokens = value.TrimEnd('\r', '\n').Split(';');
                        foreach (var token in tokens)
                        {
                            var keyValue = token.Trim().Split('=');
                            switch (keyValue[0].ToLower())
                            {
                                case "summarytype":
                                    _summaryType = keyValue[1];
                                    break;
                                default:
                                    throw new ArgumentException("Unrecognized configuration setting");
                            }
                        }
                    }
                    SaveConfigChanges();
                }
            }
        }

        #region Get Inputs
        public override AFAttributeList GetInputs(object context)
        {
            AFAttributeList attList = new AFAttributeList();

            attList.Add(this.Attribute.Element.Attributes["Latitude"]);
            attList.Add(this.Attribute.Element.Attributes["Longitude"]);

            _uomMeter = Attribute.PISystem.UOMDatabase.UOMs["meter"];
            _uomMPH = Attribute.PISystem.UOMDatabase.UOMs["mile per hour"];

            return attList;
        }

        #endregion

        public override AFValue GetValue(object context, object timeContext, AFAttributeList inputAttributes, AFValues inputValues)
        {
            
            if (timeContext == null && inputAttributes == null) throw new ArgumentNullException("Time context and/or input attributes cannot be null");

            AFTimeRange timeRange;
            try
            {
                timeRange = (AFTimeRange) timeContext;
            }
            catch (InvalidCastException)
            {
                throw new InvalidCastException("Cannot cast timeContext to AFTimeRange");
            }

            IList<AFValues> vals = inputAttributes.Data.RecordedValues(
                timeRange: timeRange, 
                boundaryType: AFBoundaryType.Inside, 
                filterExpression: "", 
                includeFilteredValues: false, 
                pagingConfig: new PIPagingConfiguration(PIPageType.TagCount, 1000)).ToList();

            if (vals[0].Count != vals[1].Count) throw new InvalidOperationException("Number of lat/long events do not match");

            switch (SummaryType)
            {
                case "Distance":
                    return GetDistance(vals, timeRange);
                case "MaxSpeed":
                    return GetMaxSpeed(vals, timeRange);
                case "MinSpeed":
                    return GetMinSpeed(vals, timeRange);
                default:
                    throw new InvalidOperationException("Invalid summary type");
            }
            

        }

        private AFValue GetDistance(IList<AFValues> vals, AFTimeRange timeRange)
        {
            var timedGeo = GetTimedGeo(vals);

            var differences = GetDifference(timedGeo);

            return new AFValue(differences.Sum(x => x.Value), timeRange.EndTime, _uomMeter);
        }

        private IEnumerable<TimedGeoCoordinate> GetTimedGeo(IList<AFValues> vals)
        {
            AFValues latitudes = vals[0];
            AFValues longitudes = vals[1];

            var timedGeo = latitudes.Zip(longitudes, (latCoor, lonCoor) =>
            {
                double dLat = Convert.ToDouble(latCoor.Value);
                double dLon = Convert.ToDouble(lonCoor.Value);
                return new TimedGeoCoordinate
                {
                    Time = latCoor.Timestamp,
                    Geo = new GeoCoordinate(dLat, dLon)
                };
            });

            return timedGeo;
        }

        private IEnumerable<TimedValue> GetDifference(IEnumerable<TimedGeoCoordinate> timedGeo)
        {

            var differences = timedGeo.Zip(timedGeo.Skip(1), (x, y) =>
            {
                return new TimedValue
                {
                    Time = y.Time,
                    Value = y.Geo.GetDistanceTo(x.Geo)
                };
            });

            return differences;
        }

        private AFValue GetMaxSpeed(IList<AFValues> vals, AFTimeRange timeRange)
        {
            var timedGeo = GetTimedGeo(vals);

            var speedArray = GetSpeedArray(timedGeo);

            double max = speedArray.Max(x => x.Value);

            return new AFValue(max, timeRange.EndTime, _uomMPH);
        }

        private AFValue GetMinSpeed(IList<AFValues> vals, AFTimeRange timeRange)
        {
            var timedGeo = GetTimedGeo(vals);

            var speedArray = GetSpeedArray(timedGeo);

            double max = speedArray.Min(x => x.Value);

            return new AFValue(max, timeRange.EndTime, _uomMPH);
        }

        private IEnumerable<TimedValue> GetSpeedArray(IEnumerable<TimedGeoCoordinate> timedGeo)
        {
        
            var speedArray = timedGeo.Zip(timedGeo.Skip(20), (x, y) =>
            {
                double timeSpan = (y.Time - x.Time).TotalHours;
                double speed = y.Geo.GetDistanceTo(x.Geo) * METER_TO_MILE / timeSpan;

                return new TimedValue
                {
                    Time = y.Time,
                    Value = speed,
                };
            });

            if (speedArray.ToList().Count == 0) throw new InvalidOperationException("Not enough values to get statistic");

            return speedArray;
            

        }

        #region Supported Methods
        public override AFDataReferenceContext SupportedContexts
        {
            get
            {
                return (AFDataReferenceContext.TimeRange);
            }
        }

        public override AFDataReferenceMethod SupportedMethods
        {
            get
            {
                AFDataReferenceMethod supportedMethods =
                    AFDataReferenceMethod.GetValue;
                return supportedMethods;
            }
        }

        public override AFDataMethods SupportedDataMethods
        {
            get
            {
                return AFDataMethods.None;
            }
        }
        #endregion





        public override Type EditorType
        {
            get
            {
                return typeof(RouteStatEditor);
            }
        }



    }
}
