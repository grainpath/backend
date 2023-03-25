using System.Net.Mime;
using GrainPath.Application.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GrainPath.Api.Controllers;

[ApiController]
[Route("api/v1/bound")]
public sealed class BoundController : ControllerBase
{
    private readonly IAppContext _context;
    private readonly ILogger<BoundController> _logger;

    public BoundController(IAppContext context, ILogger<BoundController> logger)
    {
        _context = context; _logger = logger;
    }

    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<BoundResponse> Post()
    {
        return Ok(_context.Bound);
    }
}
