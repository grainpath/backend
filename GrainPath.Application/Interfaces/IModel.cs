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
    /// Get valid bounds for a certain attribute filters.
    /// </summary>
    public BoundsObject GetBounds();

    /// <summary>
    /// Fetch an entity by id.
    /// </summary>
    /// <param name="grainId">Id as per stored in the database.</param>
    public Task<Entity> GetEntity(string grainId);

    /// <summary>
    /// Find places around a point satisfying specific categories.
    /// </summary>
    /// <param name="center">Geodetic point on the Earth.</param>
    /// <param name="radius">The maximum distance from the center (in meters).</param>
    /// <param name="categories">Categories given by the user.</param>
    /// <param name="bucket">Get at most <c>limit</c> places for each category.</param>
    public Task<List<Place>> GetAround(WgsPoint center, double radius, List<Category> categories, int bucket);

    /// <summary>
    /// Find places satisfying specific categories within a polygon close to the centroid.
    /// </summary>
    /// <param name="polygon">Closed polygon (an approximation of a bounding ellipse).</param>
    /// <param name="refPoint">Reference center point (the centroid of the polygon).</param>
    /// <param name="distance">Maximum distance from the reference point (in meters).</param>
    /// <param name="categories">Categories of objects introduced by the user.</param>
    /// <param name="bucket">Get at most <c>limit</c> places for each category.</param>
    public Task<List<Place>> GetAroundWithin(List<WgsPoint> polygon, WgsPoint refPoint, double distance, List<Category> categories, int bucket);
}
