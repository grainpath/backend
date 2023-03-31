using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using GrainPath.Application.Entities;

namespace GrainPath.RoutingEngine.Osrm.Actions;

internal static class DistanceMatrix
{
    private sealed class Answer
    {
        public string code { get; set; }

        public List<List<double>> durations { get; set; }
    }

    /// <summary>
    /// Find durations of fastest paths between all pairs of points.
    /// </summary>
    /// <returns>Distance matrix in seconds.</returns>
    public static async Task<(DistanceMatrixObject, ErrorObject)> Act(string addr, List<WgsPoint> waypoints)
    {
        /**
         * http://project-osrm.org/docs/v5.24.0/api/#responses
         * http://project-osrm.org/docs/v5.24.0/api/#table-service
         */

        var (b, e) = await RoutingEngineFetcher.GetBody(OsrmQueryConstructor.Table(addr, waypoints));

        if (e is not null) { return (null, new() { message = e }); }

        if (b is null) { return (null, null); }

        try {
            var ans = JsonSerializer.Deserialize<Answer>(b);

            if (ans.code != "Ok") { return (null, null); }

            return (new() { distances = ans.durations }, null);
        }
        catch (Exception ex) { return (null, new() { message = ex.Message }); }
    }
}
