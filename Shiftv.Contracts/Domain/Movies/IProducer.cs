using Shiftv.Contracts.Domain.Peoples;

namespace Shiftv.Contracts.Domain.Movies
{
    public interface IProducer
    {
        string Name { get; set; }
        bool Executive { get; set; }
        IPeopleImage Image { get; set; }
    }
}