using Autofac;
using Shiftv.Contracts.Data.Stats;
using Shiftv.Contracts.Domain.Stats;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class StatsDtoFactory
    {
        public static IStatistics Create(StatsDto statsDto)
        {
            var stats = Ioc.Container.Resolve<IStatistics>();
            stats.Checkins = StatsCheckinsDtoFactory.Create(statsDto.Checkins);
            stats.Collection = StatsCollectionDtoFactory.Create(statsDto.Collection);
            stats.Comments = StatsCommentsDtoFactory.Create(statsDto.Comments);
            stats.Lists = StatsListsDtoFactory.Create(statsDto.Lists);
            stats.Plays = statsDto.Plays;
            stats.Ratings = RatingDtoFactory.Create(statsDto.Ratings);
            stats.Scrobbles = StatsScrobblesDtoFactory.Create(statsDto.Scrobbles);
            stats.Watchers = statsDto.Watchers;
            return stats;
        }
    }
}