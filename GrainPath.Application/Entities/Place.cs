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
    public string placeId { get; set; }

    [Required]
    public string name { get; set; }

    [Required]
    public WgsPoint location { get; set; }

    [Required]
    [MinLength(1)]
    public SortedSet<string> keywords { get; set; }
}

[BsonIgnoreExtraElements]
public sealed class FilteredPlace
{
    ///<summary>
    /// Simplified place representation as given in the database.
    ///</summary>
    [Required]
    public Place place { get; set; }

    ///<summary>
    /// Contains a list of filters satisfied by the place.
    ///</summary>
    [Required]
    [MinLength(1)]
    public SortedSet<string> satisfy { get; set; }
}
