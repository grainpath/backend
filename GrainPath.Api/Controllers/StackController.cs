using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GrainPath.Api.Controllers;

[ApiController]
[Route("api/v1/stack")]
public sealed class StackController : ControllerBase
{
    private readonly IAppContext _context;
    private readonly ILogger<StackController> _logger;

    public StackController(IAppContext context, ILogger<StackController> logger)
    {
        _context = context; _logger = logger;
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<StackItem>>> PostAsync(StackRequest request)
    {
        try {
            // different filters with same keywords are forbidden!
            var p = new HashSet<string>(request.conditions.Select(c => c.keyword)).Count < request.conditions.Count;

            return (p) ? BadRequest() : Ok(await _context.Model.GetStack(request));
        }
        catch (Exception ex) {
            _logger.LogError(ex.Message);
            return StatusCode(500);
        }
    }
}
