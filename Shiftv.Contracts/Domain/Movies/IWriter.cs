using Shiftv.Contracts.Domain.Peoples;

namespace Shiftv.Contracts.Domain.Movies
{
    public interface IWriter
    {
        string Name { get; set; }

        string Job { get; set; }

        IPeopleImage Image { get; set; }
    }
}