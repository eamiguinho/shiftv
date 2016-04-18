using System.Collections.Generic;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Images;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Domain.Images;
using Shiftv.Contracts.Domain.Movies;

namespace Shiftv.Contracts.Domain.Shows
{
    public interface ISeason
    {
         //int Number { get; set; }
         //List<IEpisode> Episodes { get; set; }
         //string Url { get; set; }
         //string Poster { get; set; }
         //IImage Image { get; set; }

        int? Number { get; set; }

        IIds Ids { get; set; }

        double? Rating { get; set; }

        int? Votes { get; set; }

        int? EpisodeCount { get; set; }

        string Overview { get; set; }

        IImage Images { get; set; }

        List<IEpisode> Episodes { get; set; }
    }
}
