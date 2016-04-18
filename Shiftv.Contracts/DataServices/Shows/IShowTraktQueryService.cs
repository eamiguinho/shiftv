using System.Threading.Tasks;

namespace Shiftv.Contracts.DataServices.Shows
{
    public interface IShowTraktQueryService
    {
        Task<string> GetTrending();
        Task<string> GetByImdbId(int imdbId);
        Task<string> GetRecommendations();
        Task<string> GetSearchByKey(string key);
        Task<string> GetAllShowGenres();
        Task<string> RateShow();
        Task<string> AddShowToWatchList();
        Task<string> RemoveShowFromWatchList();
        Task<string> GetShowsWatchlistByUser(string username);
        Task<string> GetShowsWithEpisodesWatchlistByUser(string username);
        Task<string> GetAnimeList();
        Task<string> GetImdbRating(string imdbId);
        Task<string> GetShowProgress();
        Task<string> GetLovedByUser(string username);
        Task<string> GetTrendingV2();
        Task<string> GetTopImdb();
        Task<string> GetPeople(int imdbId);
        Task<string> GetPopular();
        Task<string> ForceUpdate(int value);
    }
}
