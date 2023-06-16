using System.Net.Mime;
using System.Threading.Tasks;
using GrainPath.Api.Helpers;
using GrainPath.Application.Entities;
using GrainPath.Application.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GrainPath.Api.Controllers;

[ApiController]
[Route("api/v1/routes")]
public sealed class RouteController : ControllerBase
{
    private readonly IAppContext _context;
    private readonly ILogger<RouteController> _logger;

    public RouteController(IAppContext context, ILogger<RouteController> logger)
    {
        _context = context; _logger = logger;
    }

    /// <summary>
    /// Get a list of distinct routes passing through categories.
    /// </summary>
    /// <remarks>
    ///     POST /routes
    ///     {
    ///         "source": { "lon": 0.0, "lat": 0.0 },
    ///         "target": { "lon": 0.0, "lat": 0.0 },
    ///         "distance": 1000,
    ///         "categories": [
    ///             {
    ///                 "keyword": "museum",
    ///                 "filters": {
    ///                     "existens": {
    ///                         "image": {}
    ///                     },
    ///                     "booleans": {
    ///                         "fee": false
    ///                     },
    ///                     "numerics": {
    ///                         "year": { "min": 1500, "max": 1599 }
    ///                     },
    ///                     "textuals": {
    ///                         "name": "slav"
    ///                     },
    ///                     "collects": {
    ///                         "cuisine": [ "czech" ]
    ///                     }
    ///                 }
    ///             }
    ///         ],
    ///         "precedence": [
    ///             { "fr": 0, "to": 1 }
    ///         ]
    ///     }
    /// </remarks>
    /// <response code="200">Returns object with a (possibly empty) list of routes.</response>
    /// <response code="400">Request structure does not comply with the specification.</response>
    /// <response code="500">Some of the backend services malfunction.</response>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RoutesResponse>> PostAsync(RoutesRequest request)
    {
        if (!RoutesVerifier.Verify(request)) { return BadRequest(); }

        var (routes, err) = await RoutesHandler.Handle(
            _context.Model, _context.Engine, request.source.AsWgs(),
            request.target.AsWgs(), request.distance.Value, request.categories);

        if (err is not null) { _logger.LogError(err.message); return StatusCode(500); }

        return new RoutesResponse() { routes = routes };
    }
}
