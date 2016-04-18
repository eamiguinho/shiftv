namespace Shiftv.Contracts.Domain.Stats
{
    public interface IGeneralStats
    {
         int Watchers { get; set; }
         int Plays { get; set; }
         int Scrobbles { get; set; }
         int ScrobblesUnique { get; set; }
         int Checkins { get; set; }
         int CheckinsUnique { get; set; }
         int Collection { get; set; }   
         int CollectionUnique { get; set; }
    }
}