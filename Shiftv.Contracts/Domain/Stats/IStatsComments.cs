namespace Shiftv.Contracts.Domain.Stats
{
    public interface IStatsComments
    {
        int All { get; set; }
        int Reviews { get; set; }
        int Shouts { get; set; }
    }
}