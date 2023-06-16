using System.Collections.Generic;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Application.Interfaces;

namespace GrainPath.Application.Handlers;

/// <summary>
/// <c>/places</c> request handler.
/// </summary>
public static class PlacesHandler
{
    /// <summary>
    /// Get a list of matching places.
    /// </summary>
    public static Task<List<Place>> Handle(IModel model, WgsPoint center, double radius, List<Category> categories)
    {
        return model.GetAround(center, radius, categories);
    }
}
