using System.Collections.Generic;
using System.Threading.Tasks;
using Shiftv.Contracts.Domain.Categories;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Peoples;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Global;

namespace Shiftv.Contracts.Services.Movies
{
    public interface IMovieService
    {
        Task<DataResult<List<IMiniMovie>>> GetTrending(bool b = false);
        Task<DataResult<List<IMiniMovie>>> SearchMoviesByKey(string key);
        Task<DataResult<List<IMovie>>> GetRecommendations();
        Task<DataResult<List<IMovie>>> GetByCategory(ICategory category);
        IMovie GetCurrentMovie();
        Task<DataResult<ICheckinResult>> CheckIn();
        Task<DataResult<List<IMiniMovie>>> GetMoviesWatchlistByUser(string username);
        Task<DataResult> SetCurrent(IMovie toModel);
        Task<DataResult> SetCurrent(IMiniMovie toModel);
        Task<DataResult<List<IMovie>>> GetLovedByUser(string username);
        Task<DataResult<IMediaStream>> GetMovieLink();
        Task<DataResult<List<IMiniMovie>>> GetTop();
        Task<DataResult<List<IMiniMovie>>> GetFresh();
        Task<DataResult<IGenericPostResult>> AddToWatchlist();
        Task<DataResult<IGenericPostResult>> RemoveFromWatchlist();
        Task<DataResult<IGenericPostResult>> SetAsSeen();
        Task<DataResult<IGenericPostResult>> SetAsUnseen();
        Task<DataResult<IRateResult>> RateMovie(int rate);
        //Task<DataResult<List<IMovie>>> GetTopImdb();
        Task<DataResult<double?>> GetImdbRanting(string imdbId);
        Task<DataResult<ICheckinResult>> CancelCheckIn();
        Task<DataResult<List<IMiniMovie>>> GetTopImdb();
        Task<DataResult<List<IMiniMovie>>> GetAnimationMovies();
        Task<DataResult<IMediaStream>> GetMovieSubtitles(string subtitlesLanguages);
        void ClearTrending();
        Task<DataResult<List<IMiniMovie>>> GetPopular();
        Task<DataResult<IPeople>> GetPeople();
        Task<DataResult<List<IMiniMovie>>> GetOscarsMovies();
        Task<DataResult<List<IMiniMovie>>> GetChristmasMovies();
    }
}