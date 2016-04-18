namespace Shiftv.Contracts.Domain.Stats
{
    public interface IStatistics
    {
        IRating Ratings { get; set; }
        int Watchers { get; set; }
        int Plays { get; set; }
        IStatsScrobbles Scrobbles { get; set; }
        IStatsCheckins Checkins { get; set; }
        IStatsCollection Collection { get; set; }
        IStatsLists Lists { get; set; }
        IStatsComments Comments { get; set; }
    }
}