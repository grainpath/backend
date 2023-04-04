namespace GrainPath.Domain.Entities;

public sealed class Poi
{
    /// <summary>
    /// Row in a distance matrix.
    /// </summary>
    public int Order { get; }

    public string Keyword { get; private set; }

    public Poi(int row, string keyword) { this.Order = row; this.Keyword = keyword; }

    public void Erase() { Keyword = string.Empty; }
}
