using Autofac;
using Shiftv.Contracts.Data.Stats;
using Shiftv.Contracts.Domain.Stats;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class StatsCollectionDtoFactory
    {
        public static IStatsCollection Create(StatsCollectionDto collectionDto)
        {
            var collection = Ioc.Container.Resolve<IStatsCollection>();
            collection.All = collectionDto.All;
            collection.Users = collectionDto.Users;
            return collection;
        }
    }
}