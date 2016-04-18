using Autofac;
using Shiftv.Contracts.Data.Stats;
using Shiftv.Contracts.Domain.Stats;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class StatsScrobblesDtoFactory
    {
        public static IStatsScrobbles Create(StatsScrobblesDto scrobblesDto)
        {
            var scrobbles = Ioc.Container.Resolve<IStatsScrobbles>();
            scrobbles.All = scrobblesDto.All;
            scrobbles.Users = scrobblesDto.Users;
            return scrobbles;
        }
    }
}