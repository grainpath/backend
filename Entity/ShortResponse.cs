using System.Collections.Generic;

namespace backend.Entity;

public class ShortResponse
{
    public double distance { get; set; }

    public List<WebPoint> route { get; set; }
}
