using System.Collections.Generic;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Core.Models.Shows
{
    class SeasonProgress : ISeasonProgress
    {
        public int Season { get; set; }
        public int Percentage { get; set; }
        public int Aired { get; set; }
        public int Completed { get; set; }
        public int Left { get; set; }
        public List<int> EpisodesSeen { get; set; }
        public List<int> EpisodesToSee { get; set; }
    }
}