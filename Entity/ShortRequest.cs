using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.Entity;

public class ShortRequest
{
    [Required]
    public WebPoint source { get; set; }

    [Required]
    public WebPoint target { get; set; }

    [Required]
    public List<WebPoint> sequence { get; set; }
}
