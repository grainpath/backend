namespace GrainPath.Data.MongoDb;

internal static class MongoDbConst
{
    public static string CONNECTION { get; } = "GRAINPATH_DBM_CONN";

    public static string DATABASE { get; } = "grainpath";

    public static string GRAIN_COLLECTION { get; } = "grain";

    public static string INDEX_COLLECTION { get; } = "index";

    public static int BUCKET_SIZE { get; } = 20;

    public static int REQUEST_SIZE { get; } = 100;
}
