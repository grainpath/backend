using System.Collections.Generic;
using GrainPath.Application.Entities;

namespace GrainPath.Application.Handlers;

/// <summary>
/// <c>/autocs</c> request handler.
/// </summary>
public static class AutocsHandler
{
    /// <summary>
    /// Get a list of autocomplete items.
    /// </summary>
    /// <param name="count">Maximum possible number of fetched items.</param>
    /// <param name="prefix">Keywords must have the passed prefix.</param>
    public static List<AutocItem> Handle(AutocsIndex index, string prefix, int count)
        => index.TopK(prefix, count);
}
