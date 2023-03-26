using System.Collections.Generic;
using System.Threading.Tasks;
using GrainPath.Application.Entities;

namespace GrainPath.Application.Interfaces;

public interface IModel
{
    public AutocIndex GetAutoc();

    public BoundResponse GetBound();

    public Task<HeavyPlace> GetPlace(PlaceRequest request);

    public Task<List<FilteredPlace>> GetStack(StackRequest request);
}
