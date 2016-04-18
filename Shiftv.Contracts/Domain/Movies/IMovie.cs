using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Domain.Images;
using Shiftv.Contracts.Domain.Peoples;
using Shiftv.Contracts.Domain.Stats;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Contracts;

namespace Shiftv.Contracts.Domain.Movies
{
    public interface IMovie
    {
        //string Title { get; set; }
        //int Year { get; set; }

        //int Released { get; set; }
        //DateTime FirstAiredUtc { get; set; }
        //string Url { get; set; }
        //string Trailer { get; set; }
        //int Runtime { get; set; }
        //string Tagline { get; set; }
        //string Overview { get; set; }
        //string Certification { get; set; }
        //string ImdbId { get; set; }
        //int TmdbId { get; set; }
        //int RtId { get; set; }
        //int LastUpdated { get; set; }
        //string Poster { get; set; }
        //IImage Image { get; set; }
        //List<IUserProfile> TopWatchers { get; set; }
        //IRating Ratings { get; set; }
        //IGeneralStats Stats { get; set; }
        //IPeople People { get; set; }
        //List<string> Genres { get; set; }
        //bool InWatchlist { get; set; }
        //bool IsLoveOrHate { get; set; }
        //bool UserRating { get; set; }
        //bool Watched { get; set; }
        //bool RatingAdvanced { get; set; }
        //double ImdbRating { get; set; }
        //DateTime FirstAiredLocal { get; set; }

        //void SetInWatchlist(bool b);
        //void SetMovieAsSeen(bool b);
        //void SetMovieLove(bool b);

        string Title { get; set; }

        int? Year { get; set; }

        IIds Ids { get; set; }

        string Tagline { get; set; }

        string Overview { get; set; }

        string Released { get; set; }

        int? Runtime { get; set; }

        string Trailer { get; set; }

        string Homepage { get; set; }

        double? Rating { get; set; }

        int? Votes { get; set; }

        string UpdatedAt { get; set; }

        string Language { get; set; }

        List<string> AvailableTranslations { get; set; }

        List<string> Genres { get; set; }

        string Certification { get; set; }

        IImage Images { get; set; }

        int? UserRating { get; set; }
        bool Watched { get; set; }
        bool InWatchlist { get; set; }
    }
}
