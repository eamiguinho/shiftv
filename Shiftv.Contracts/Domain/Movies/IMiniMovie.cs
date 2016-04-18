using System;
using System.Collections.Generic;
using Shiftv.Contracts.Data.Images;

namespace Shiftv.Contracts.Domain.Movies
{
    public interface IMiniMovie
    {
        string Title { get; set; }

        IIds Ids { get; set; }
        int? Runtime { get; set; }

        double? Rating { get; set; }

        int? Votes { get; set; }

        List<string> Genres { get; set; }

        FanartDto Fanart { get; set; }

        string Released { get; set; }
        DateTime? ReleasedDate { get;  }
        int? UserRating { get; set; }
        bool Watched { get; set; }
        bool InWatchlist { get; set; }

    }
}