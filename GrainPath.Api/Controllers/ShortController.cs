using System.Net.Mime;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GrainPath.Api.Controllers;

[ApiController]
[Route("api/v1/short")]
public sealed class ShortController : ControllerBase
{
    private readonly IAppContext _context;
    private readonly ILogger<ShortController> _logger;

    public ShortController(IAppContext context, ILogger<ShortController> logger)
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
    public async Task<ActionResult<ShortResponse>> PostAsync(ShortRequest request)
    {
        if (request.waypoints.Count < 2) { return BadRequest(); }

        var obj = await _context.Engine.GetShortestPath(request.waypoints);

        if (obj.status != RoutingEngineStatus.OK) { _logger.LogError(obj.message); }

        return obj.status switch
        {
            RoutingEngineStatus.OK => Ok(obj.response),
            RoutingEngineStatus.NF => NotFound(),
            _                      => StatusCode(500)
        };
    }
}
