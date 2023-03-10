namespace GrainPath.Api.Reporters;

internal static class ShortReporter
{
    public static string Report404() => "Cannot find the shortest path for the given configuration.";

    public static string Report500() => BaseReporter.Report500();
}
