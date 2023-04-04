using System.Collections.Generic;
using System.Collections.Immutable;

namespace GrainPath.Domain.Entities;

public sealed class Route
{
    private SortedSet<int> _selected;

    public DistanceMatrix Matrix { get; }
    public ImmutableArray<int> Sequence { get; set; }

    private Route(DistanceMatrix matrix, ImmutableArray<int> sequence, SortedSet<int> selected)
    {
        Matrix = matrix; Sequence = sequence; this._selected = selected;
    }

    public Route(DistanceMatrix matrix)
    {
        Matrix = matrix;
        _selected = new SortedSet<int>();
        Sequence = new List<int>() { 0, matrix.Dim - 1 }.ToImmutableArray();
    }

    public void Insert(int poiIndex, string keyword, int seqIndex)
    {
        if (_selected.Contains(poiIndex)) { return; }
        Sequence = Sequence.Insert(++seqIndex, poiIndex);
    }
}
