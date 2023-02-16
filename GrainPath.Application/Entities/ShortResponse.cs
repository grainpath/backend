using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GrainPath.Application.Entities;

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
