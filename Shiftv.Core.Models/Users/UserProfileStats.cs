using Shiftv.Contracts.Domain.Users;

namespace Shiftv.Core.Models.Users
{
    class UserProfileStats : IUserProfileStats
    {
        public int Friends { get; set; }
        public IUserProfileStatsShows Shows { get; set; }
        public IUserProfileStatsEpisodes Episodes { get; set; }
        public IUserProfileStatsMovies Movies { get; set; }
    }
}