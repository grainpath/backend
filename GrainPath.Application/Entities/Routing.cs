using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GrainPath.Application.Entities;

public sealed class ShortestPathObject
{
    /// <summary>
    /// Distance of the route in <b>kilometres</b>.
    /// </summary>
    [Required]
    [Range(0, double.MaxValue)]
    public double? distance { get; set; }

    /// <summary>
    /// Duration of the route in <b>minutes</b>
    /// </summary>
    [Required]
    [Range(0, double.MaxValue)]
    public double? duration { get; set; }

    /// <summary>
    /// Ordered sequence of points representing connected linestring.
    /// </summary>
    [Required]
    public List<WebPoint> polyline { get; set; }
}

public sealed class DistanceMatrixObject
{
    // TODO: complete distance matrix object
}
