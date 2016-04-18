using System.Collections.Generic;
using Shiftv.Contracts.Domain.Images;
using Shiftv.Contracts.Domain.Movies;

namespace Shiftv.Contracts.Domain.Shows
{
    public interface IShow
    {
        //string Title { get; set; }
        //int Year { get; set; }
        //string Url { get; set; }
        //int FirstAired { get; set; }
        //string FirstAiredIso { get; set; }
        //DateTime FirstAiredUtc { get; set; }
        //string Country { get; set; }
        //string Overview { get; set; }
        //int Runtime { get; set; }
        //string Status { get; set; }
        //string Network { get; set; }
        //string AirDay { get; set; }
        //string AirDayUtc { get; set; }
        //string AirTime { get; set; }
        //string AirTimeUtc { get; set; }
        //string Certification { get; set; }
        //string ImdbId { get; set; }
        //int TvDbId { get; set; }
        //int? TvRageId { get; set; }
        //int LastUpdated { get; set; }
        //string Poster { get; set; }
        //IImage Image { get; set; }  
        //List<IUserProfile> TopWatchers { get; set; }
        //List<IEpisode> TopEpisodes { get; set; }
        //IPeople People { get; set; }
        //List<string> Genres { get; set; }
        //IEpisode GetNextEpisode();
        //List<ISeason> Seasons { get; set; }
        //int Plays { get; set; }
        //bool Watched { get; set; }
        //bool InWatchlist { get; set; }
        //bool UserRating { get; set; }
        //bool RatingAdvanced { get; set; }
        //bool IsLoveOrHate { get; set; }
        //IRating Rating { get; set; }
        //bool IsAnime { get; }

        string Title { get; set; }

        int? Year { get; set; }

        IIds Ids { get; set; }

        string Overview { get; set; }

        string FirstAired { get; set; }

        IAirs Airs { get; set; }

        int? Runtime { get; set; }

        string Certification { get; set; }

        string Network { get; set; }

        string Country { get; set; }


        string Homepage { get; set; }

        string Status { get; set; }

        double? Rating { get; set; }

        int? Votes { get; set; }

        string UpdatedAt { get; set; }

        string Language { get; set; }

        List<string> AvailableTranslations { get; set; }

        List<string> Genres { get; set; }

        int? AiredEpisodes { get; set; }

        IImage Images { get; set; }
        bool IsAnime { get;  }
        List<ISeason> Seasons { get; set; }
        IEpisode GetNextEpisode();

        int? UserRating { get; set; }

    }
}
