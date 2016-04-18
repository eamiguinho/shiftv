using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Helpers;
using ShiftvAPI.Contracts.Infrastucture.Shiftv;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Movies;
using ShiftvAPI.Contracts.Services.Movies;

namespace ShiftvAPI.Services.Implementation.Movies
{
    class MovieService : IMovieService
    {
        private IMovieShiftvDataService _movieShiftvDataService;
        private IMovieTraktDataService _movieTraktDataService;
        private ISyncShiftvDataService _syncShiftvDataService;

        public MovieService(IMovieShiftvDataService movieShiftvDataService, IMovieTraktDataService movieTraktDataService, ISyncShiftvDataService syncShiftvDataService)
        {
            _movieShiftvDataService = movieShiftvDataService;
            _movieTraktDataService = movieTraktDataService;
            _syncShiftvDataService = syncShiftvDataService;
        }
        public async Task<DataResult<Movie>> GetMovieById(int id, string token)
        {
            if (id < 0) return new DataResult<Movie>(StandardResults.Error);
            var movieData = await _movieShiftvDataService.GetMovieById(id);
            if (movieData == null)
            {
                movieData = await _movieTraktDataService.GetMovieById(id);
                await SaveMovieDb(movieData);
            }
            if (movieData == null)
            {
                return new DataResult<Movie>(StandardResults.Error);
            }
            CheckUserRating(token, movieData);
            CheckWatched(token, movieData);
            CheckWatchlist(token, movieData);
            return new DataResult<Movie>(movieData);
        }

       

        public async Task<DataResult<List<MiniMovie>>> GetTrending(int page, int limit, string token)
        {
            var movieData = await _movieShiftvDataService.GetTrending(page, limit);
            if (movieData == null)
            {
                var fullDataMovie = await _movieTraktDataService.GetTrending(page, limit);
                if (fullDataMovie == null) return new DataResult<List<MiniMovie>>(StandardResults.Error);
                var listMini = fullDataMovie.Select(fullData => new MiniMovie()
                {
                    Fanart = fullData.Movie.Images.Fanart,
                    Ids = fullData.Movie.Ids,
                    Genres = fullData.Movie.Genres,
                    Rating = fullData.Movie.Rating,
                    Title = fullData.Movie.Title,
                    Votes = fullData.Movie.Votes,
                    Runtime = fullData.Movie.Runtime,
                    Released = fullData.Movie.Released
                }).ToList();
                movieData = listMini;
                await _movieShiftvDataService.SaveTrending(listMini);
                //foreach (var showTrending in fullDataMovie)
                //{
                //    await SaveMovieDb(showTrending.Movie);
                //}
            }
            CheckUserRatings(token, movieData);
            CheckWatched(token, movieData); 
            CheckWatchlist(token, movieData);
            return new DataResult<List<MiniMovie>>(movieData);
        }
        private void CheckUserRatings(string token, List<MiniMovie> movieData)
        {
            if (string.IsNullOrEmpty(token)) return;
            var userRatings = _syncShiftvDataService.GetMovieRatingsByUser(token);
            if (userRatings == null) return;
            foreach (var userRating in userRatings)
            {
                var show = movieData.FirstOrDefault(x => x.Ids.TraktId == userRating.TraktId);
                if (show != null) show.UserRating = userRating.Rating;
            }
        }

        private void CheckWatched(string token, Movie movieData)
        {
            if (!string.IsNullOrEmpty(token))
            {
                var userWatched = _syncShiftvDataService.GetMoviesWatchedByUser(token);
                movieData.Watched = userWatched.Any(x=>movieData.Ids.TraktId != null && x.TraktId == movieData.Ids.TraktId.Value);
            }
        }

        private void CheckWatched(string token, List<MiniMovie> movieData)
        {
            if (!string.IsNullOrEmpty(token))
            {
                var userWatched = _syncShiftvDataService.GetMoviesWatchedByUser(token);
                if (userWatched == null) return;
                foreach (var miniMovie in movieData)
                {
                    if (userWatched.Any(x => x.TraktId == miniMovie.Ids.TraktId))
                    {
                        miniMovie.Watched = true;
                    }
                }
            }
        }
        private void CheckWatchlist(string token, Movie movieData)
        {
            if (!string.IsNullOrEmpty(token))
            {
                var userWatched = _movieShiftvDataService.GetUserWatchlist(token);
                movieData.InWatchlist = userWatched.Any(x => movieData.Ids.TraktId != null && x.Ids.TraktId == movieData.Ids.TraktId.Value);
            }
        }
        private void CheckWatchlist(string token, List<MiniMovie> movieData)
        {
            if (!string.IsNullOrEmpty(token))
            {
                var userWatched = _movieShiftvDataService.GetUserWatchlist(token);
                if (userWatched == null) return;
                foreach (var miniMovie in movieData)
                {
                    if (userWatched.Any(x => x.Ids.TraktId == miniMovie.Ids.TraktId))
                    {
                        miniMovie.InWatchlist = true;
                    }
                }
            }
        }

        private void CheckUserRating(string token, Movie showData)
        {
            if (string.IsNullOrEmpty(token)) return;
            var userRatings = _syncShiftvDataService.GetMovieRatingsByUser(token);
            if (userRatings == null) return;
            var userRating =
                userRatings.FirstOrDefault(
                    x => showData.Ids.TraktId != null && x.TraktId == showData.Ids.TraktId.Value);
            if (userRating == null) return;
            showData.UserRating = userRating.Rating;
        }


        private async Task SaveMovieDb(Movie movie)
        {
            await _movieShiftvDataService.SaveMovie(movie);
        }

        public async Task<DataResult<List<MiniMovie>>> GetPopular(int page, int limit, string token)
        {
            var movieData = await _movieShiftvDataService.GetPopular(page, limit);
            if (movieData == null)
            {
                var movieFullData = await _movieTraktDataService.GetPopular(page, limit);
                if (movieFullData == null) return new DataResult<List<MiniMovie>>(StandardResults.Error);
                var listMini = movieFullData.Select(fullData => new MiniMovie()
                {
                    Fanart = fullData.Images.Fanart,
                    Ids = fullData.Ids,
                    Genres = fullData.Genres,
                    Runtime = fullData.Runtime,
                    Rating = fullData.Rating,
                    Title = fullData.Title,
                    Votes = fullData.Votes,
                    Released = fullData.Released
                }).ToList();
                movieData = listMini;
                await _movieShiftvDataService.SavePopular(listMini);
                //foreach (var showTrending in movieFullData)
                //{
                //    await SaveMovieDb(showTrending);
                //}

            }
            CheckUserRatings(token, movieData);
            CheckWatched(token, movieData);
            CheckWatchlist(token, movieData);
            return new DataResult<List<MiniMovie>>(movieData);
        }

        public async void UpdateData()
        {
            var page = 1;
            var lastUpdate = _movieShiftvDataService.GetLastUpdate() ?? DateTime.Now.AddDays(-5);
            while (true)
            {
                var movieData = await _movieTraktDataService.GetUpdates(page, 100, lastUpdate);
                if (movieData == null || movieData.Count == 0)
                {
                    _movieShiftvDataService.SaveLastUpdate(DateTime.Now);
                    return;
                }
                foreach (var movieUpdate in movieData)
                {
                    if (!string.IsNullOrEmpty(movieUpdate.Movie.Ids.ImdbId)) await SaveMovieDb(movieUpdate.Movie);
                }
                page++;
            }
        }

        public async Task<DataResult<People>> GetPeople(int movieId)
        {
            var peopleData = await _movieShiftvDataService.GetPeople(movieId);
            if (peopleData == null)
            {
                peopleData = await _movieTraktDataService.GetPeople(movieId);
                if (peopleData == null) return new DataResult<People>(StandardResults.Error);
                await _movieShiftvDataService.SavePeople(peopleData, movieId);
            }
            return new DataResult<People>(peopleData);
        }

        public async Task<DataResult<List<MiniMovie>>> Search(string key)
        {
            var traktSearch = await _movieTraktDataService.Search(key);
            if (traktSearch == null || traktSearch.Count <= 0)
            {
                var searchData = await _movieShiftvDataService.Search(key);
                return searchData != null ? new DataResult<List<MiniMovie>>(searchData) : new DataResult<List<MiniMovie>>(StandardResults.Error);
            }
            var list = traktSearch.OrderByDescending(x=>x.Score).Select(movie => new MiniMovie
            {
                Fanart = movie.Movie.Images.Fanart,
                Ids =  movie.Movie.Ids,
                Rating =  movie.Movie.Rating,
                Title =  movie.Movie.Title,
                Votes =  movie.Movie.Votes,
                Runtime =  movie.Movie.Runtime,
                Genres =  movie.Movie.Genres,
                Released =  movie.Movie.Released
            }).ToList();
            return new DataResult<List<MiniMovie>>(list);
        }

        public async Task<DataResult<List<Comment>>> GetComments(int movieId, int page, int limit)
        {
            var x = await _movieTraktDataService.GetComments(page, limit, movieId);
            var shiftvComments = await _movieShiftvDataService.GetComments(movieId);
            var list = new List<Comment>();
            if (x != null && x.Count > 0) list.AddRange(x);
            if (shiftvComments != null && shiftvComments.Count > 0) list.AddRange(shiftvComments);
            return new DataResult<List<Comment>>(list);
        }

        public async Task<bool> AddToWatchlist(int id, string token)
        {
            var getUserWatchlist = _movieShiftvDataService.GetUserWatchlist(token);
            if (getUserWatchlist == null) return false;
            if (getUserWatchlist.Any(x => x.Ids.TraktId != null && x.Ids.TraktId.Value == id))
            {
                var first = getUserWatchlist.FirstOrDefault(x => x.Ids.TraktId != null && x.Ids.TraktId.Value == id);
                if (first != null) getUserWatchlist.Remove(first);
                _movieShiftvDataService.UpdateWatchlist(getUserWatchlist, token);
            }
            else
            {
                var movie = await GetMovieById(id, token);
                if (movie.IsOk && movie.Data != null)
                {
                    var minimovie = new MiniMovie
                    {
                        Fanart = movie.Data.Images.Fanart,
                        Votes = movie.Data.Votes,
                        Genres = movie.Data.Genres,
                        Released = movie.Data.Released,
                        Ids = movie.Data.Ids,
                        Runtime = movie.Data.Runtime,
                        Title = movie.Data.Title,
                        Rating = movie.Data.Rating
                    };
                    getUserWatchlist.Add(minimovie);
                    _movieShiftvDataService.UpdateWatchlist(getUserWatchlist, token);
                }
            }
            return true;
        }

        public Task<DataResult<List<MiniMovie>>> GetWatchlist(string token)
        {
            return Task.Run(() =>
            {
                var getUserWatchlist = _movieShiftvDataService.GetUserWatchlist(token);
                if (getUserWatchlist == null) return new DataResult<List<MiniMovie>>(StandardResults.Error);
                CheckUserRatings(token, getUserWatchlist);
                CheckWatched(token, getUserWatchlist);
                CheckWatchlist(token, getUserWatchlist);
                return new DataResult<List<MiniMovie>>(getUserWatchlist);
            });
           
        }
    }
}
