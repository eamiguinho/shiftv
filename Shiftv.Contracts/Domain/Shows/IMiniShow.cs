using System;
using System.Collections.Generic;
using Shiftv.Contracts.Domain.Images;
using Shiftv.Contracts.Domain.Movies;

namespace Shiftv.Contracts.Domain.Shows
{
    public interface IMiniShow
    {
        string Title { get; set; }

        IIds Ids { get; set; }

        string Network { get; set; }

        double? Rating { get; set; }

        int? Votes { get; set; }

        int? Year { get; set; }
        IFanart Fanart { get; set; }

        string FirstAired { get; set; }
        DateTime? FirstAiredData { get; }
        int? UserRating { get; set; }

        List<string> Genres { get; set; }
        string Status { get; set; }
    }
}