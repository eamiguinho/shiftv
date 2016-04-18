using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Contracts.Domain.Users
{
    public interface IUserProfileWatched
    {
        string Type { get; set; }
        string Action { get; set; }
        long Watched { get; set; }
        IShow Show { get; set; }
        IEpisode Episode { get; set; }
        IMovie Movie { get; set; }
    }
}