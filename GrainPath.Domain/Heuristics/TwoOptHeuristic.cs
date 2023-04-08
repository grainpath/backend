using System.Collections.Generic;
using GrainPath.Domain.Entities;

namespace GrainPath.Domain.Heuristics;

/// <summary>
/// Implements standard 2-Opt heuristic for TSP on <b>open</b> route. The first
/// and last items are not swappable.
/// </summary>
internal sealed class TwoOptHeuristic
{
    /// <summary>
    /// Cap number of iterations at a reasonably large value.
    /// </summary>
    private static readonly int cap = 128;

    public static List<int> Advise(List<int> sequence, DistanceMatrix matrix)
    {
        bool change;
        int iters = 0;
        var dist = 0.0;

        for (int i = 0; i < sequence.Count - 1; ++i) { dist += matrix.Distance(i, i + 1); }

        do
        {
            ++iters;
            change = false;

            for (int i = 0; i < sequence.Count - 3; ++i)
            {
                for (int j = i + 1; j < sequence.Count - 2; ++j)
                {
                    double diff =
                        - matrix.Distance(sequence[i    ], sequence[i + 1])
                        - matrix.Distance(sequence[j    ], sequence[j + 1])
                        + matrix.Distance(sequence[i    ], sequence[j    ])
                        + matrix.Distance(sequence[i + 1], sequence[j + 1]);
                    if (diff < 0)
                    {
                        dist += diff; change = true; sequence.Reverse(i + 1, j - i);
                    }
                }
            }
        } while (change && iters < cap);

        return sequence;
    }
}
