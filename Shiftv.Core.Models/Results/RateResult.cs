using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Domain.Stats;

namespace Shiftv.Core.Models.Results
{
    public class RateResult : IRateResult
    {
        public IShow Show { get; set; }
        public IMovie Movie { get; set; }
        public IEpisode Episode { get; set; }
        public bool Status { get; set; }
    }
}