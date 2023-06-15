using GrainPath.Application.Interfaces;
using GrainPath.RoutingEngine.Osrm;

namespace GrainPath.RoutingEngine;

internal static class OsrmRoutingEngineFactory
{
    private static readonly string _addr;

    static OsrmRoutingEngineFactory()
    {
        _addr = System.Environment.GetEnvironmentVariable("GRAINPATH_RE_ADDR");
    }

    public static IRoutingEngine GetInstance() => new OsrmRoutingEngine(_addr);
}

/// <summary>
/// Simple Factory for a default routing machine.
/// </summary>
public static class RoutingEngineFactory
{
    public static IRoutingEngine GetInstance() => OsrmRoutingEngineFactory.GetInstance();
}
