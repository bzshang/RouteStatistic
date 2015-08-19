## RouteStatistic
This repository contains the code for a custom AF data reference (RouteStatistic) that calculates route statistics from time series GPS coordinates defined within a time range.

### Preliminaries

It requires AF SDK built against at least .NET Framework 4.0. It uses methods within `System.Linq` and `System.Device.Location` namespaces.

To register the data reference plugin, use `regplugin` under %pihome%\AF\. Example:

```
regplugin.exe \path\to\RouteStatistic.dll /PISystem:MYPISYSTEM
```

The data reference is designed to be used with AF Event Frame attributes. It assumes there are sibling attributes named `Latitude` and `Longitude` that store GPS coordinates in units of degrees.

### Function

Time series values for the latitude/longitude pair and the AF Event Frame define a geographic "route", upon which route-based statistics can be calculated.

The data reference returns either the distance traveled, the maximum speed, or minimum speed along the route.

These options are determine by the data reference's configuration string, which is of the form:

```
summaryType=Distance|MinSpeed|MaxSpeed
```

The only time context supported is `AFDataReferenceContext.TimeRange` because the data reference's values represent summaries over a time range (set by the AF Event Frame).

The only supported data retrieval method is `AFDataReferenceMethod.GetValue`. Any call to the `GetValue()` method for this data reference must pass in an `AFTimeRange` object or an `InvalidOperationException` is thrown. This is done automatically when calling `AFAttribute.GetValue()` from an event frame attribute without specifying the time context.

### Limitations

Currently, the data reference supports only the following units of measure:

- miles (for distance)
- miles per hour (for minimum and maximum speed)

The data reference itself re-computes the route statistics on each `GetValue()` call on the client-side so caution is advised when making frequent repeated calls or when the data density within the event frame time range is high. However, it is possible to use [AFEventFrame.CaptureValues()](https://techsupport.osisoft.com/Documentation/PI-AF-SDK/html/M_OSIsoft_AF_EventFrame_AFEventFrame_CaptureValues.htm) to cache the values persistently for fast subsequent retrieval.
