using Shiftv.Contracts.Domain.Users;

namespace Shiftv.Core.Models.Users
{
    class UserProfileStatsMovies : IUserProfileStatsMovies
    {
        public int Watched { get; set; }
        public int Scrobbles { get; set; }
        public int Checkins { get; set; }
        public int Seen { get; set; }
        public int Shouts { get; set; }
        public int Loved { get; set; }
        public int Hated { get; set; }
    }
}