using System.Net.Mime;
using System.Threading.Tasks;
using backend.Entity;
using backend.RequestHandler;
using backend.RoutingEngine;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace backend.Controllers;

[ApiController]
[Route("api/v1/short")]
public sealed class ShortController : ControllerBase
{
    private readonly ILogger<ShortController> _logger;

    public ShortController(ILogger<ShortController> logger) { _logger = logger; }

    [HttpPost(Name = "GetShortestPath")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<ShortResponse>> PostAsync(ShortRequest request)
    {
        var handle = await ShortRequestHandler.GetHandle(request);

        if (handle.status != RoutingEngineStatus.OK) { _logger.LogError(handle.message); }

        return handle.status switch
        {
            RoutingEngineStatus.OK => Ok(new ShortResponse() { distance = handle.path.distance, route = handle.path.route }),
            RoutingEngineStatus.BR => BadRequest("Cannot find the shortest path for the given configuration."),
            _                      => StatusCode(500, "Service is temporarily unavailable. Try again later.")
        };
    }
}
