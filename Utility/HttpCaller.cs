using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace backend.Utility;

internal static class HttpCaller
{
    internal static async Task<string> Body(string query)
    {
        try {
            var client = new HttpClient();
            using var res = await client.GetAsync(query);

            if (res.StatusCode != System.Net.HttpStatusCode.OK) {
                throw new HttpRequestException($"Http request failed with status code {res.StatusCode}.");
            }

            return await res.Content.ReadAsStringAsync();
        }
        catch (Exception ex) { throw new HttpRequestException(ex.Message); }
    }
}
