using System;
using System.Collections.Generic;
using Shiftv.Contracts.Data.Images;
using Shiftv.Contracts.Domain.Movies;

namespace Shiftv.Core.Models.Movies
{
    class MiniMovie : IMiniMovie
    {
        public string Title { get; set; }
        public IIds Ids { get; set; }
        public int? Runtime { get; set; }
        public double? Rating { get; set; }
        public int? Votes { get; set; }
        public List<string> Genres { get; set; }
        public FanartDto Fanart { get; set; }
        public string Released { get; set; }

        public DateTime? ReleasedDate
        {
            get
            {
                try
                {
                    return !string.IsNullOrEmpty(Released) ? DateTime.Parse(Released).ToLocalTime() : (DateTime?)null;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public int? UserRating { get; set; }
        public bool Watched { get; set; }

        public bool InWatchlist { get; set; }
    }
}