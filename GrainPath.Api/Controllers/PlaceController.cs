using System;
using System.Net.Mime;
using System.Threading.Tasks;
using GrainPath.Api.Reporters;
using GrainPath.Application.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace GrainPath.Api.Controllers;

[ApiController]
[Route("api/v1/place")]
public sealed class PlaceController : ControllerBase
{
    private readonly IAppContext _context;
    private readonly ILogger<PlaceController> _logger;

    public PlaceController(IAppContext context, ILogger<PlaceController> logger)
    {
        _context = context; _logger = logger;
    }

    [HttpPost(Name = "GetPlace")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<HeavyPlace>> PostAsync(PlaceRequest request)
    {
        try {
            if (!ObjectId.TryParse(request.id, out _)) { return NotFound(); }

            var grain = await _context.Model.Find(request.id);
            return grain is not null ? Ok(grain) : NotFound();
        }
        catch (Exception ex) {
            _logger.LogError(ex.Message);
            return StatusCode(500, PlaceReporter.Report500());
        }
    }
}
