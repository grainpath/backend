using System.Collections.Generic;
using System.Threading.Tasks;
using GrainPath.Application.Entities;

namespace GrainPath.Application.Interfaces;

public interface IModel
{
    /// <summary>
    /// Construct an index used for autocomplete functionality.
    /// </summary>
    public AutocIndex GetAutoc();

    /// <summary>
    /// Get valid bounds for a certain conditions.
    /// </summary>
    public BoundObject GetBound();

    /// <summary>
    /// Fetch place by id.
    /// </summary>
    /// <param name="id">Id as per stored in the database.</param>
    public Task<HeavyPlace> GetPlace(string id);

    /// <summary>
    /// Find places around a point satisfying specific conditions.
    /// </summary>
    /// <param name="center">Geodetic point on Earth.</param>
    /// <param name="radius">Radius of a circle around a point (in meters).</param>
    /// <param name="conditions">Specific conditions.</param>
    public Task<List<FilteredPlace>> GetAround(WgsPoint center, double radius, List<KeywordCondition> conditions);

    /// <summary>
    /// Find places within a given polygon, and satisfying specific conditions.
    /// </summary>
    /// <param name="polygon">Closed polygon.</param>
    /// <param name="conditions">Specific conditions.</param>
    public Task<List<FilteredPlace>> GetNearestWithin(List<WgsPoint> polygon, WgsPoint centroid, double radius, List<KeywordCondition> conditions);
}
