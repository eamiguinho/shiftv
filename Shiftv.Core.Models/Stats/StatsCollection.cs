using Shiftv.Contracts.Domain.Stats;

namespace Shiftv.Core.Models.Stats
{
    class StatsCollection : IStatsCollection
    {
        public int All { get; set; }
        public int Users { get; set; }
    }
}