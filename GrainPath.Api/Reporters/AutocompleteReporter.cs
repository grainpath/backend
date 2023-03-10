namespace GrainPath.Api.Reporters;

public static class AutocompleteReporter
{
    public static string Report404(string index) => $"Requested index \"{index}\" does not exist.";
}
