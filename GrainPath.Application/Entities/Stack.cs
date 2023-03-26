using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GrainPath.Application.Entities;

public sealed class StackRequest
{
    [Required]
    public double? radius { get; set; }

    [Required]
    public WebPoint center { get; set; }

    [Required]
    [MinLength(1)]
    public List<KeywordCondition> conditions { get; set; }
}

public sealed class StackItem
{
    [Required]
    public LightPlace place { get; set; }

    [Required]
    [MinLength(1)]
    public SortedSet<string> fulfill { get; set; }
}
