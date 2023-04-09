using System.Collections.Generic;
using System.Linq;
using GrainPath.Application.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace GrainPath.Data.MongoDb.Actions;

internal static class BoundsAction
{
    private sealed class CollectBound
    {
        public int count { get; set; }

        public string label { get; set; }
    }

    private sealed class Bounds
    {
        public List<CollectBound> rental { get; set; }

        public List<CollectBound> clothes { get; set; }

        public List<CollectBound> cuisine { get; set; }

        public NumericBound rank { get; set; }

        public NumericBound capacity { get; set; }

        public NumericBound minimumAge { get; set; }
    }

    private sealed class Document
    {
        [BsonId]
        // [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        public Bounds bounds { get; set; }
    }

    public static BoundsObject Act(IMongoDatabase database)
    {
        var bounds = database
            .GetCollection<Document>(MongoDbConst.INDEX_COLLECTION)
            .Find(doc => doc.id == "bounds")
            .FirstOrDefault() // synchronous!
            .bounds;

        return new()
        {
            rental = bounds.rental.Select(item => item.label).ToList(),
            clothes = bounds.clothes.Select(item => item.label).ToList(),
            cuisine = bounds.cuisine.Select(item => item.label).ToList(),
            rank = bounds.rank,
            capacity = bounds.capacity,
            minimumAge = bounds.minimumAge,
        };
    }
}
