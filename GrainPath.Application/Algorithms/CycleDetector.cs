using System.Collections.Generic;
using System.Linq;

namespace GrainPath.Application.Algorithms;

/// <summary>
/// Detect cycle using standard 3-color recursive procedure.
/// </summary>
public class CycleDetector
{
    private enum Color { W, G, B }

    private class Vertex
    {
        public int pred;
        public Color color;
        public Vertex() { pred = -1; color = Color.W; }
    }

    private int pos = -1;
    private readonly List<Vertex> Vs;
    private readonly List<SortedSet<int>> Es = new();

    private bool cycle(int u)
    {
        Vs[u].color = Color.G;

        foreach (var v in Es[u])
        {
            Vs[v].pred = u;
            switch (Vs[v].color)
            {
                case Color.W: return cycle(v);
                case Color.G:
                    pos = v;
                    return true;
            }
        }

        Vs[u].color = Color.B;
        return false;
    }

    public CycleDetector(int order)
    {
        Vs = Enumerable.Repeat(new Vertex(), order).ToList();
        Es = Enumerable.Repeat(new SortedSet<int>(), order).ToList();
    }

    /// <summary>
    /// Loops are recognized as cycles.
    /// </summary>
    public void AddEdge(int fr, int to) => Es[fr].Add(to);

    public List<int> Cycle()
    {
        for (int v = 0; v < Vs.Count; ++v)
        {
            if (Vs[v].color == Color.W && cycle(v)) { break; }
        }

        List<int> res = null;

        if (pos > -1)
        {
            var cur = pos;
            do
            {
                res.Add(cur);
                cur = Vs[cur].pred;
            } while (cur != pos);
            res.Add(cur);
        }

        return res;
    }
}
