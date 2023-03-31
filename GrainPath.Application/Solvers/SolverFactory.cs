namespace GrainPath.Application.Solvers;

internal static class SolverFactory
{
    public static ISolver GetInstance() => new RandomHeuristic();
}
