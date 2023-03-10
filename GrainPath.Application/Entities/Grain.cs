using System.Collections.Generic;

namespace GrainPath.Application.Entities;

public class LightGrain
{
    public class Tags
    {
        public string name { get; set; }
    }

    public Tags tags { get; set; }

    public SortedSet<string> keywords { get; set; }
}
