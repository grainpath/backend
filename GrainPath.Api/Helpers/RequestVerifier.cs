using System.Collections.Generic;
using System.Linq;
using GrainPath.Application.Entities;

namespace GrainPath.Api.Helpers;

internal static class RequestVerifier
{
    public static bool verify(List<KeywordCondition> conditions)
    {
        // different filters with same keywords are forbidden!
        var p = new HashSet<string>(conditions.Select(c => c.keyword)).Count < conditions.Count;

        // invalid numeric condition
        foreach(var con in conditions) {
            foreach (var ncon in new[] { con.filters.rank, con.filters.capacity, con.filters.minimum_age }) {
                p |= ncon is not null && ncon.max < ncon.min;
            }
        }

        return !p;
    }

    public static bool Verify(StackRequest request) => verify(request.conditions);

    public static bool Verify(RouteRequest request) => verify(request.conditions);
}
