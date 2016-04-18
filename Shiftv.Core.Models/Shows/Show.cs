using System;
using System.Collections.Generic;
using System.Linq;
using Shiftv.Contracts.Domain.Images;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Core.Models.Shows
{
    public class Show : IShow
    {
        //public string Title { get; set; }
        //public int Year { get; set; }
        //public string Url { get; set; }
        //public int FirstAired { get; set; }
        //public string FirstAiredIso { get; set; }
        //public DateTime FirstAiredUtc { get; set; }
        //public string Country { get; set; }
        //public string Overview { get; set; }
        //public int Runtime { get; set; }
        //public string Status { get; set; }
        //public string Network { get; set; }
        //public string AirDay { get; set; }
        //public string AirDayUtc { get; set; }
        //public string AirTime { get; set; }
        //public string AirTimeUtc { get; set; }
        //public string Certification { get; set; }
        //public string ImdbId { get; set; }
        //public int TvDbId { get; set; }
        //public int? TvRageId { get; set; }
        //public int LastUpdated { get; set; }
        //public string Poster { get; set; }
        //public IImage Image { get; set; }
        //public List<IUserProfile> TopWatchers { get; set; }
        //public List<IEpisode> TopEpisodes { get; set; }
        //public IPeople People { get; set; }
        //public List<string> Genres { get; set; }
        //public List<ISeason> Seasons { get; set; }
        //public int Plays { get; set; }
        //public bool Watched { get; set; }
        //public bool InWatchlist { get; set; }
        //public bool UserRating { get; set; }
        //public bool RatingAdvanced { get; set; }
        //public bool IsLoveOrHate { get; set; }
        //public IRating Rating { get; set; }



        public IEpisode GetNextEpisode()
        {
            if (Seasons == null) return null;
            var season = Seasons.OrderByDescending(x => x.Number).FirstOrDefault();
            var list =
                season.Episodes != null?
                season.Episodes.Where(x => x.FirstAiredDate != null && x.FirstAiredDate.Value > DateTime.Now)
                .ToList() : null;
            if (list != null && list.Count > 0)
            {
                var epi =  list.First();
                if (epi.Images.Screenshot.Full == null)
                {
                    epi.Images.Screenshot.Full = Images.Fanart.Full;
                    epi.Images.Screenshot.Medium = Images.Fanart.Medium;
                    epi.Images.Screenshot.Thumb = Images.Fanart.Thumb;
                }
                return epi;
            }
            return list != null ? season.Episodes.All(x => x.FirstAiredDate != null) ? season.Episodes.Last() : season.Episodes.First() : null;
        }

        public int? UserRating { get; set; }

        public string Title { get; set; }
        public int? Year { get; set; }
        public IIds Ids { get; set; }
        public string Overview { get; set; }
        public string FirstAired { get; set; }
        public IAirs Airs { get; set; }
        public int? Runtime { get; set; }
        public string Certification { get; set; }
        public string Network { get; set; }
        public string Country { get; set; }
        public string Homepage { get; set; }
        public string Status { get; set; }
        public double? Rating { get; set; }
        public int? Votes { get; set; }
        public string UpdatedAt { get; set; }
        public string Language { get; set; }
        public List<string> AvailableTranslations { get; set; }
        public List<string> Genres { get; set; }
        public int? AiredEpisodes { get; set; }
        public IImage Images { get; set; }
        public bool IsAnime
        {
            get { return (Country.ToLower() == "japan"|| Country.ToLower() =="jp") && Genres.Contains("animation") || Country == null && Genres.Contains("animation"); }
        }


        public List<ISeason> Seasons { get; set; }

    }
}
