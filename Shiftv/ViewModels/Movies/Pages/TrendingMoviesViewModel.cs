using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BugSense;
using BugSense.Core.Model;
using Microsoft.ApplicationInsights;
//using Microsoft.ApplicationInsights.Telemetry.WindowsStore;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Services;
using Shiftv.DataModel;
using Shiftv.Global;

namespace Shiftv.ViewModels.Movies.Pages
{
    public class TrendingMoviesViewModel : MovieGridViewBase
    {
        private ObservableCollection<MiniMovieDataModel> _trendingMovies;
        

        public TrendingMoviesViewModel()
        {
            LoadData();
            //BugSenseHandler.Instance.SendEventAsync("Movie/Trending");
            var tc = new TelemetryClient();
            tc.TrackPageView("Movie/Trending");
        }

        public ObservableCollection<MiniMovieDataModel> TrendingMovies { get { return _trendingMovies ?? (_trendingMovies = new ObservableCollection<MiniMovieDataModel>()); } }

        public override sealed async void LoadData()
        {if (IsProcessing) return;
            if (NumberRequested > 100) return;
            IsDataLoaded = false;
            ErrorGettingData = false;
            var trendingMovies = await CoreServices.Movie.GetTrending();
            switch (trendingMovies.Result)
            {
                case StandardResults.Ok:
                    ProcessTrending(trendingMovies.Data);
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

        private async void ProcessTrending(IReadOnlyList<IMiniMovie> x)
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
            for (int i = NumberRequested; i < NumberRequested + PageSize; i++)
            {
                var movie = x[i];
                switch (count)
                {
                    case 0:
                        TrendingMovies.Add(new MiniMovieDataModel(movie, TileType.Big));
                        break;
                    case 1:
                        if (IsToShowAds && !AddShowed && i == 1)
                        {
                            TrendingMovies.Add(new MiniMovieDataModel(movie, TileType.Normal, true));
                            AddShowed = true;
                            i--;
                        }
                        else
                        {
                            TrendingMovies.Add(new MiniMovieDataModel(movie, TileType.Normal));
                        }
                        break;
                    case 2:
                        TrendingMovies.Add(new MiniMovieDataModel(movie, TileType.Normal));
                        break;
                    case 3:
                        TrendingMovies.Add(new MiniMovieDataModel(movie, TileType.Normal));
                        break;
                    case 4:
                        TrendingMovies.Add(new MiniMovieDataModel(movie, TileType.DoubleHeight));
                        break;
                }
                count++;
                if (count == 5) count = 0;
            }
             NumberRequested += PageSize; _pageSize = -1;
            OnPropertyChanged("TrendingMovies");
            IsDataLoaded = true;
           await Task.Delay(1500);
            IsProcessing = false;
        }


        public void UpdateChangedItem()
        {
            if (TrendingMovies != null)
            {
                var currentMovie = CoreServices.Movie.GetCurrentMovie();
                if (currentMovie == null) return;
                var movie = TrendingMovies.FirstOrDefault(x =>
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
    }
}
