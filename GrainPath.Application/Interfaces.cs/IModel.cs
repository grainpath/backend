using System.Collections.Generic;
using GrainPath.Application.Entities;

namespace GrainPath.Application.Interfaces;

public interface IModel
{
    public Dictionary<string, AutocompleteIndex> GetAutocomplete();
}
