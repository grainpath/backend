using System;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using backend.Entity;
using backend.RequestHandler;
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
        var ine = () => StatusCode(500, "service is temporarily unavailable");
        var nof = () => NotFound("cannot construct the shortest path for the given configuration");

        try {

            var path = await ShortRequestHandler.Handle(request);
            return Ok(new ShortResponse() { distance = path.distance, route = path.route });
        }
        catch (AggregateException ea) {

            var ex = ea.GetBaseException();
            _logger.LogError(ex.Message);
            return (ex as HttpRequestException is not null) ? ine.Invoke() : nof.Invoke();
        }
        catch (HttpRequestException ex) {

            _logger.LogError(ex.Message);
            return ine.Invoke();
        }
        catch (Exception ex) { Console.Write(ex.GetType());

            _logger.LogError(ex.Message);
            return nof.Invoke();
        }
    }
}
