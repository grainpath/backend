using System.Collections.Generic;
using System.Linq;
using GrainPath.Application.Entities;

namespace GrainPath.RoutingEngine.Osrm;

internal static class OsrmQueryConstructor
{
    private static string chain(List<WebPoint> waypoints)
        => string.Join(';', waypoints.Select(w => w.lon.ToString() + ',' + w.lat.ToString()));

    public static string Table(string addr, List<WebPoint> waypoints)
        => addr + "/table/v1/foot/" + chain(waypoints) + "?annotations=distance&skip_waypoints=true";

    public static string Route(string addr, List<WebPoint> waypoints)
        => addr + "/route/v1/foot/" + chain(waypoints) + "?geometries=geojson&skip_waypoints=true";
}
