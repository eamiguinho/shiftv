using Shiftv.Contracts.Domain.Stats;

namespace Shiftv.Core.Models.Stats
{
    public class GeneralStats : IGeneralStats
    {
        public int Watchers { get; set; }
        public int Plays { get; set; }
        public int Scrobbles { get; set; }
        public int ScrobblesUnique { get; set; }
        public int Checkins { get; set; }
        public int CheckinsUnique { get; set; }
        public int Collection { get; set; }
        public int CollectionUnique { get; set; }
    }
}