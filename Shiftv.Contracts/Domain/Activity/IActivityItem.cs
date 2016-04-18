using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Domain.Users;

namespace Shiftv.Contracts.Domain.Activity
{
    public interface IActivityItem
    {
        int Timestamp { get; set; }
        IActivityWhen When { get; set; }
        IActivityElapsed Elapsed { get; set; }
        ActivityTypes Type { get; set; }
        ActivityActions Action { get; set; }
        IUser User { get; set; }
        IEpisode Episode { get; set; }
        IShow Show { get; set; }
        int Id { get; set; }
        IMovie Movie { get; set; }
        string Rating { get; set; }
        int? RatingAdvanced { get; set; }
        bool? IsRatingAdvanced { get; set; }
    }

    public enum ActivityActions
    {
        Watching,
        Scrobble,
        Checkin,
        Seen,
        Collection,
        Rating,
        Watchlist,
        Shout,
        Review,
        Unknown
    }

    public enum ActivityTypes
    {
        Episode,
        Show,
        Movie,
        Unknown
    }
}