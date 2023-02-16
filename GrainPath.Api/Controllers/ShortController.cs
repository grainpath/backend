using System.Net.Mime;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Application.Interfaces;
using GrainPath.Application.RequestHandlers;
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

    [HttpPost(Name = "GetShortestPath")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<ShortResponse>> PostAsync(ShortRequest request)
    {
        var obj = await ShortRequestHandler.Handle(_context.Engine, request);

        if (obj.status != RoutingEngineStatus.OK) { _logger.LogError(obj.message); }

        return obj.status switch
        {
            RoutingEngineStatus.OK => Ok(obj.response),
            RoutingEngineStatus.BR => BadRequest("Cannot find the shortest path for the given configuration."),
            _                      => StatusCode(500, "Service is temporarily unavailable. Try again later.")
        };
    }
}
