namespace GrainPath.Application.Interfaces;

public interface IDistanceMatrix
{
    /// <summary>
    /// Calculate the distance between two points.
    /// </summary>
    /// <param name="fr">index of the 1st point</param>
    /// <param name="to">index of the 2nd point</param>
    /// <returns>Distance in meters.</returns>
    public double Distance(int fr, int to);
}
