using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace GrainPath.RoutingEngine.Osrm.Helpers;

internal static class QueryExecutor
{
    private static string report(HttpStatusCode code) => $"Routing machine answered with status code ${code}.";

    /// <summary>
    /// Executor of OSRM queries.
    /// </summary>
    /// <param name="query">well-formed query</param>
    /// <returns>response body</returns>
    public static async Task<(string, string)> Execute(string query)
    {
        HttpResponseMessage res;

        try
        {
            res = await new HttpClient().GetAsync(query);
        }
        catch (Exception ex) { return (null, ex.Message); }

        if (res.StatusCode == HttpStatusCode.BadRequest) { return (null, null); }

        if (!res.IsSuccessStatusCode) { return (null, report(res.StatusCode)); }

        return (await res.Content.ReadAsStringAsync(), null);
    }
}
