using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.DataServices.Movies;
using Shiftv.Contracts.Domain.Categories;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Peoples;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Services;
using Shiftv.Contracts.Services.Accounts;
using Shiftv.Contracts.Services.Crawler;
using Shiftv.Contracts.Services.Movies;
using Shiftv.Global;

namespace Shiftv.Services.Implementation.Movies
{
    public class MovieService : ServiceHelper, IMovieService
    {
        private List<IMiniMovie> _trendingMovies;
        private IUserService _userService;
        private IMovieTraktDataService _movieDataService;
        private IMovie _selectedMovie;
        private ICrawlerService _crawler;
        private List<IMiniMovie> _popular;

        public MovieService(IMovieTraktDataService movieDataService, IUserService userService, ICrawlerService crawler)
        {
            _userService = userService;
            _movieDataService = movieDataService;
            _crawler = crawler;
        }

        public async Task<DataResult<List<IMiniMovie>>> GetTrending(bool isLaunch = false)
        {
            if (_trendingMovies == null)
            {
                var currentUser = _userService.GetCurrentUser();
                var res = await _movieDataService.GetTrending(UserTokenDtoFactory.GetDto(currentUser));

                if (res == null)
                    return new DataResult<List<IMiniMovie>>(StandardResults.Error);

                if (!isLaunch) _trendingMovies = res;
                //HACK

                return new DataResult<List<IMiniMovie>>(_trendingMovies);
            }
            return new DataResult<List<IMiniMovie>>(_trendingMovies);

        }

        private async void LoadData(List<IMovie> res)
        {
            CoreServices.User.SetUser(null);
            var x = await GetAnimationMovies();
            if (x.IsOk && x.Data != null && x.Data.Count > 0)
            {
                //res = x.Data;
            }
            else return;
            foreach (var show in res)
            {
                await SetCurrent(show);
                var sFull = GetCurrentMovie();
                if (sFull != null)
                {
                    System.Diagnostics.Debug.WriteLine(sFull.Title + "synced");
                }
            }
        }

        public void UpdateTrending()
        {
            if (_trendingMovies == null) return;
            var currentShow = GetCurrentMovie();
            if (currentShow == null) return;
        }


        public async Task<DataResult<List<IMiniMovie>>> SearchMoviesByKey(string key)
        {
            if (string.IsNullOrEmpty(key)) return new DataResult<List<IMiniMovie>>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<List<IMovie>>(StandardResults.Offline);
            var res = await _movieDataService.SearchMoviesByKey(key);
            if (res == null)
            {
                return new DataResult<List<IMiniMovie>>(StandardResults.Error);
            }
            return new DataResult<List<IMiniMovie>>(res);
        }

        public async Task<DataResult<List<IMovie>>> GetRecommendations()
        {
            //if (!await IsInternet()) return new DataResult<List<IMovie>>(StandardResults.Offline);
            var currentUser = _userService.GetCurrentUser();
            if (currentUser == null) return new DataResult<List<IMovie>>(StandardResults.Error);
            var res = await _movieDataService.GetRecommendations(UserTokenDtoFactory.GetDto(currentUser));
            return res == null ? new DataResult<List<IMovie>>(StandardResults.Error) : new DataResult<List<IMovie>>(res);
        }

        public async Task<DataResult<List<IMovie>>> GetByCategory(string categoryName)
        {
            //if (!await IsInternet()) return new DataResult<List<IMovie>>(StandardResults.Offline);
            var currentUser = _userService.GetCurrentUser();
            if (currentUser == null) return new DataResult<List<IMovie>>(StandardResults.Error);
            var res = await _movieDataService.GetRecommendations(UserTokenDtoFactory.GetDto(currentUser));
            return res == null ? new DataResult<List<IMovie>>(StandardResults.Error) : new DataResult<List<IMovie>>(res);
        }

        public async Task<DataResult<List<IMovie>>> GetByCategory(ICategory category)
        {
            return new DataResult<List<IMovie>>(StandardResults.Error);
            //if (category == null) return new DataResult<List<IMovie>>(StandardResults.Error);
            ////if (!await IsInternet()) return new DataResult<List<IMovie>>(StandardResults.Offline);
            //var res = await GetTrending();
            //if (res == null || !res.IsOk || res.Data == null)
            //    return new DataResult<List<IMovie>>(StandardResults.Error);
            //var x = res.Data.Where(a => a.Genres.Contains(category.Name)).ToList();
            //return new DataResult<List<IMovie>>(x);
        }


        public async Task<DataResult<IRateResult>> RateMovie(int rate)
        {
            //if (rate <= -1) return new DataResult<IRateResult>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<IRateResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            var currentMovie = GetCurrentMovie();
            if (user == null || currentMovie == null) return new DataResult<IRateResult>(StandardResults.Error);
            var res = await _movieDataService.RateMovie(UserTokenDtoFactory.GetDto(user), rate, MovieDtoFactory.GetDto(currentMovie));
            if (res == null || !res.Status)
                return new DataResult<IRateResult>(StandardResults.Error);
            // await SetCurrent(currentMovie);
            currentMovie.UserRating = rate;
            UpdateTrending();
            return new DataResult<IRateResult>(res);
        }



        public async Task<DataResult> SetCurrent(IMovie miniMovie)
        {
            if (miniMovie == null) return new DataResult(StandardResults.Error);
            var movie = await GetByImdbId(miniMovie.Ids);
            _selectedMovie = movie.IsOk ? movie.Data : null;
            return movie;
        }

        public async Task<DataResult> SetCurrent(IMiniMovie miniMovie)
        {
            if (miniMovie == null) return new DataResult(StandardResults.Error);
            var movie = await GetByImdbId(miniMovie.Ids);
            _selectedMovie = movie.IsOk ? movie.Data : null;
            return movie;
        }

        public async Task<DataResult<List<IMovie>>> GetLovedByUser(string username)
        {
            //if (!await IsInternet()) return new DataResult<List<IMovie>>(StandardResults.Offline);
            var res = await _movieDataService.GetLovedByUser(username);
            return res == null ? new DataResult<List<IMovie>>(StandardResults.Error) : new DataResult<List<IMovie>>(res);
        }

        public async Task<DataResult<IMediaStream>> GetMovieLink()
        {
            try
            {
                //if (!await IsInternet()) return new DataResult<IMediaStream>(StandardResults.Offline);
                var currentMovie = GetCurrentMovie();
                if (currentMovie == null) return new DataResult<IMediaStream>(StandardResults.Error);
                if (currentMovie.Runtime != null && currentMovie.Year != null && currentMovie.Ids.TmDbId != null)
                {
                    var res =
                        await
                            _crawler.GetLinks(currentMovie.Title, 0, 0, currentMovie.Runtime.Value, currentMovie.Year.Value, CrawlerType.Movie, currentMovie.Ids.TmDbId.Value, currentMovie.Ids.ImdbId);
                    if (res == null) return new DataResult<IMediaStream>(StandardResults.Error);
                    var linksModel = Ioc.Container.Resolve<IMediaStream>();
                    linksModel.Links = res;
                    return new DataResult<IMediaStream>(linksModel);
                }

            }
            catch (Exception)
            {
                return new DataResult<IMediaStream>(StandardResults.Error);
            }
            return new DataResult<IMediaStream>(StandardResults.Error);

        }

        private async Task<DataResult<IMovie>> GetByImdbId(IIds ids)
        {
            if (ids == null) return new DataResult<IMovie>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<IMovie>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            var req = await _movieDataService.GetByImdbId(UserTokenDtoFactory.GetDto(user), IdsDtoFactory.GetDto(ids));
            if (req == null)
            {
                return new DataResult<IMovie>(StandardResults.Error);
            }
            return new DataResult<IMovie>(req);
        }

        public IMovie GetCurrentMovie()
        {
            return _selectedMovie;
        }


        public async Task<DataResult<ICheckinResult>> CheckIn()
        {
            //if (!await IsInternet()) return new DataResult<ICheckinResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<ICheckinResult>(StandardResults.Error);
            var currentMovie = GetCurrentMovie();
            if (currentMovie == null) return new DataResult<ICheckinResult>(StandardResults.Error);
            if (currentMovie.Year != null)
            {
                var res = await _movieDataService.CheckIn(UserTokenDtoFactory.GetDto(user), currentMovie.Title, currentMovie.Ids.ImdbId, currentMovie.Year.Value);
                if (res == null)
                    return new DataResult<ICheckinResult>(StandardResults.Error);
                return new DataResult<ICheckinResult>(res);
            }
            return new DataResult<ICheckinResult>(StandardResults.Error);
        }
        public async Task<DataResult<ICheckinResult>> CancelCheckIn()
        {
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<ICheckinResult>(StandardResults.Error);
            var res = await _movieDataService.CancelCheckIn(UserTokenDtoFactory.GetDto(user));
            if (res == null)
                return new DataResult<ICheckinResult>(StandardResults.Error);
            return new DataResult<ICheckinResult>(res);
        }

        public async Task<DataResult<List<IMiniMovie>>> GetTopImdb()
        {
            var currentUser = _userService.GetCurrentUser();
            var res = await _movieDataService.GetTopImdb(UserTokenDtoFactory.GetDto(currentUser));
            if (res == null)
            {
                return new DataResult<List<IMiniMovie>>(StandardResults.Error);
            }
            return new DataResult<List<IMiniMovie>>(res.OrderByDescending(x => x.Votes).ToList());
        }

        public async Task<DataResult<List<IMiniMovie>>> GetAnimationMovies()
        {
            var currentUser = _userService.GetCurrentUser();
            var res = await _movieDataService.GetAnimationMovies(UserTokenDtoFactory.GetDto(currentUser));
            if (res == null)
            {
                return new DataResult<List<IMiniMovie>>(StandardResults.Error);
            }
            return new DataResult<List<IMiniMovie>>(res.OrderByDescending(x => x.Rating).ToList());
        }

        public async Task<DataResult<IMediaStream>> GetMovieSubtitles(string subtitlesLanguages)
        {
            var currentMovie = GetCurrentMovie();
            if (currentMovie == null) return new DataResult<IMediaStream>(StandardResults.Error);
            var res2 = await _movieDataService.GetLinks(currentMovie.Ids.ImdbId, subtitlesLanguages);
            if (res2 == null)
            {
                res2 = await _movieDataService.GetSubtitlesFromAzure(currentMovie.Ids.ImdbId, subtitlesLanguages);
                if (res2 == null) return new DataResult<IMediaStream>(StandardResults.Error);
            }
            return new DataResult<IMediaStream>(res2);
        }

        public void ClearTrending()
        {
            _trendingMovies = null;
        }

        public async Task<DataResult<List<IMiniMovie>>> GetPopular()
        {
            if (_popular == null)
            {
                var currentUser = _userService.GetCurrentUser();
                var list = await _movieDataService.GetPopular(UserTokenDtoFactory.GetDto(currentUser));
                _popular = list.OrderByDescending(x => x.Votes).ToList();
            }
            return _popular == null ? new DataResult<List<IMiniMovie>>(StandardResults.Error) : new DataResult<List<IMiniMovie>>(_popular);
            //if (!await IsInternet()) return new DataResult<List<IMovie>>(StandardResults.Offline); 
        }

        public async Task<DataResult<IPeople>> GetPeople()
        {
            var currentMovie = GetCurrentMovie();
            var res = await _movieDataService.GetPeople(IdsDtoFactory.GetDto(currentMovie.Ids));
            if (res == null) return new DataResult<IPeople>(StandardResults.Error);
            return new DataResult<IPeople>(res);
        }

        public async Task<DataResult<List<IMiniMovie>>> GetOscarsMovies()
        {
            var currentUser = _userService.GetCurrentUser();
            var res = await _movieDataService.GetOscars(UserTokenDtoFactory.GetDto(currentUser));
            if (res == null)
            {
                return new DataResult<List<IMiniMovie>>(StandardResults.Error);
            }
            return new DataResult<List<IMiniMovie>>(res.OrderByDescending(x => x.Votes).ToList());
        }

        public async Task<DataResult<List<IMiniMovie>>> GetChristmasMovies()
        {
            var currentUser = _userService.GetCurrentUser();
            var res = await _movieDataService.GetChristmasMovies(UserTokenDtoFactory.GetDto(currentUser));
            if (res == null)
            {
                return new DataResult<List<IMiniMovie>>(StandardResults.Error);
            }
            return new DataResult<List<IMiniMovie>>(res.OrderByDescending(x => x.Votes).ToList());
        }


        public async Task<DataResult<List<IMiniMovie>>> GetMoviesWatchlistByUser(string username)
        {
            if (string.IsNullOrEmpty(username)) return new DataResult<List<IMiniMovie>>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<List<IMovie>>(StandardResults.Offline); 
            var currentUser = _userService.GetCurrentUser();
            var res = await _movieDataService.GetMoviesWatchlistByUser( UserTokenDtoFactory.GetDto(currentUser));
            return res == null ? new DataResult<List<IMiniMovie>>(StandardResults.Error) : new DataResult<List<IMiniMovie>>(res);
        }

        public async Task<DataResult<List<IMiniMovie>>> GetTop()
        {
            if (!await IsInternet()) return new DataResult<List<IMiniMovie>>(StandardResults.Offline);
            var list = await CoreServices.Movie.GetPopular();
            return list.IsOk ? new DataResult<List<IMiniMovie>>(list.Data) : new DataResult<List<IMiniMovie>>(StandardResults.Error);
        }

        //public async Task<DataResult<List<IMovie>>> GetTopImdb()
        //{
        //    var list = await GetTrending();
        //    if (!list.IsOk) return new DataResult<List<IMovie>>(StandardResults.Error);
        //    foreach (var movie in list.Data.OrderByDescending(x=>x.Ratings.Loved).Take(30))
        //    {
        //        var imdb = await GetImdbRanting(movie.ImdbId);
        //        if (imdb.IsOk && imdb.Data != null)
        //        {
        //            movie.ImdbRating = imdb.Data.Value;
        //        }
        //    }
        //    return new DataResult<List<IMovie>>(list.Data.OrderByDescending(x => x.ImdbRating).ToList());
        //}
        public async Task<DataResult<List<IMiniMovie>>> GetFresh()
        {
            if (!await IsInternet()) return new DataResult<List<IMiniMovie>>(StandardResults.Offline);
            var list = await GetTrending();
            var listPop = await GetPopular();
            var listTotal = new List<IMiniMovie>();
            listTotal.AddRange(list.Data);
            listTotal.AddRange(listPop.Data);
            var finalList = new List<IMiniMovie>();
            foreach (var miniMovie in listTotal.Where(miniShow => finalList.Any(x => x.Ids.ImdbId == miniShow.Ids.ImdbId)))
            {
                finalList.Add(miniMovie);
            }
            return list.IsOk ? new DataResult<List<IMiniMovie>>(list.Data.OrderByDescending(x => x.ReleasedDate).ToList()) : new DataResult<List<IMiniMovie>>(StandardResults.Error);
        }

        public async Task<DataResult<IGenericPostResult>> AddToWatchlist()
        {
            var movie = GetCurrentMovie();
            if (movie == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<IGenericPostResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            if (movie.Year != null)
            {
                var res = await _movieDataService.AddMovieToWatchlist(UserTokenDtoFactory.GetDto(user), movie.Ids.TraktId, movie.Title, movie.Year.Value);
                if (res == null || res.Status == RequestResults.Failure)
                    return new DataResult<IGenericPostResult>(StandardResults.Error);
                movie.InWatchlist = true;
                await SetCurrent(movie);
                UpdateTrending();
                return new DataResult<IGenericPostResult>(res);
            }
            return new DataResult<IGenericPostResult>(StandardResults.Error);
        }

        public async Task<DataResult<IGenericPostResult>> RemoveFromWatchlist()
        {
            var movie = GetCurrentMovie();
            if (movie == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<IGenericPostResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            if (movie.Year != null)
            {
                var res = await _movieDataService.RemoveMovieFromWatchlist(UserTokenDtoFactory.GetDto(user), movie.Ids.TraktId, movie.Title, movie.Year.Value);
                if (res == null || res.Status == RequestResults.Failure)
                    return new DataResult<IGenericPostResult>(StandardResults.Error);
                movie.InWatchlist = false;
                await SetCurrent(movie);
                UpdateTrending();
                return new DataResult<IGenericPostResult>(res);
            }
            return new DataResult<IGenericPostResult>(StandardResults.Error);
        }

        public async Task<DataResult<IGenericPostResult>> SetAsSeen()
        {
            var movie = GetCurrentMovie();
            if (movie == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<IGenericPostResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<IGenericPostResult>(StandardResults.Error);

            var res = await _movieDataService.SetAsSeen(UserTokenDtoFactory.GetDto(user), MovieDtoFactory.GetDto(movie));
            if (res == null || res.Status == RequestResults.Failure)
                return new DataResult<IGenericPostResult>(StandardResults.Error);
            movie.Watched = true;
            UpdateTrending();
            return new DataResult<IGenericPostResult>(res);

        }

        public async Task<DataResult<IGenericPostResult>> SetAsUnseen()
        {
            var movie = GetCurrentMovie();
            if (movie == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<IGenericPostResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            var res = await _movieDataService.SetAsUnseen(UserTokenDtoFactory.GetDto(user), MovieDtoFactory.GetDto(movie));
            if (res == null || res.Status == RequestResults.Failure)
                return new DataResult<IGenericPostResult>(StandardResults.Error);
            movie.Watched = false;
            UpdateTrending();
            return new DataResult<IGenericPostResult>(res);
        }

        public async Task<DataResult<double?>> GetImdbRanting(string imdbId)
        {
            //if (!await IsInternet()) return new DataResult<double?>(StandardResults.Offline);
            var res = await _movieDataService.GetImdbRanting(imdbId);
            if (res == null) return new DataResult<double?>(StandardResults.Error);
            return new DataResult<double?>(res);
        }


    }
}
