using System.Collections.Generic;

/// <summary>
/// Simple wrapper over distance matrix to avoid undesired modifications.
/// </summary>
public sealed class DistanceMatrix
{
    private List<List<double>> matrix;

    public int Dim { get => matrix.Count; }

    public DistanceMatrix(List<List<double>> matrix) { this.matrix = matrix; }

    public double Distance(int fr, int to) => matrix[fr][to];
}
