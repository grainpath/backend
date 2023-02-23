using System.Collections.Generic;
using GrainPath.Application.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace GrainPath.Data.MongoDb.Actions;

internal static class Autocomplete
{
    private sealed class Item
    {
        public long count { get; set; }

        public string value { get; set; }
    }

    private sealed class Document
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        public List<Item> values { get; set; }
    }

    public static Dictionary<string, AutocompleteIndex> Act(IMongoDatabase database)
    {
        var docs = database
            .GetCollection<Document>(MongoDbConst.IndexCollection)
            .Find(Builders<Document>.Filter.Empty)
            .ToList(); // synchronous!

        var result = new Dictionary<string, AutocompleteIndex>();

        foreach (var doc in docs) {
            var comp = new AutocompleteIndex();
            foreach (var item in doc.values) { comp.Add(item.value, item.count); }
            result[doc.id] = comp;
        }

        return result;
    }
}
