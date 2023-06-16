using System.Net.Mime;
using GrainPath.Application.Entities;
using GrainPath.Application.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

    /// <summary>
    /// Obtain filter bounds.
    /// </summary>
    /// <remarks>
    ///     POST /bounds
    ///     {
    ///     }
    /// </remarks>
    /// <response code="200">Returns response with bounds object.</response>
    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<BoundsResponse> Post(BoundsRequest _)
    {
        return new BoundsResponse()
        {
            bounds = BoundsHandler.Handle(_context.Bounds)
        };
    }
}
