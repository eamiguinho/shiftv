using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Contracts.Domain.Users
{
    public interface IUserProfileWatching
    {
        string Type { get; set; }
        string Action { get; set; }
        IShow Show { get; set; }
        IEpisode Episode { get; set; }
        IMovie Movie { get; set; }  
    }
}