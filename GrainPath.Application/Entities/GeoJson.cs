using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GrainPath.Application.Entities;

public sealed class GeoJsonPoint
{
    [Required]
    public string type { get; set; } = "Point";

    [Required]
    [MinLength(2)]
    public List<double> coordinates { get; set; }
}
