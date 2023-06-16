using System;
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
[Route("api/v1/places")]
public sealed class PlacesController : ControllerBase
{
    private readonly IAppContext _context;
    private readonly ILogger<PlacesController> _logger;

    public PlacesController(IAppContext context, ILogger<PlacesController> logger)
    {
        _context = context; _logger = logger;
    }

    /// <summary>
    /// Get places of specific categories around a point on the map.
    /// </summary>
    /// <remarks>
    ///     POST /places
    ///     {
    ///         "center": { "lon": 0.0, "lat": 0.0 },
    ///         "radius": 1000
    ///         "categories": [
    ///             {
    ///                 "keyword": "castle",
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
    ///         ]
    ///     }
    /// </remarks>
    /// <response code="200">Returns object with a list of places.</response>
    /// <response code="400">Malformed object passed in the request.</response>
    /// <response code="500">Some of the backend services malfunction.</response>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PlacesResponse>> PostAsync(PlacesRequest request)
    {
        if (!PlacesVerifier.Verify(request)) { return BadRequest(); }

        try
        {
            return new PlacesResponse()
            {
                places = await PlacesHandler.Handle(_context.Model, request.center.AsWgs(), request.radius.Value, request.categories)
            };
        }
        catch (Exception ex) { _logger.LogError(ex.Message); return StatusCode(500); }
    }
}
