using System.Net.Mime;
using GrainPath.Application.Entities;
using GrainPath.Application.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GrainPath.Api.Controllers;

[ApiController]
[Route("api/v1/autocs")]
public sealed class AutocsController : ControllerBase
{
    private readonly IAppContext _context;
    private readonly ILogger<AutocsController> _logger;

    public AutocsController(IAppContext context, ILogger<AutocsController> logger)
    {
        _context = context; _logger = logger;
    }

    /// <summary>
    /// Get a list of autocomplete items based on count and prefix.
    /// </summary>
    /// <remarks>
    ///     POST /autocs
    ///     {
    ///         "count": 3
    ///         "prefix": "mus"
    ///     }
    /// </remarks>
    /// <response code="200">Returns object with a (possibly empty) list of items.</response>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<AutocsResponse> Post(AutocsRequest request)
    {
        return new AutocsResponse()
        {
            items = AutocsHandler.Handle(_context.Autocs, request.prefix, request.count)
        };
    }
}
