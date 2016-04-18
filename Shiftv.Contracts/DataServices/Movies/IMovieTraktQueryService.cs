using System.Threading.Tasks;

namespace Shiftv.Contracts.DataServices.Movies
{
    public interface IMovieTraktQueryService
    {
        Task<string> GetTredingQuery();
        Task<string> GetSearchMoviesByKey(string key);
        Task<string> GetRecommendations();
        Task<string> RateMovie();
        Task<string> GetByImdbId(int imdbId);
        Task<string> GetCheckIn();
        Task<string> GetUserWatchlist();
        Task<string> GetLovedByUser(string username);
        Task<string> GetLinks(string imdbId, string language);
        Task<string> GetSetAsSeen();
        Task<string> GetSetAsUnseen();
        Task<string> AddMovieToWatchList();
        Task<string> RemoveMovieFromWatchlist();
        Task<string> GetImdbRating(string imdbId);
        Task<string> GetCancelCheckIn();
        Task<string> GetTopImdb();
        Task<string> GetAnimationMovies();
        Task<string> GetPopular();
        Task<string> GetPeople(int imdbId);
        Task<string> GetOscars();
        Task<string> GetChristmas();
    }
}
