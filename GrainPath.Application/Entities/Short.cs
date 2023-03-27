using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GrainPath.Application.Interfaces;

namespace GrainPath.Application.Entities;

public class ShortRequest
{
    /// <summary>
    /// Ordered sequence of waypoints to be visited.
    /// </summary>
    [Required]
    [MinLength(2)]
    public List<WebPoint> waypoints { get; set; }
}

public class ShortObject
{
    public ShortResponse response { get; set; }

    public RoutingEngineStatus status { get; set; }

    public string message { get; set; }
}

public class ShortResponse
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
    /// Ordered sequence of points representing linestring.
    /// </summary>
    [Required]
    public List<WebPoint> polyline { get; set; }
}
