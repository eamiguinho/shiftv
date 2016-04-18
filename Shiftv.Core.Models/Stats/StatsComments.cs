using Shiftv.Contracts.Domain.Stats;

namespace Shiftv.Core.Models.Stats
{
    class StatsComments : IStatsComments
    {
        public int All { get; set; }
        public int Reviews { get; set; }
        public int Shouts { get; set; }
    }
}