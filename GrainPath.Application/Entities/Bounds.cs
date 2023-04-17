using System.ComponentModel.DataAnnotations;

using CollectBound = System.Collections.Generic.List<string>;

namespace GrainPath.Application.Entities;

public sealed class NumericBound
{
    [Required]
    public int min { get; set; }

    [Required]
    public int max { get; set; }
}

public sealed class BoundsObject
{
    [Required]
    public CollectBound clothes { get; set; }

    [Required]
    public CollectBound cuisine { get; set; }

    [Required]
    public CollectBound rental { get; set; }

    [Required]
    public NumericBound rating { get; set; }

    [Required]
    public NumericBound capacity { get; set; }

    [Required]
    public NumericBound minimumAge { get; set; }
}
