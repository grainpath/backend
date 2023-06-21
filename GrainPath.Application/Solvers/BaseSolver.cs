using System.Collections.Generic;
using System.Linq;
using GrainPath.Application.Heuristics;

namespace GrainPath.Application.Solvers;

internal class BaseSolver
{
    protected BaseSolver() { }

    protected static List<SolverPlace> FilterPlaces(List<SolverPlace> places, SortedSet<int> occur)
        => places.Where(place => !occur.Contains(place.Index)).ToList();
}
