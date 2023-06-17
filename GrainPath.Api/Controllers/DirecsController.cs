using System.Net.Mime;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Application.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GrainPath.Api.Controllers;

[ApiController]
[Route("api/v1/direcs")]
public sealed class DirecsController : ControllerBase
{
    private readonly IAppContext _context;
    private readonly ILogger<DirecsController> _logger;

    public DirecsController(IAppContext context, ILogger<DirecsController> logger)
    {
        _context = context; _logger = logger;
    }

    /// <summary>
    /// Get a list of alternative directions passing through sequence of points
    /// in the given order.
    /// </summary>
    /// <remarks>
    ///     POST /direcs
    ///     {
    ///         "waypoints": [
    ///             { "lon": 0.0, "lat": 0.0 }
    ///             { "lon": 1.0, "lat": 1.0 }
    ///         ]
    ///     }
    /// </remarks>
    /// <response code="200">Returns object with a (possibly empty) list of directions.</response>
    /// <response code="400">Request structure does not comply with the specification.</response>
    /// <response code="500">Some of the backend services malfunction.</response>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DirecsResponse>> PostAsync(DirecsRequest request)
    {
        // The length is verified by [MinLength] attribute defined on the type.

        var (direcs, err) = await DirecsHandler.Handle(_context.Engine, request.waypoints);

        if (err is not null) { _logger.LogError(err.Message); return StatusCode(500); }

        return new DirecsResponse() { direcs = direcs };
    }
}
