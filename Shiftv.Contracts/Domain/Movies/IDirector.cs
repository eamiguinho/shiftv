using Shiftv.Contracts.Domain.Peoples;

namespace Shiftv.Contracts.Domain.Movies
{
    public interface IDirector
    {
        string Name { get; set; }

        IPeopleImage Image { get; set; }
    }
}