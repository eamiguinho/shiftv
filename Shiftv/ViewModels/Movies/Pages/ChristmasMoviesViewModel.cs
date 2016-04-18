using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Services;
using Shiftv.DataModel;
using Shiftv.Global;

namespace Shiftv.ViewModels.Movies.Pages
{
    public class ChristmasMoviesViewModel : MovieGridViewBase
    {
        private ObservableCollection<MiniMovieDataModel> _topMovies;
        private DataResult<List<IMiniMovie>> _topMoviesDownload;


        public ChristmasMoviesViewModel()
        {
            LoadData();
            //BugSenseHandler.Instance.SendEventAsync("Movie/OscarsMovies");
            var tc = new TelemetryClient();
            tc.TrackPageView("Movie/ChristmasMovies");
        }

        public override sealed async void LoadData()
        {
            if (NumberRequested > 100 || IsProcessing) return;
            IsDataLoaded = false;
            ErrorGettingData = false;
            if(_topMoviesDownload == null || _topMoviesDownload.Data == null) _topMoviesDownload = await CoreServices.Movie.GetChristmasMovies();
            switch (_topMoviesDownload.Result)
            {
                case StandardResults.Ok:
                    ProcessTopMovies(_topMoviesDownload.Data);
                    break;
                case StandardResults.Offline:
                    if (NumberRequested == 0)
                    {
                        ErrorGettingData = true;
                        IsDataLoaded = true;
                    }
                    break;
                case StandardResults.Error:
                    if (NumberRequested == 0)
                    {
                        ErrorGettingData = true;
                        IsDataLoaded = true;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private async void ProcessTopMovies(IReadOnlyList<IMiniMovie> x)
        {
            if (x == null)
            {
                ErrorGettingData = true;
                IsDataLoaded = true;
                return;
            }
            if (x.Count == 0)
            {
                NoDataAvailable = true;
                IsDataLoaded = true;
                return;
            }
            if (NumberRequested >= x.Count)
            {
                IsDataLoaded = true;
                return;
            }
            if (IsProcessing)
            {
                IsDataLoaded = true;
                return;
            }
            IsProcessing = true;
            var count = 0;
            var numberToBeRequest = NumberRequested + PageSize >= x.Count ? x.Count : NumberRequested + PageSize;
            for (var i = NumberRequested; i < numberToBeRequest; i++)
            {
                var movie = x[i];
                switch (count)
                {
                    case 0:
                        TopMovies.Add(new MiniMovieDataModel(movie, TileType.Big));
                        break;
                    case 1:
                        if (IsToShowAds && !AddShowed && i == 1)
                        {
                            TopMovies.Add(new MiniMovieDataModel(movie, TileType.Normal, true));
                            AddShowed = true;
                            i--;
                        }
                        else
                        {
                            TopMovies.Add(new MiniMovieDataModel(movie, TileType.Normal));
                        }
                        break;
                    case 2:
                        TopMovies.Add(new MiniMovieDataModel(movie, TileType.Normal));
                        break;
                    case 3:
                        TopMovies.Add(new MiniMovieDataModel(movie, TileType.Normal));
                        break;
                    case 4:
                        TopMovies.Add(new MiniMovieDataModel(movie, TileType.DoubleHeight));
                        break;
                }
                count++;
                if (count == 5) count = 0;
            }
            NumberRequested += PageSize; _pageSize = -1;
            OnPropertyChanged("TopMovies");
            IsDataLoaded = true;
            await Task.Delay(1500);
            IsProcessing = false;
        }

        public void UpdateChangedItem()
        {
            if (TopMovies != null)
            {
                var currentMovie = CoreServices.Movie.GetCurrentMovie();
                if (currentMovie == null) return;
                var movie = TopMovies.FirstOrDefault(x =>
                {
                    var traktId = x.ToModel().Ids.TraktId;
                    return currentMovie.Ids.TraktId != null && (traktId != null && traktId.Value == currentMovie.Ids.TraktId.Value);
                });
                if (movie != null)
                {
                    movie.ToModel().InWatchlist = currentMovie.InWatchlist;
                    movie.ToModel().Watched = currentMovie.Watched;
                    movie.ToModel().UserRating = currentMovie.UserRating;
                    movie.UpdateData();
                }
            }
        }




        public ObservableCollection<MiniMovieDataModel> TopMovies
        {
            get { return _topMovies ?? (_topMovies = new ObservableCollection<MiniMovieDataModel>()); }
        }

     
    
    }
}