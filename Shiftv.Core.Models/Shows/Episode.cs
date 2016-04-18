using System;
using System.Collections.Generic;
using Shiftv.Contracts.Domain.Images;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Domain.Stats;

namespace Shiftv.Core.Models.Shows
{
    public class Episode : IEpisode
    {
        //public int Season { get; set; }
        //public int Number { get; set; }
        //public int TvDbId { get; set; }
        //public string ImdbId { get; set; }
        //public string Title { get; set; }
        //public string Overview { get; set; }
        //public string Url { get; set; }
        //public int FirstAired { get; set; }
        //public string FirstAiredIso { get; set; }
        //public long FirstAiredUtc { get; set; }
        //public IImage Image { get; set; }
        //public DateTime? FirstAiredUtcToDate
        //{
        //    get
        //    {
        //        if (FirstAiredIso != null) return DateTime.Parse(FirstAiredIso).ToUniversalTime();
        //        return null;
        //    }
        //}
        //public DateTime? FirstAiredLocalToDate
        //{
        //    get
        //    {
        //        if (FirstAiredIso != null) return DateTime.Parse(FirstAiredIso).ToLocalTime();
        //        return null;
        //    }
        //}
        //public string Screen { get; set; }
        //public bool Watched { get; set; }
        //public bool InCollection { get; set; }
        //public bool InWatchlist { get; set; }
        //public bool UserRating { get; set; }
        //public int UserRatingAdvanced { get; set; }
        //public bool IsLoveOrHate { get; set; }
        //public bool IsAdvancedRate { get; set; }
        //public string ShowName { get; set; }
        //public IRating Rating { get; set; }
        //public int Plays { get; set; }

        //public void SetEpisodeLove(bool loved)
        //{
        //    IsLoveOrHate = true;
        //    UserRating = loved;
        //}

        //public void SetEpisodeAsSeen(bool seen)
        //{
        //    Watched = seen;
        //}

        //public void SetInWatchlist(bool inWatchList)
        //{
        //    InWatchlist = inWatchList;
        //}
        public int Season { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public IIds Ids { get; set; }
        public int? NumberAbs { get; set; }
        public string Overview { get; set; }
        public string FirstAired { get; set; }
        public string UpdatedAt { get; set; }
        public double? Rating { get; set; }
        public int? Votes { get; set; }
        public List<string> AvailableTranslations { get; set; }
        public IImage Images { get; set; }
        public DateTime? FirstAiredDate
        {
            get
            {
                try
                {
                    if (FirstAired == null) return null;
                    return DateTime.Parse(FirstAired).ToLocalTime();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public string ShowName { get; set; }
        public bool Watched { get; set; }
        public int? RatedValue { get; set; }
    }
}