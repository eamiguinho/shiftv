using System.Collections.Generic;
using Shiftv.Contracts.Domain.Images;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Core.Models.Shows
{
    public class Season : ISeason
    {
        //public int Number { get; set; }
        //public List<IEpisode> Episodes { get; set; }
        //public string Url { get; set; }
        //public string Poster { get; set; }
        //public IImage Image { get; set; }

        public int? Number { get; set; }
        public IIds Ids { get; set; }
        public double? Rating { get; set; }
        public int? Votes { get; set; }
        public int? EpisodeCount { get; set; }
        public string Overview { get; set; }
        public IImage Images { get; set; }
        public List<IEpisode> Episodes { get; set; }
    }
}
