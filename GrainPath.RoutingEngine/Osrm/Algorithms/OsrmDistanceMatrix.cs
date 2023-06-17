using System.Collections.Generic;
using GrainPath.Application.Interfaces;

namespace GrainPath.RoutingEngine.Osrm.Algorithms;

/// <summary>
/// Simple wrapper over List-based distance matrix calculated by OSRM.
/// </summary>
internal sealed class OsrmDistanceMatrix : IDistanceMatrix
{
    private readonly List<List<double>> _matrix;

    public OsrmDistanceMatrix(List<List<double>> matrix) { _matrix = matrix; }

    public double Distance(int fr, int to) => _matrix[fr][to];
}
