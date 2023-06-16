using System.Collections.Generic;
using GrainPath.Application.Algorithms;
using GrainPath.Application.Entities;

namespace GrainPath.Api.Helpers;

internal static class RoutesVerifier
{
    private static bool verifyPrecedence(List<PrecedenceEdge> edges, int order)
    {
        var g = new CycleDetector(order);

        foreach (var e in edges)
        {
            var fr = e.fr.Value;
            var to = e.to.Value;

            if (fr == to || fr >= order || to >= order) { return false; }

            g.AddEdge(fr, to);
        }

        return g.Cycle() is null;
    }

    /// <summary>
    /// Verify category list, acyclicity of the precedence list, and absence of loops.
    /// </summary>
    public static bool Verify(RoutesRequest request)
    {
        return CategoryVerifier.Verify(request.categories)
            && verifyPrecedence(request.precedence, request.categories.Count);
    }
}
