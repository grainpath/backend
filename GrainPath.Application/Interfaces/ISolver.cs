using System.Collections.Generic;
using GrainPath.Application.Entities;

internal interface ISolver
{
    List<FilteredPlace> Decide(List<FilteredPlace> places, double distance, List<KeywordCondition> conditions, List<List<double>> matrix);
}
