using GrainPath.Application.Interfaces;

namespace GrainPath.Api;

public interface IAppContext
{
    public IRoutingEngine Engine { get; init; }
}
