using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GrainPath.Application.Entities;

public sealed class KeywordFilterExistens
{
    public object image { get; set; }

    public object description { get; set; }

    public object website { get; set; }

    public object address { get; set; }

    public object payment { get; set; }

    public object email { get; set; }

    public object phone { get; set; }

    public object charge { get; set; }

    public object openingHours { get; set; }
}

public sealed class KeywordFilterBooleans
{
    public bool? fee { get; set; }

    public bool? delivery { get; set; }

    public bool? drinkingWater { get; set; }

    public bool? internetAccess { get; set; }

    public bool? shower { get; set; }

    public bool? smoking { get; set; }

    public bool? takeaway { get; set; }

    public bool? toilets { get; set; }

    public bool? wheelchair { get; set; }
}

public sealed class KeywordFilterNumeric
{
    [Required]
    [Range(0, double.MaxValue)]
    public double? min { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public double? max { get; set; }
}

public sealed class KeywordFilterNumerics
{
    public KeywordFilterNumeric rank { get; set; }

    public KeywordFilterNumeric capacity { get; set; }

    public KeywordFilterNumeric minimumAge { get; set; }
}

public sealed class KeywordFilterTextuals
{
    [MinLength(1)]
    public string name { get; set; }
}

public sealed class KeywordFilterCollect
{
    [Required]
    public SortedSet<string> includes { get; set; }

    [Required]
    public SortedSet<string> excludes { get; set; }
}

public sealed class KeywordFilterCollects
{
    public KeywordFilterCollect rental { get; set; }

    public KeywordFilterCollect clothes { get; set; }

    public KeywordFilterCollect cuisine { get; set; }
}

public sealed class KeywordFilters
{
    [Required]
    public KeywordFilterExistens existens { get; set; }

    [Required]
    public KeywordFilterBooleans booleans { get; set; }

    [Required]
    public KeywordFilterNumerics numerics { get; set; }

    [Required]
    public KeywordFilterTextuals textuals { get; set; }

    [Required]
    public KeywordFilterCollects collects { get; set; }
}

public sealed class KeywordCondition
{
    /// <summary>
    /// Consider only objects identified as a <b>keyword</b>.
    /// </summary>
    [Required]
    [MinLength(1)]
    public string keyword { get; set; }

    /// <summary>
    /// Additional filters introduced by a user.
    /// </summary>
    [Required]
    public KeywordFilters filters { get; set; }
}
