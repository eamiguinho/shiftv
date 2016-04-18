using Autofac;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public class UserProfileStatsShowsDtoFactory
    {
        public static IUserProfileStatsShows Create(UserProfileStatsShowsDto showsDto)
        {
            var shows = Ioc.Container.Resolve<IUserProfileStatsShows>();
            shows.Collection = showsDto.Collection;
            shows.Hated = showsDto.Hated;
            shows.Library = showsDto.Library;
            shows.Loved = showsDto.Loved;
            shows.Shouts = showsDto.Shouts;
            shows.Watched = showsDto.Watched;
            return shows;
        }
    }
}