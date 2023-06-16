using System.Collections.Generic;
using GrainPath.Application.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace GrainPath.Data.MongoDb.Fetchers;

internal static class AutocsFetcher
{
    private sealed class Item
    {
        public long count { get; set; }

        public string label { get; set; }

        public List<string> attributeList { get; set; }
    }

    private sealed class Document
    {
        [BsonId]
        // [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        public List<Item> keywords { get; set; }
    }

    public static AutocsIndex Fetch(IMongoDatabase db)
    {
        var doc = db
            .GetCollection<Document>(MongoDbConst.INDEX_COLLECTION)
            .Find(doc => doc.id == "keywords")
            .FirstOrDefault(); // synchronous!

        var result = new AutocsIndex();

        foreach (var item in doc.keywords)
        {
            result.Add(item.label, item.attributeList, item.count);
        }

        return result;
    }
}
