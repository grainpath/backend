using System.Threading.Tasks;
using GrainPath.Application.Entities;

namespace GrainPath.Application.Interfaces;

public interface IModel
{
    public AutocIndex GetAutoc();

    public BoundResponse GetBound();

    public Task<HeavyPlace> Find(string id);
}
