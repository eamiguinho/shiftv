using System;
using System.Collections.Generic;
using Shiftv.Contracts.Domain.Images;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Peoples;
using Shiftv.Contracts.Domain.Stats;
using Shiftv.Contracts.Domain.Users;

namespace Shiftv.Core.Models.Movies
{
    public class Movie : IMovie
    {
        //public string Title { get; set; }
        //public int? Year { get; set; }
        //public int? Released { get; set; }
        //public string Url { get; set; }
        //public string Trailer { get; set; }
        //public int? Runtime { get; set; }
        //public string Tagline { get; set; }
        //public string Overview { get; set; }
        //public string Certification { get; set; }
        //public string ImdbId { get; set; }

        //public DateTime FirstAiredUtc { get; set; }
        //public int TmdbId { get; set; }
        //public int RtId { get; set; }
        //public int LastUpdated { get; set; }
        //public string Poster { get; set; }
        //public IImage Image { get; set; }
        //public List<IUserProfile> TopWatchers { get; set; }
        //public IRating Ratings { get; set; }
        //public IGeneralStats Stats { get; set; }
        //public IPeople People { get; set; }
        //public List<string> Genres { get; set; }
        //public bool InWatchlist { get; set; }
        //public bool IsLoveOrHate { get; set; }
        //public bool UserRating { get; set; }
        //public bool Watched { get; set; }
        //public bool RatingAdvanced { get; set; }
        //public double ImdbRating { get; set; }
        //public DateTime FirstAiredLocal { get; set; }

        //public void SetInWatchlist(bool inWatchList)
        //{
        //    InWatchlist = inWatchList;
        //}

        //public void SetMovieAsSeen(bool seen)
        //{
        //    Watched = seen;
        //}

        //public void SetMovieLove(bool loved)
        //{
        //    IsLoveOrHate = true;
        //    UserRating = loved;
        //}
        public string Title { get; set; }
        public int? Year { get; set; }
        public IIds Ids { get; set; }
        public string Tagline { get; set; }
        public string Overview { get; set; }
        public string Released { get; set; }
        public int? Runtime { get; set; }
        public string Trailer { get; set; }
        public string Homepage { get; set; }
        public double? Rating { get; set; }
        public int? Votes { get; set; }
        public string UpdatedAt { get; set; }
        public string Language { get; set; }
        public List<string> AvailableTranslations { get; set; }
        public List<string> Genres { get; set; }
        public string Certification { get; set; }
        public IImage Images { get; set; }
        public int? UserRating { get; set; }
        public bool Watched { get; set; }

        public bool InWatchlist
        { get; set; }
    }
}
