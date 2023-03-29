using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GrainPath.Application.Entities;

public sealed class StackRequest
{
    /// <summary>
    /// Coordinates of a pivot point.
    /// </summary>
    [Required]
    public WebPoint center { get; set; }

    /// <summary>
    /// Radius around the center in <b>kilometres</b> (up to 12).
    /// </summary>
    [Required]
    [Range(0, 12)]
    public double? radius { get; set; }

    /// <summary>
    /// Search conditions with a keyword and optional features.
    /// </summary>
    [Required]
    [MinLength(1)]
    public List<KeywordCondition> conditions { get; set; }
}

public sealed class StackResponse
{
    /// <summary>
    /// Places satisfying given conditions.
    /// </summary>
    [Required]
    public List<FilteredPlace> places { get; set; }
}
