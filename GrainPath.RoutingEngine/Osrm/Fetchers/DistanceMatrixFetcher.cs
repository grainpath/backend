using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Application.Interfaces;
using GrainPath.RoutingEngine.Osrm.Algorithms;
using GrainPath.RoutingEngine.Osrm.Helpers;

namespace GrainPath.RoutingEngine.Osrm.Fetchers;

internal static class DistanceMatrixFetcher
{
    private static readonly double SPEED_COEFF = 5000.0 / 3600.0;

    private sealed class Answer
    {
        public string code { get; set; }

        public List<List<double>> durations { get; set; }
    }

    /// <summary>
    /// Request distance of the fastest paths between all pairs of waypoints.
    /// <list>
    /// <item>http://project-osrm.org/docs/v5.24.0/api/#responses</item>
    /// <item>http://project-osrm.org/docs/v5.24.0/api/#table-service</item>
    /// </list>
    /// </summary>
    /// <param name="addr">base URL of the service</param>
    /// <param name="waypoints">list of WGS84 points</param>
    /// <returns>distance matrix in meters</returns>
    public static async Task<(IDistanceMatrix, ErrorObject)> Fetch(string addr, List<WgsPoint> waypoints)
    {
        var (b, e) = await QueryExecutor
            .Execute(QueryConstructor.Table(addr, waypoints));

        if (b is null) { return (null, e is null ? null : new() { message = e }); }

        try
        {
            var ans = JsonSerializer.Deserialize<Answer>(b);

            if (ans.code != "Ok" || ans.durations is null) { return (null, null); }

            for (int r = 0; r < ans.durations.Count; ++r)
            {
                for (int c = 0; c < ans.durations.Count; ++c)
                {
                    ans.durations[r][c] *= SPEED_COEFF;
                }
            }

            return (new OsrmDistanceMatrix(ans.durations), null);
        }
        catch (Exception ex) { return (null, new() { message = ex.Message }); }
    }
}
