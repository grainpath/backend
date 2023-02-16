using GrainPath.Application.Interfaces;

namespace GrainPath.Api;

public class AppContext : IAppContext
{
    public IRoutingEngine Engine { get; init; }
}
