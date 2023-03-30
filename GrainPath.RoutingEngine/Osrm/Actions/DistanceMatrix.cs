using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using GrainPath.Application.Entities;

namespace GrainPath.RoutingEngine.Osrm.Actions;

internal static class DistanceMatrix
{
    private static readonly string _prefix = "/table/v1/foot/";
    private static readonly string _suffix = "?annotations=distance&skip_waypoints=true";

    private sealed class Answer
    {
        public string code { get; set; }

        public List<List<double>> distances { get; set; }
    }

    public static async Task<(DistanceMatrixObject, ErrorObject)> Act(string addr, List<WebPoint> waypoints)
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

            return (new() { distances = ans.distances }, null);
        }
        catch (Exception ex) { return (null, new() { message = ex.Message }); }
    }
}
