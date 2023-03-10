using System.Collections.Generic;
using System.Threading.Tasks;
using GrainPath.Application.Entities;

namespace GrainPath.Application.Interfaces;

public interface IModel
{
    public Task<HeavyPlace> Find(string id);

    public Dictionary<string, AutocompleteIndex> GetAutocomplete();
}
