using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using GrainPath.Api.Helpers;
using GrainPath.Application.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using DirectResponse = GrainPath.Application.Entities.ShortestPathObject;

namespace GrainPath.Api.Controllers;

[ApiController]
[Route("api/v1/direct")]
public sealed class DirectController : ControllerBase
{
    private readonly IAppContext _context;
    private readonly ILogger<DirectController> _logger;

    public DirectController(IAppContext context, ILogger<DirectController> logger)
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
    public async Task<ActionResult<DirectResponse>> PostAsync(DirectRequest request)
    {
        if (!RequestVerifier.Verify(request)) { return BadRequest(); }

        var points = request.waypoints.Select(w => w.AsWgs()).ToList();
        var (s, e) = await _context.Engine.GetShortestPath(points);

        if (e is not null) { _logger.LogError(e.message); return StatusCode(500); }

        return (s is not null) ? Ok(s) : NotFound();
    }
}
