using System.ComponentModel.DataAnnotations;

namespace GrainPath.Application.Entities;

public class PlaceRequest
{
    [Required]
    public string id { get; set; }
}
