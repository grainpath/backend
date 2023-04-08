using GrainPath.Domain.Interfaces;
using GrainPath.Domain.Solvers;

namespace GrainPath.Domain;

public static class SolverFactory
{
    public static ISolver GetInstance() => new StandardSolver();
}
