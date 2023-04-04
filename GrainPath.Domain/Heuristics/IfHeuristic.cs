using System.Collections.Generic;
using System.Linq;
using GrainPath.Domain.Entities;

namespace GrainPath.Domain.Heuristics;

/// <summary>
/// Infrequent-First Heuristic from https://doi.org/10.1145/1463434.1463449.
/// </summary>
internal static class IfHeuristic
{
    private static readonly int CNT = 5;

    private sealed class KeywordComparer : IComparer<KeyValuePair<string, int>>
    {
        public int Compare(KeyValuePair<string, int> l, KeyValuePair<string, int> r)
            => l.Value.CompareTo(r.Value);
    }

    /// <summary>
    /// Calculate ascending keyword order with respect to frequencies.
    /// </summary>
    private static List<string> KeywordOrder(IReadOnlyList<string> ks, IReadOnlyList<Poi> ps)
    {
        var fs = new Dictionary<string, int>();

        foreach (var k in ks) {
            fs[k] = 0;

            foreach (var p in ps) {
                if (k == p.Keyword) { ++fs[k]; }
            }
        }

        var kv = fs
            .Select(item => KeyValuePair.Create(item.Key, item.Value))
            .ToList();

        kv.Sort(new KeywordComparer());

        return kv.Select(item => item.Key).ToList();
    }

    /// <summary>
    /// Given a certain keyword, find a pair of poi and position for insertion
    /// that gives the smallest distance increase.
    /// </summary>
    private static (int, int, double) Candidate(string keyword, IReadOnlyList<int> sequence, IReadOnlyList<Poi> pois, DistanceMatrix matrix)
    {
        double disIncr = double.MaxValue;
        int poiIndex = -1, seqIndex = -1;

        for (var p = 0; p < pois.Count; ++p) {
            if (keyword == pois[p].Keyword) {
                for (int i = 0; i < sequence.Count - 1; ++i) {
                    var dis_next = matrix.Distance(sequence[i], pois[p].Order)
                                 + matrix.Distance(pois[p].Order, sequence[i + 1]);

                    if (dis_next < disIncr) { poiIndex = p; seqIndex = i; disIncr = dis_next; }
                }
            }
        }

        return (poiIndex, seqIndex, disIncr);
    }

    /// <summary>
    /// Attempt to find several (up to 5) routes.
    /// </summary>
    /// <param name="pois">First and last pois are source and target respectively.</param>
    public static List<Route> Advise(IReadOnlyList<string> keywords, IReadOnlyList<Poi> pois, DistanceMatrix matrix, double distance)
    {
        var rs = new List<Route>();
        var ks = KeywordOrder(keywords, pois);

        while (rs.Count < CNT) {
            var route = new Route(matrix);
            var disPrev = matrix.Distance(0, matrix.Dim - 1);

            // insert only keywords with feasible candidate

            foreach (var keyword in ks) {
                var (poiIndex, seqIndex, disIncr) = Candidate(keyword, route.Sequence, pois, matrix);

                if (poiIndex >= 0) {
                    var disNext = disPrev - matrix.Distance(route.Sequence[seqIndex], route.Sequence[seqIndex + 1]) + disIncr;
                    if (disNext <= distance) {
                        disPrev = disNext;
                        pois[poiIndex].Erase();
                        route.Insert(poiIndex, keyword, seqIndex);
                    }
                }
            }

            rs.Add(route);

            if (route.Sequence.Length == 2) { break; } // no suitable points remained
        }

        return rs;
    }
}
