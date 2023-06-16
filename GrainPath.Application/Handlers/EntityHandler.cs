using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Application.Interfaces;

namespace GrainPath.Application.Handlers;

/// <summary>
/// <c>/entity</c> request handler.
/// </summary>
public static class EntityHandler
{
    /// <summary>
    /// Get entity by id.
    /// </summary>
    public static Task<Entity> Handle(IModel model, string grainId)
        => model.GetEntity(grainId);
}
