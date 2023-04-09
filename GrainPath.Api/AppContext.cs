using GrainPath.Application.Entities;
using GrainPath.Application.Interfaces;

public interface IAppContext
{
    public IModel Model { get; init; }

    public AutocsIndex Autocs { get; init; }

    public BoundsObject Bounds { get; init; }

    public IRoutingEngine Engine { get; init; }
}

public class AppContext : IAppContext
{
    public IModel Model { get; init; }

    public AutocsIndex Autocs { get; init; }

    public BoundsObject Bounds { get; init; }

    public IRoutingEngine Engine { get; init; }
}
