using System.Collections.Generic;
using GrainPath.Application.Entities;

public static class AutocsHandler
{
    /// <summary>
    /// Get a list of autocomplete items.
    /// </summary>
    /// <param name="prefix">Keywords associated with found items must have the passed prefix.</param>
    /// <param name="count">Maximum number of fetched items.</param>
    public static List<AutocItem> GetItems(AutocsIndex index, string prefix, int count)
    {
        return index.TopK(prefix, count);
    }
}
