using System.Collections.Generic;

namespace Shiftv.Contracts.Domain.Shows
{
    public interface IShowProgress
    {
        IMiniShow Show { get; set; }

        List<IEpisode> EpisodesLeft { get; set; }

        int? TotalEpisodes { get; set; }
    }
}