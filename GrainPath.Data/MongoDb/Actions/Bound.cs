using System.Collections.Generic;
using System.Linq;
using GrainPath.Application.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace GrainPath.Data.MongoDb.Actions;

internal static class Bound
{
    private sealed class CollectBound
    {
        public int count { get; set; }

        public string label { get; set; }
    }

    private sealed class Limits
    {
        public List<CollectBound> rental { get; set; }

        public List<CollectBound> clothes { get; set; }

        public List<CollectBound> cuisine { get; set; }

        public NumericBound rank { get; set; }

        public NumericBound capacity { get; set; }

        public NumericBound minimum_age { get; set; }
    }

    private sealed class Document
    {
        [BsonId]
        // [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        public Limits limits { get; set; }
    }

    public static BoundResponse Act(IMongoDatabase database)
    {
        var limits = database
            .GetCollection<Document>(MongoDbConst.IndexCollection)
            .Find(doc => doc.id == "limits")
            .FirstOrDefault() // synchronous!
            .limits;

        return new ()
        {
            rental = limits.rental.Select(item => item.label).ToList(),
            clothes = limits.clothes.Select(item => item.label).ToList(),
            cuisine = limits.cuisine.Select(item => item.label).ToList(),
            rank = limits.rank,
            capacity = limits.capacity,
            minimum_age = limits.minimum_age,
        };
    }
}
