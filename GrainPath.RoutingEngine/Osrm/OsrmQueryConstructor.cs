using System.Collections.Generic;
using System.Linq;
using GrainPath.Application.Entities;

namespace GrainPath.RoutingEngine.Osrm;

internal static class OsrmQueryConstructor
{
    private static string chain(List<WgsPoint> waypoints)
        => string.Join(';', waypoints.Select(w => w.lon.ToString() + ',' + w.lat.ToString()));

    /// <summary>
    /// Fetch fastest path connecting waypoints in a given order.
    /// </summary>
    public static string Route(string addr, List<WgsPoint> waypoints)
        => addr + "/route/v1/foot/" + chain(waypoints) + "?geometries=geojson&skip_waypoints=true";

    /// <summary>
    /// Fetch matrix with durations of the <b>fastest</b> routes between all
    /// pairs of waypoints.
    /// </summary>
    public static string Table(string addr, List<WgsPoint> waypoints)
        => addr + "/table/v1/foot/" + chain(waypoints) + "?annotations=duration&skip_waypoints=true";
}
