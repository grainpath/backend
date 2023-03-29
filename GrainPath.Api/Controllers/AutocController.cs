using System.Net.Mime;
using GrainPath.Application.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GrainPath.Api.Controllers;

[ApiController]
[Route("api/v1/autoc")]
public sealed class AutocController : ControllerBase
{
    private readonly IAppContext _context;
    private readonly ILogger<AutocController> _logger;

    public AutocController(IAppContext context, ILogger<AutocController> logger)
    {
        _context = context; _logger = logger;
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<AutocResponse> Post(AutocRequest request)
    {
        return Ok(new AutocResponse() { items = _context.Autoc.TopK(request.prefix, request.count) });
    }
}
