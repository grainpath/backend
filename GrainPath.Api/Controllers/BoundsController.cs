using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using BoundsResponse = GrainPath.Application.Entities.BoundsObject;

namespace GrainPath.Api.Controllers;

[ApiController]
[Route("api/v1/bounds")]
public sealed class BoundsController : ControllerBase
{
    private readonly IAppContext _context;
    private readonly ILogger<BoundsController> _logger;

    public BoundsController(IAppContext context, ILogger<BoundsController> logger)
    {
        _context = context; _logger = logger;
    }

    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<BoundsResponse> Post()
    {
        return Ok(_context.Bounds);
    }
}
