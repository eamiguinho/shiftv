using System.Collections.Generic;
using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Core.Models.Shows
{
    class ShowProgress : IShowProgress
    {
        public IMiniShow Show { get; set; }

        public List<IEpisode> EpisodesLeft { get; set; }

        public int? TotalEpisodes { get; set; }
    }
}