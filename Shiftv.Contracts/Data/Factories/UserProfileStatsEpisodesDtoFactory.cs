using Autofac;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public class UserProfileStatsEpisodesDtoFactory
    {
        public static IUserProfileStatsEpisodes Create(UserProfileStatsEpisodesDto episodesDto)
        {
            var episodes = Ioc.Container.Resolve<IUserProfileStatsEpisodes>();
            episodes.Checkins = episodesDto.Checkins;
            episodes.Hated = episodesDto.Hated;
            episodes.Loved = episodesDto.Loved;
            episodes.Scrobbles = episodesDto.Scrobbles;
            episodes.Seen = episodesDto.Seen;
            episodes.Shouts = episodesDto.Shouts;
            episodes.Watched = episodesDto.Watched;
            return episodes;
        }
    }
}