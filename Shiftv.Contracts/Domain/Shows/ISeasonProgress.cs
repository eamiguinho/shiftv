using System.Collections.Generic;
using Shiftv.Contracts.Data.Shows;

namespace Shiftv.Contracts.Domain.Shows
{
    public interface ISeasonProgress
    {
        int Season { get; set; }
        int Percentage { get; set; }
        int Aired { get; set; }
        int Completed { get; set; }
        int Left { get; set; }
        List<int> EpisodesSeen { get; set; }
        List<int> EpisodesToSee { get; set; }
    }
}