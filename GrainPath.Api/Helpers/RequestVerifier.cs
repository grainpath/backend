using System.Collections.Generic;
using System.Linq;
using GrainPath.Application.Entities;
using MongoDB.Bson;

namespace GrainPath.Api.Helpers;

internal static class RequestVerifier
{
    public static bool verify(List<KeywordCondition> conditions)
    {
        // different filters with same keywords are forbidden!
        var p = new HashSet<string>(conditions.Select(c => c.keyword)).Count < conditions.Count;

        // invalid numeric condition
        foreach(var con in conditions) {
            var nums = con.filters.numerics;
            foreach (var ncon in new[] { nums.rank, nums.capacity, nums.minimum_age }) {
                p |= ncon is not null && ncon.max < ncon.min;
            }
        }

        return !p;
    }

    public static bool Verify(PlaceRequest request) => ObjectId.TryParse(request.id, out _);

    public static bool Verify(ShortRequest request) => request.waypoints.Count < 2;

    public static bool Verify(StackRequest request) => verify(request.conditions);

    public static bool Verify(RouteRequest request) => verify(request.conditions);
}
