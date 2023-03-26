using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GrainPath.Application.Entities;

public class KeywordNumericFilter
{
    [Required]
    [Range(0, int.MaxValue)]
    public int min { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int max { get; set; }
}

public class KeywordCollectFilter
{
    [Required]
    public SortedSet<string> includes { get; set; }

    [Required]
    public SortedSet<string> excludes { get; set; }
}

public class KeywordFilters
{
    public object image { get; set; }

    public object description { get; set; }

    public object website { get; set; }

    public object address { get; set; }

    public object payment { get; set; }

    public object email { get; set; }

    public object phone { get; set; }

    public object charge { get; set; }

    public object opening_hours { get; set; }

    public bool? fee { get; set; }

    public bool? delivery { get; set; }

    public bool? drinking_water { get; set; }

    public bool? internet_access { get; set; }

    public bool? shower { get; set; }

    public bool? smoking { get; set; }

    public bool? takeaway { get; set; }

    public bool? toilets { get; set; }

    public bool? wheelchair { get; set; }

    public KeywordNumericFilter rank { get; set; }

    public KeywordNumericFilter capacity { get; set; }

    public KeywordNumericFilter minimum_age { get; set; }

    [MinLength(1)]
    public string name { get; set; }

    public KeywordCollectFilter clothes { get; set; }

    public KeywordCollectFilter cuisine { get; set; }

    public KeywordCollectFilter rental { get; set; }
}

public class KeywordCondition
{
    [Required]
    public string keyword { get; set; }

    [Required]
    public KeywordFilters filters { get; set; }
}
