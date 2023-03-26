using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GrainPath.Application.Entities;

public sealed class StackRequest
{
    /// <summary>
    /// Radius around the center in <b>kilometers</b>.
    /// </summary>
    [Required]
    public double? radius { get; set; }

    /// <summary>
    /// Coordinates of a pivot point.
    /// </summary>
    [Required]
    public WebPoint center { get; set; }

    /// <summary>
    /// Search conditions with a keyword and optional features.
    /// </summary>
    [Required]
    [MinLength(1)]
    public List<KeywordCondition> conditions { get; set; }
}
