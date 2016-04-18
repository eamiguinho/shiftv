using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Shows;
namespace Shiftv.Contracts.Domain.Results
{
    class SetAsSeenResult : ISetAsSeenResult
    {
        public IShow Show { get; set; }
        public IMovie Movie { get; set; }
        public IEpisode Episode { get; set; }
        public bool Success { get; set; }
    }
}