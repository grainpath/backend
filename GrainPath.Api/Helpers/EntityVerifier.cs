using GrainPath.Application.Entities;
using MongoDB.Bson;

namespace GrainPath.Api.Helpers;

internal static class EntityVerifier
{
    public static bool Verify(EntityRequest request) => ObjectId.TryParse(request.grainId, out _);
}
