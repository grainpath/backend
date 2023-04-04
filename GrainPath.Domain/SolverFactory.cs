using GrainPath.Domain.Interfaces;

namespace GrainPath.Domain;

public static class SolverFactory
{
    public static ISolver GetInstance() => new StandardSolver();
}
