using Autofac;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public class UserProfileStatsDtoFactory
    {
        public static IUserProfileStats Create(UserProfileStatsDto statsDto)
        {
            var stats = Ioc.Container.Resolve<IUserProfileStats>();
            stats.Friends = statsDto.Friends;
            if (statsDto.Shows != null) stats.Shows = UserProfileStatsShowsDtoFactory.Create(statsDto.Shows);
            if (statsDto.Movies != null) stats.Movies = UserProfileStatsMoviesDtoFactory.Create(statsDto.Movies);
            if (statsDto.Episodes != null) stats.Episodes = UserProfileStatsEpisodesDtoFactory.Create(statsDto.Episodes);
            return stats;
        }
    }
}