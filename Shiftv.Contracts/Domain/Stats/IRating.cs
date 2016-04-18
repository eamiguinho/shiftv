namespace Shiftv.Contracts.Domain.Stats
{
    public interface IRating
    {
         float Percentage { get; set; }
         int Votes { get; set; }
         int Loved { get; set; }
         int Hated { get; set; }
         IDistribution Distribution { get; set; }
    }
}   