using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

internal static class RoutingEngineFetcher
{
    public static async Task<(string, string)> GetBody(string query)
    {
        var report = (HttpStatusCode code) => $"Routing server answered with status code ${code}.";

        HttpResponseMessage res;

        try {
            res = await new HttpClient().GetAsync(query);
        }
        catch (Exception ex) { return (null, ex.Message); }

        if (res.StatusCode == HttpStatusCode.BadRequest) { return (null, null); }

        if (!res.IsSuccessStatusCode) { return (null, report(res.StatusCode)); }

        return (await res.Content.ReadAsStringAsync(), null);
    }
}
