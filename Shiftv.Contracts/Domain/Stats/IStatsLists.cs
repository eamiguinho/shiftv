namespace Shiftv.Contracts.Domain.Stats
{
    public interface IStatsLists
    {
        int All { get; set; }
        int Watchlist { get; set; }
        int Custom { get; set; }
    }
}