using System.Collections.Generic;
using GrainPath.Application.Entities;
using GrainPath.Application.Interfaces;

public interface IAppContext
{
    public IModel Model { get; init; }

    public IRoutingEngine Engine { get; init; }

    public Dictionary<string, AutocompleteIndex> Autocomplete { get; init; }
}
