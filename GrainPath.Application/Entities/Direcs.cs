using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GrainPath.Application.Entities;

public sealed class DirecsRequest
{
    /// <summary>
    /// <b>Ordered</b> sequence of waypoints to be visited.
    /// </summary>
    [Required]
    [MinLength(2)]
    public List<WebPoint> waypoints { get; set; }
}

public sealed class DirecsResponse
{
    [Required]
    public List<ShortestPathObject> direcs { get; set; }
}
