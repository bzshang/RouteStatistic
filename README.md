## RouteStatistic
This repository contains code for a custom AF data reference that calculates statistics from time series GPS coordinates.

It requires AF SDK built against at least .NET Framework 4.0. It uses methods within `System.Linq` and `System.Device.Location` namespaces.

To register the data reference plugin, use `regplugin` under %pihome%\AF\. Example:

```
regplugin.exe \path\to\RouteStatistic.dll /PISystem:MYPISYSTEM
```

The data reference is designed to be used with AF Event Frame attributes. There must be sibling attributes named `Latitude` and `Longitude` that store GPS coordinates in units of degrees.

Time series values for the latitude/longitude pair and the AF Event Frame define a geographic "route", upon which route-based statistics can be calculated.

The data reference returns either the distance traveled, the maximum speed, or minimum speed along the route.

These option is determine by the configuration string, which is of the form:

```
summaryType=Distance|MinSpeed|MaxSpeed
```

The only time context supported is `AFDataReferenceContext.TimeRange` because the RouteStatistic data reference is associated with a time range (defined by the parent AF Event Frame).

The only supported data retrieval method is `AFDataReferenceMethod.GetValue`. Any call to the `GetValue()` method for this data reference must pass in an `AFTimeRange` object or an `InvalidOperationException` is thrown.

Currently, the data reference supports only the following units of measure:

- miles (for distance)
- miles per hour (for minimum and maximum speed)

The data reference itself re-computes the route statistics on each `GetValue()` call on the client-side so caution is advised. However, it is possible to use [AFEventFrame.CaptureValues()](https://techsupport.osisoft.com/Documentation/PI-AF-SDK/html/M_OSIsoft_AF_EventFrame_AFEventFrame_CaptureValues.htm) to cache the values.
