using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GrainPath.Application.Entities;

public class PlaceRequest
{
    [Required]
    public string id { get; set; }
}

[BsonIgnoreExtraElements]
public class HeavyPlace
{
    [BsonIgnoreExtraElements]
    public class Tags
    {
        [BsonIgnoreExtraElements]
        public class Address
        {
            public string settlement { get; set; }

            public string postal_code { get; set; }
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<WebPoint> polygon { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string description { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string image { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string website { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Address address { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Linked
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string osm { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string wikidata { get; set; }
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string id { get; set; }

    public string name { get; set; }

    public WebPoint location { get; set; }

    public SortedSet<string> keywords { get; set; }

    public Linked linked { get; set; }

    public Tags tags { get; set; }
}
