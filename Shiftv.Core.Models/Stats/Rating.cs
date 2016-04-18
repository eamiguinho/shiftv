using Newtonsoft.Json;
using Shiftv.Contracts.Domain.Stats;

namespace Shiftv.Core.Models.Stats
{
    public class Rating : IRating
    {
        public float Percentage { get; set; } 
        public int Votes { get; set; }
        public int Loved { get; set; }
        public int Hated { get; set; }
        public IDistribution Distribution { get; set; }
    }
}