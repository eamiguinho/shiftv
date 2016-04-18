using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BugSense;
using Microsoft.ApplicationInsights;
//using Microsoft.ApplicationInsights.Telemetry.WindowsStore;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Services;
using Shiftv.DataModel;
using Shiftv.Global;

namespace Shiftv.ViewModels.Movies.Pages
{
    public class FreshMoviesViewModel : MovieGridViewBase
    {
        private ObservableCollection<MiniMovieDataModel> _freshMovies;
        

        public FreshMoviesViewModel()
        {
            LoadData();
            //BugSenseHandler.Instance.SendEventAsync("Movie/Fresh");
            var tc = new TelemetryClient();
            tc.TrackPageView("Movie/Fresh");
        }

        private async void ProcessFreshMovies(IReadOnlyList<IMiniMovie> x)
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
            if (IsProcessing) return;
            IsProcessing = true;
            var count = 0;
            for (int i = NumberRequested; i < NumberRequested + PageSize; i++)
            {
                var movie = x[i];
                switch (count)
                {
                    case 0:
                        FreshMovies.Add(new MiniMovieDataModel(movie, TileType.Big));
                        break;
                    case 1:
                        if (IsToShowAds && !AddShowed && i == 1)
                        {
                            FreshMovies.Add(new MiniMovieDataModel(movie, TileType.Normal, true));
                            AddShowed = true;
                            i--;
                        }
                        else
                        {
                            FreshMovies.Add(new MiniMovieDataModel(movie, TileType.Normal));
                        }
                        break;
                    case 2:
                        FreshMovies.Add(new MiniMovieDataModel(movie, TileType.Normal));
                        break;
                    case 3:
                        FreshMovies.Add(new MiniMovieDataModel(movie, TileType.Normal));
                        break;
                    case 4:
                        FreshMovies.Add(new MiniMovieDataModel(movie, TileType.DoubleHeight));
                        break;
                }
                count++;
                if (count == 5) count = 0;
            }
             NumberRequested += PageSize; _pageSize = -1;
            OnPropertyChanged("FreshMovies");
            IsDataLoaded = true;
            await Task.Delay(1500);
            IsProcessing = false;
        }

        public ObservableCollection<MiniMovieDataModel> FreshMovies
        {
            get { return _freshMovies ?? (_freshMovies = new ObservableCollection<MiniMovieDataModel>()); }
        }

        public override sealed async void LoadData()
        {
            if (NumberRequested > 100 || IsProcessing) return;
            IsDataLoaded = false;
            ErrorGettingData = false;
            var freshMovies = await CoreServices.Movie.GetFresh();
            switch (freshMovies.Result)
            {
                case StandardResults.Ok:
                    ProcessFreshMovies(freshMovies.Data);
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

        public void UpdateChangedItem()
        {
            if (FreshMovies != null)
            {
                var currentMovie = CoreServices.Movie.GetCurrentMovie();
                if (currentMovie == null) return;
                var movie = FreshMovies.FirstOrDefault(x =>
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