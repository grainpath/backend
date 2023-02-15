namespace backend.RoutingEngine;

static class RoutingEngineFactory
{
    static IRoutingEngine _instance = new OsrmRoutingEngine();

    public static IRoutingEngine GetInstance() => _instance;
}
