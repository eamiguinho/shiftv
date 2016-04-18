using Shiftv.Contracts.Domain.Images;
using Shiftv.Contracts.Domain.Movies;

namespace Shiftv.Contracts.Domain.Peoples
{
    public interface IPerson
    {
        string Name { get; set; }

        IIds Ids { get; set; }

        IImage Images { get; set; }

        string Biography { get; set; }

        string Birthday { get; set; }

        string Death { get; set; }

        string Birthplace { get; set; }

        string Homepage { get; set; }
    }
}