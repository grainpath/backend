using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GrainPath.Application.Entities;

public sealed class DirectRequest
{
    /// <summary>
    /// <b>Ordered</b> sequence of waypoints to be visited.
    /// </summary>
    [Required]
    [MinLength(2)]
    public List<WebPoint> waypoints { get; set; }
}
