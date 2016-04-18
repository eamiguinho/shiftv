using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Shows;
namespace Shiftv.Contracts.Domain.Results
{
    public interface ISetAsSeenResult
    {
        IShow Show { get; set; }
        IMovie Movie { get; set; }
        IEpisode Episode { get; set; }
        bool Success { get; set; }
    }
}