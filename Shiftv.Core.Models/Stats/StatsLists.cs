using Shiftv.Contracts.Domain.Stats;

namespace Shiftv.Core.Models.Stats
{
    class StatsLists : IStatsLists
    {
        public int All { get; set; }
        public int Watchlist { get; set; }
        public int Custom { get; set; }
    }
}