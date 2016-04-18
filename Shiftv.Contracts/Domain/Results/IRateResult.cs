using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Domain.Stats;

namespace Shiftv.Contracts.Domain.Results
{
    public interface IRateResult
    {
        bool Status { get; set; }
        IShow Show { get; set; }
        IMovie Movie { get; set; }

        IEpisode Episode { get; set; }
    }
  
}