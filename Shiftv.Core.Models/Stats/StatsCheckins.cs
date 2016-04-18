using Shiftv.Contracts.Domain.Stats;

namespace Shiftv.Core.Models.Stats
{
    class StatsCheckins : IStatsCheckins
    {
        public int All { get; set; }
        public int Users { get; set; }
    }
}