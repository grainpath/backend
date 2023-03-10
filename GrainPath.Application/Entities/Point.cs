using System.ComponentModel.DataAnnotations;

namespace GrainPath.Application.Entities;

public class WebPoint
{
    [Required]
    [Range(-180.0, 180.0)]
    public double? lon { get; set; }

    [Required]
    [Range(-85.06, 85.06)]
    public double? lat { get; set; }
}
