using System;
using System.Net.Mime;
using System.Threading.Tasks;
using GrainPath.Api.Helpers;
using GrainPath.Application.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using EntityResponse = GrainPath.Application.Entities.Entity;

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

            var grain = await _context.Model.GetEntity(request.placeId);
            return grain is not null ? Ok(grain) : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500);
        }
    }
}
