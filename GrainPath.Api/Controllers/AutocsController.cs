using System.Net.Mime;
using GrainPath.Application.Entities;
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

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<AutocsResponse> Post(AutocsRequest request)
    {
        return Ok(new AutocsResponse() { items = _context.Autocs.TopK(request.prefix, request.count) });
    }
}
