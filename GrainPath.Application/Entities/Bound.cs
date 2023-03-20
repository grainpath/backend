using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GrainPath.Application.Entities;

public sealed class NumericBound
{
    [Required]
    public int min { get; set; }

    [Required]
    public int max { get; set; }
}

public sealed class BoundResponse
{
    [Required]
    public List<string> clothes { get; set; }

    [Required]
    public List<string> cuisine { get; set; }

    [Required]
    public List<string> rental { get; set; }

    [Required]
    public NumericBound capacity { get; set; }

    [Required]
    public NumericBound min_age { get; set; }

    [Required]
    public NumericBound rank { get; set; }
}
