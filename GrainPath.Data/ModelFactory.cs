using System;
using GrainPath.Application.Interfaces;
using GrainPath.Data.MongoDb;
using MongoDB.Driver;

namespace GrainPath.Data;

internal static class MongoDbModelFactory
{
    private static readonly MongoClient _client;

    static MongoDbModelFactory()
    {
        var conn = Environment.GetEnvironmentVariable(MongoDbConst.CONNECTION);
        _client = new MongoClient(new MongoUrl(conn));
    }

    public static IModel GetInstance()
    {
        try
        {
            return new MongoDbModel(_client.GetDatabase(MongoDbConst.DATABASE));
        }
        catch { throw new Exception("Failed to get database instance from the given connection string."); }
    }
}

public static class ModelFactory
{
    public static IModel GetInstance() => MongoDbModelFactory.GetInstance();
}
