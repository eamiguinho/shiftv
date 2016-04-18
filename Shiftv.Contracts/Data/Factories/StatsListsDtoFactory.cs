using Autofac;
using Shiftv.Contracts.Data.Stats;
using Shiftv.Contracts.Domain.Stats;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class StatsListsDtoFactory
    {
        public static IStatsLists Create(StatsListsDto listsDto)
        {
            var lists = Ioc.Container.Resolve<IStatsLists>();
            lists.All = listsDto.All;
            lists.Custom = listsDto.Custom;
            lists.Watchlist = listsDto.Watchlist;
            return lists;      
        }
    }
}