using System.Collections.Generic;
using System.Net.Mime;
using GrainPath.Api.Reporters;
using GrainPath.Application.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GrainPath.Api.Controllers;

[ApiController]
[Route("api/v1/autocomplete")]
public sealed class AutocompleteController : ControllerBase
{
    private readonly IAppContext _context;
    private readonly ILogger<AutocompleteController> _logger;

    public AutocompleteController(IAppContext context, ILogger<AutocompleteController> logger)
    {
        _context = context; _logger = logger;
    }

    [HttpPost(Name = "GetAutocompleteList")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<string>> Post(AutocompleteRequest request)
    {
        return _context.Autocomplete.TryGetValue(request.index, out var index)
            ? Ok(index.TopK(request.prefix, request.count))
            : NotFound(AutocompleteReporter.Report404(request.index));
    }
}
