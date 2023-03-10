namespace GrainPath.Data.MongoDb;

internal static class MongoDbConst
{
    public static string Connection { get; set; } = "GRAINPATH_DBM_CONN";

    public static string Database { get; set; } = "grainpath";

    public static string GrainCollection { get; set; } = "grain";

    public static string IndexCollection { get; set; } = "index";
}
