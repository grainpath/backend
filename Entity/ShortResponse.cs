using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.Entity;

public class ShortResponse
{
    [Required]
    [Range(0, double.MaxValue)]
    public double distance { get; set; }

    [Required]
    public List<WebPoint> route { get; set; }
}
