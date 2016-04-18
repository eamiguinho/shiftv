using Shiftv.Contracts.Domain.Stats;

namespace Shiftv.Core.Models.Stats
{
    class StatsScrobbles : IStatsScrobbles
    {
        public int All { get; set; }
        public int Users { get; set; }
    }
}