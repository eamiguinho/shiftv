using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Domain.Users;

namespace Shiftv.Core.Models.Users
{
    class UserProfileWatched : IUserProfileWatched
    {
        public string Type { get; set; }
        public string Action { get; set; }
        public long Watched { get; set; }
        public IShow Show { get; set; }
        public IEpisode Episode { get; set; }
        public IMovie Movie { get; set; }
    }
}