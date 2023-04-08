using System.ComponentModel.DataAnnotations;

namespace GrainPath.Application.Entities;

public sealed class PlaceRequest
{
    [Required]
    public string grainId { get; set; }
}
