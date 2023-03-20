using System.Collections.Generic;
using GrainPath.Application.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace GrainPath.Data.MongoDb.Actions;

internal static class Autoc
{
    private sealed class Item
    {
        public long count { get; set; }

        public string label { get; set; }

        public List<string> tags { get; set; }
    }

    private sealed class Document
    {
        [BsonId]
        // [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        public List<Item> keywords { get; set; }
    }

    public static AutocIndex Act(IMongoDatabase database)
    {
        var doc = database
            .GetCollection<Document>(MongoDbConst.IndexCollection)
            .Find(doc => doc.id == "keywords")
            .FirstOrDefault(); // synchronous!

        var result = new AutocIndex();

        foreach (var item in doc.keywords) {
            result.Add(item.label, item.tags, item.count);
        }

        return result;
    }
}
