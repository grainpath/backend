using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GrainPath.Application.Entities;

public sealed class RouteRequest
{
    /// <summary>
    /// Starting point.
    /// </summary>
    [Required]
    public WebPoint source { get; set; }

    /// <summary>
    /// Destination point.
    /// </summary>
    [Required]
    public WebPoint target { get; set; }

    /// <summary>
    /// Maximum walking distance in <b>meters</b>.
    /// </summary>
    [Required]
    [Range(0, 30_000)]
    public double? distance { get; set; }

    /// <summary>
    /// Search conditions with a keyword and optional features.
    /// </summary>
    [Required]
    [MinLength(1)]
    public List<KeywordCondition> conditions { get; set; }
}

public sealed class RouteObject
{
    /// <summary>
    /// Connect source, target, and waypoints in-between.
    /// </summary>
    [Required]
    public ShortestPathObject path { get; set; }

    /// <summary>
    /// Ordered sequence of places satisfying given conditions.
    /// </summary>
    [Required]
    public List<FilteredPlace> order { get; set; }
}
