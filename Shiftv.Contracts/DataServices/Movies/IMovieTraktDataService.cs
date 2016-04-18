using System.Collections.Generic;
using System.Threading.Tasks;
using Shiftv.Contracts.Data.Movies;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Peoples;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Contracts.DataServices.Movies
{
    public interface IMovieTraktDataService
    {
        Task<List<IMiniMovie>> GetTrending(UserTokenDto userAccount);
        Task<List<IMiniMovie>> SearchMoviesByKey(string key);
        Task<List<IMovie>> GetRecommendations(UserTokenDto userAccount);
        Task<IRateResult> RateMovie(UserTokenDto userAccount, int rate, MovieDto movie);
        Task<IMovie> GetByImdbId(UserTokenDto getDto, IdsDto ids);
        Task<ICheckinResult> CheckIn(UserTokenDto userAccount, string title, string imdbId, int year);
        Task<List<IMiniMovie>> GetMoviesWatchlistByUser(UserTokenDto getDto);
        Task<List<IMovie>> GetLovedByUser(string username);
        Task<IMediaStream> GetLinks(string imdbId, string language);
        Task<IGenericPostResult> SetAsSeen(UserTokenDto userAccount, MovieDto movie);
        Task<IGenericPostResult> SetAsUnseen(UserTokenDto userAccount, MovieDto movie);

        Task<IGenericPostResult> AddMovieToWatchlist(UserTokenDto userAccount, int? imdbId, string title, int year);

        Task<IGenericPostResult> RemoveMovieFromWatchlist(UserTokenDto userAccount, int? imdbId, string title, int year);

        Task<double?> GetImdbRanting(string imdbId);

        Task<ICheckinResult> CancelCheckIn(UserTokenDto getDto);
        Task<List<IMiniMovie>> GetTopImdb(UserTokenDto getDto);
        Task<List<IMiniMovie>> GetAnimationMovies(UserTokenDto getDto);
        Task<IMediaStream> GetSubtitlesFromAzure(string imdbId, string language);
        Task<List<IMiniMovie>> GetPopular(UserTokenDto user);
        Task<IPeople> GetPeople(IdsDto ids);
        Task<List<IMiniMovie>> GetOscars(UserTokenDto user);
        Task<List<IMiniMovie>> GetChristmasMovies(UserTokenDto getDto);
    }
}