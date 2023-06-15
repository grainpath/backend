using System;
using System.Net.Mime;
using System.Threading.Tasks;
using GrainPath.Api.Helpers;
using GrainPath.Application.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GrainPath.Api.Controllers;

[ApiController]
[Route("api/v1/entity")]
public sealed class PlaceController : ControllerBase
{
    private readonly IAppContext _context;
    private readonly ILogger<PlaceController> _logger;

    public PlaceController(IAppContext context, ILogger<PlaceController> logger)
    {
        _context = context; _logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    ///     POST /entity
    ///     {
    ///         "grainId": "64878b360b308ed470e2498b"
    ///     }
    /// </remarks>
    /// <response code="200">Returns object with an entity item.</response>
    /// <response code="400">Impossible identifier passed in the request object.</response>
    /// <response code="404">An entity with identifier does not exist.</response>
    /// <response code="500">Some of the backend services malfunction.</response>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<EntityResponse>> PostAsync(EntityRequest request)
    {
        try
        {
            if (!RequestVerifier.Verify(request)) { return BadRequest(); }

            var grain = await EntityHandler.GetEntity(_context.Model, request.grainId);

            return grain is not null ? new EntityResponse() { entity = grain } : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500);
        }
    }
}
