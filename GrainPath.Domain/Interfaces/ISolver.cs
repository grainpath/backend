using System.Collections.Generic;
using GrainPath.Domain.Entities;

namespace GrainPath.Domain.Interfaces;

public interface ISolver
{
    List<Route> Solve(IReadOnlyList<string> keywords, IReadOnlyList<Poi> pois, DistanceMatrix matrix, double distance);
}
