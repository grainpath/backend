namespace GrainPath.DataModel.MongoDb;

internal static class MongoDbConst
{
    public static string CONNECTION { get; } = "GRAINPATH_DB_CONN";

    public static string DATABASE { get; } = "grainpath";

    public static string GRAIN_COLLECTION { get; } = "grain";

    public static string INDEX_COLLECTION { get; } = "index";
}
