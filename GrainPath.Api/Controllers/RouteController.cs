using System.Net.Mime;
using System.Threading.Tasks;
using GrainPath.Api.Helpers;
using GrainPath.Application.Entities;
using GrainPath.Application.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using RouteResponse = GrainPath.Application.Entities.RouteObject;

namespace GrainPath.Api.Controllers;

[ApiController]
[Route("api/v1/route")]
public sealed class RouteController : ControllerBase
{
    private readonly IAppContext _context;
    private readonly ILogger<RouteController> _logger;

    public RouteController(IAppContext context, ILogger<RouteController> logger)
    {
        _context = context; _logger = logger;
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RouteResponse>> PostAsync(RouteRequest request)
    {
        if (!RequestVerifier.Verify(request)) { return BadRequest(); }

        var (route, err) = await RouteFinder.Find(
            _context.Model, _context.Engine, request.source.AsWgs(),
            request.target.AsWgs(), request.distance.Value, request.conditions);

        if (err is not null) { _logger.LogError(err.message); return StatusCode(500); }

        return (route is not null) ? Ok(route) : NotFound();
    }
}
