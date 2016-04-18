using Shiftv.Contracts.Domain.Users;

namespace Shiftv.Core.Models.Users
{
    class UserProfileStatsShows : IUserProfileStatsShows
    {
        public int Library { get; set; }
        public int Watched { get; set; }
        public int Collection { get; set; }
        public int Shouts { get; set; }
        public int Loved { get; set; }
        public int Hated { get; set; }
    }
}