using System.Collections.Generic;
using System.Threading.Tasks;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Peoples;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Domain.Stats;
using Shiftv.Global;

namespace Shiftv.Contracts.Services.Shows
{
    public interface IShowService
    {
        Task<DataResult<List<IMiniShow>>> GetTrending(bool b = false);
        Task<DataResult<List<IMiniShow>>> GetTop();
        Task<DataResult<List<IMiniShow>>> GetFresh();
        Task<DataResult<IShow>> GetByImdbId(IIds imdbId);
        Task<DataResult> SetCurrent(IShow toModel);
        Task<DataResult> SetCurrent(IMiniShow toModel);
        IShow GetCurrentShow();
        Task<IShow> GetCurrentShowWithoutCache();
        Task<DataResult<List<IShow>>> GetRecommendations();
        Task<DataResult<List<IMiniShow>>> SearchShowsByKey(string key); 
        void UpdateCurrentShow();
        Task<DataResult<IRateResult>> RateShow(int rate);
        Task<DataResult<IGenericPostResult>> AddToWatchlist();
        Task<DataResult<IGenericPostResult>> RemoveFromWatchlist();
        Task<DataResult<List<IShow>>> GetShowsWithEpisodesWatchlistByUser(string username);
        Task<DataResult<List<IShow>>> GetShowsWatchlistByUser(string username);
        Task<DataResult<List<IMiniShow>>> GetAnime();
        Task<DataResult<double?>> GetImdbRanting(string imdbId);
        Task<DataResult<List<IShowProgress>>> GetShowProgress();
        Task<DataResult<List<IShow>>> GetLovedByUser(string username);
        Task<DataResult> SetCurrent(IIds ids, string title);
        Task<DataResult<List<IMiniShow>>> GetTopImdb();
        void ClearTrending();
        void UpdateProgress();
        void UpdateWatchlist();

        Task<DataResult> SetCurrentAzure(IShow toModel);
        Task<DataResult<IPeople>> GetPeople();
        Task<DataResult<List<IMiniShow>>> GetPopular();
        Task<DataResult> ForceUpdate();
    }
}   
