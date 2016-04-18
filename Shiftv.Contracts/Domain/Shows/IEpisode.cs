using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Images;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Domain.Images;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Stats;

namespace Shiftv.Contracts.Domain.Shows
{
    public interface IEpisode
    {
        //int Season { get; set; }
        //int Number { get; set; }
        //int TvDbId { get; set; }
        //string ImdbId { get; set; }
        //string Title { get; set; }
        //string Overview { get; set; }
        //string Url { get; set; }
        //int FirstAired { get; set; }
        //string FirstAiredIso { get; set; }
        //long FirstAiredUtc { get; set; }
        //IImage Image { get; set; }
        //DateTime? FirstAiredUtcToDate { get; }
        //DateTime? FirstAiredLocalToDate { get; }
        //string Screen { get; set; }
        //bool Watched { get; set; }
        //bool InCollection { get; set; }
        //bool InWatchlist { get; set; }
        //bool UserRating { get; set; }
        //int UserRatingAdvanced { get; set; }
        //bool IsLoveOrHate { get; set; }
        //bool IsAdvancedRate { get; set; }
        //string ShowName { get; set; }
        //IRating Rating { get; set; }
        //int Plays { get; set; }
        //void SetEpisodeLove(bool love);
        //void SetEpisodeAsSeen(bool seen);
        //void SetInWatchlist(bool inWatchList);

        int Season { get; set; }

        int Number { get; set; }

        string Title { get; set; }

        IIds Ids { get; set; }

        int? NumberAbs { get; set; }

        string Overview { get; set; }

        string FirstAired { get; set; }

        string UpdatedAt { get; set; }

        double? Rating { get; set; }

        int? Votes { get; set; }

        List<string> AvailableTranslations { get; set; }

        IImage Images { get; set; }
        DateTime? FirstAiredDate { get; }
        string ShowName { get; set; }

        bool Watched { get; set; }
        int? RatedValue { get; set; }
    }
}
