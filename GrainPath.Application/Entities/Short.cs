using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GrainPath.Application.Interfaces;

namespace GrainPath.Application.Entities;

public class ShortRequest
{
    [Required]
    public WebPoint source { get; set; }

    [Required]
    public WebPoint target { get; set; }

    [Required]
    public List<WebPoint> sequence { get; set; }
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
    /// Distance of the route in <b>meters</b>.
    /// </summary>
    [Required]
    [Range(0, double.MaxValue)]
    public double? distance { get; set; }

    /// <summary>
    /// GeoJSON LineString represented as an ordered sequence of points.
    /// </summary>
    [Required]
    public List<WebPoint> route { get; set; }
}
