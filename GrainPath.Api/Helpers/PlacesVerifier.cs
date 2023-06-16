using GrainPath.Application.Entities;

namespace GrainPath.Api.Helpers;

internal static class PlacesVerifier
{
    public static bool Verify(PlacesRequest request) => CategoryVerifier.Verify(request.categories);
}
