using Shiftv.Contracts.Domain.Stats;

namespace Shiftv.Core.Models.Stats
{
    class Statistics : IStatistics
    {
        public IRating Ratings { get; set; }
        public int Watchers { get; set; }
        public int Plays { get; set; }
        public IStatsScrobbles Scrobbles { get; set; }
        public IStatsCheckins Checkins { get; set; }
        public IStatsCollection Collection { get; set; }
        public IStatsLists Lists { get; set; }
        public IStatsComments Comments { get; set; }
    }
}