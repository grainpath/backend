using System.Collections.Generic;
using GrainPath.Application.Entities;
using GrainPath.Application.Interfaces;

namespace GrainPath.Application.Algorithms;

/// <summary>
/// Simple wrapper over Haversine formula.
/// </summary>
internal sealed class HaversineDistanceMatrix : IDistanceMatrix
{
    private readonly List<WgsPoint> _points;

    public HaversineDistanceMatrix(List<WgsPoint> points) { _points = points; }

    public double Distance(int fr, int to) => Spherical.HaversineDistance(_points[fr], _points[to]);
}
