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
[Route("api/v1/places")]
public sealed class PlacesController : ControllerBase
{
    private readonly IAppContext _context;
    private readonly ILogger<PlacesController> _logger;

    public PlacesController(IAppContext context, ILogger<PlacesController> logger)
    {
        _context = context; _logger = logger;
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PlacesResponse>> PostAsync(PlacesRequest request)
    {
        try
        {
            return RequestVerifier.Verify(request)
                ? Ok(await _context.Model.GetAround(request.center.AsWgs(), request.radius.Value, request.conditions))
                : BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500);
        }
    }
}
