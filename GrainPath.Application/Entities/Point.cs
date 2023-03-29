using System.ComponentModel.DataAnnotations;
using GrainPath.Domain.Geometry;

namespace GrainPath.Application.Entities;

public sealed class WebPoint
{
    [Required]
    [Range(-180.0, 180.0)]
    public double? lon { get; set; }

    [Required]
    [Range(-85.06, 85.06)]
    public double? lat { get; set; }

    public GeodeticPoint AsGeodetic() => new(lon.Value, lat.Value);

    public CartesianPoint AsCartesian() => new(lon.Value, lat.Value);
}
