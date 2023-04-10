using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GrainPath.Application.Entities;

[BsonIgnoreExtraElements]
public class Place
{
    [BsonId]
    [Required]
    [BsonRepresentation(BsonType.ObjectId)]
    public string grainId { get; set; }

    [Required]
    public string name { get; set; }

    [Required]
    public WgsPoint location { get; set; }

    [Required]
    [MinLength(1)]
    public SortedSet<string> keywords { get; set; }

    [Required]
    public SortedSet<string> selected { get; set; } = new();
}
