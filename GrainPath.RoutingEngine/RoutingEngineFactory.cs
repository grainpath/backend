using GrainPath.Application.Interfaces;
using GrainPath.RoutingEngine.Osrm;
using GrainPath.RoutingEngine.Valhalla;

namespace GrainPath.RoutingEngine;

public static class OsrmRoutingEngineFactory
{
    private static readonly string _addr;

    static OsrmRoutingEngineFactory()
    {
        _addr = System.Environment.GetEnvironmentVariable("GRAINPATH_REO_ADDR");
    }

    public static IRoutingEngine GetInstance() => new OsrmRoutingEngine(_addr);
}

internal static class ValhallaRoutingEngineFactory
{
    private static readonly string _addr;

    static ValhallaRoutingEngineFactory()
    {
        _addr = System.Environment.GetEnvironmentVariable("GRAINPATH_REV_ADDR");
    }

    public static IRoutingEngine GetInstance() => new ValhallaRoutingEngine(_addr);
}

public static class RoutingEngineFactory
{
    public static IRoutingEngine GetInstance() => OsrmRoutingEngineFactory.GetInstance();
}
