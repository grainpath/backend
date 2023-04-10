using System.Collections.Generic;
using System.Threading.Tasks;
using GrainPath.Application.Entities;

namespace GrainPath.Application.Interfaces;

public interface IModel
{
    /// <summary>
    /// Construct an index used for autocomplete functionality.
    /// </summary>
    public AutocsIndex GetAutocs();

    /// <summary>
    /// Get valid bounds for a certain conditions.
    /// </summary>
    public BoundsObject GetBounds();

    /// <summary>
    /// Fetch entity (place with attributes) by id.
    /// </summary>
    /// <param name="placeId">Id as per stored in the database.</param>
    public Task<Entity> GetEntity(string placeId);

    /// <summary>
    /// Find places around a point satisfying specific conditions.
    /// </summary>
    /// <param name="center">Geodetic point on Earth.</param>
    /// <param name="radius">Radius of a circle around a point (in meters).</param>
    /// <param name="conditions">Specific conditions.</param>
    public Task<List<SelectedPlace>> GetAround(WgsPoint center, double radius, List<KeywordCondition> conditions);

    /// <summary>
    /// Find places within a given polygon, and satisfying specific conditions.
    /// </summary>
    /// <param name="polygon">Closed polygon.</param>
    /// <param name="conditions">Specific conditions.</param>
    public Task<List<SelectedPlace>> GetNearestWithin(List<WgsPoint> polygon, WgsPoint centroid, double radius, List<KeywordCondition> conditions);
}
