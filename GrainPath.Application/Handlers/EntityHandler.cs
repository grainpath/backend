using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Application.Interfaces;

public static class EntityHandler
{
    public static Task<Entity> GetEntity(IModel model, string grainId)
        => model.GetEntity(grainId);
}
