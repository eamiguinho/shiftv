using Autofac;
using Shiftv.Contracts.Data.Stats;
using Shiftv.Contracts.Domain.Stats;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class StatsCheckinsDtoFactory
    {
        public static IStatsCheckins Create(StatsCheckinsDto checkinsDto)
        {
            var checkins = Ioc.Container.Resolve<IStatsCheckins>();
            checkins.All = checkinsDto.All;
            checkins.Users = checkinsDto.Users;
            return checkins;
        }
    }
}