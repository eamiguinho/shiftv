using Autofac;
using Shiftv.Contracts.Data.Stats;
using Shiftv.Contracts.Domain.Stats;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class GeneralStatsDtoFactory
    {
        public static IGeneralStats Create(GeneralStatsDto generalStats)
        {
            if (generalStats == null) return null;
            var gstats = Ioc.Container.Resolve<IGeneralStats>();
            gstats.Checkins = generalStats.Checkins;
            gstats.CheckinsUnique = generalStats.CheckinsUnique;
            gstats.Collection = generalStats.Collection;
            gstats.CollectionUnique = generalStats.CollectionUnique;
            gstats.Plays = generalStats.Plays;
            gstats.Scrobbles = generalStats.Scrobbles;
            gstats.ScrobblesUnique = generalStats.ScrobblesUnique;
            gstats.Watchers = generalStats.Watchers;
            return gstats;
        }
    }
}