using System.Collections.Generic;
using GrainPath.Application.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace GrainPath.Data.MongoDb.Actions;

internal static class AutocsAction
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

    public static AutocsIndex Act(IMongoDatabase database)
    {
        var doc = database
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
