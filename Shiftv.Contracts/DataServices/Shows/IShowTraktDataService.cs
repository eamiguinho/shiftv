using System.Collections.Generic;
using System.Threading.Tasks;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.Domain.Peoples;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Contracts.DataServices.Shows
{
    public interface IShowTraktDataService
    {
        Task<List<IMiniShow>> GetTrending(UserTokenDto userAccount);
        Task<IShow> GetByImdbId(UserTokenDto getDto, IdsDto imdbId);
        Task<List<IShow>> GetRecommendationsByUser(UserTokenDto userAccount);
        Task<List<IMiniShow>> SearchShowsByKey(string key);
        Task<IGenericPostResult> AddShowToWatchlist(UserTokenDto userAccount, int tvDbId, string title, int year);
        Task<IGenericPostResult> RemoveShowFromWatchlist(UserTokenDto userAccount, int tvDbId, string title, int year);
        Task<List<IShow>> GetShowsWatchlistByUser(UserTokenDto userAccount, bool isMyUser = false);
        Task<List<IShow>> GetShowsWithEpisodesWatchlistByUser(string username);
        Task<List<IMiniShow>> GetAnimeList(UserTokenDto user);
        Task<double?> GetImdbRanting(string imdbId);
        Task<List<IShowProgress>> GetShowProgress(UserTokenDto userAccount);
        Task<List<IShow>> GetLovedByUser(string username);
        Task<List<IMiniShow>> GetTopImdb(UserTokenDto getDto);
        Task<IPeople> GetPeople(IdsDto imdbId);
        Task<List<IMiniShow>> GetPopular(UserTokenDto getDto);
        Task<IRateResult> RateShow(UserTokenDto userAccount, int rate, ShowDto show);
        Task<IShow> ForceUpdate(IdsDto getDto, UserTokenDto user);
    }
}
        