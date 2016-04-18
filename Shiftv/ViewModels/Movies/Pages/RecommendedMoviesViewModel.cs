using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class RecommendedMoviesViewModel : MovieGridViewBase
    {
        private ObservableCollection<MovieDataModel> _recommendedMovies;
        

        public RecommendedMoviesViewModel()
        {
            LoadData();
            //BugSenseHandler.Instance.SendEventAsync("Movie/Recommended");
            var tc = new TelemetryClient();
            tc.TrackPageView("Movie/Recommended");
        }

        public ObservableCollection<MovieDataModel> RecommendedMovies
        {
            get { return _recommendedMovies ?? (_recommendedMovies = new ObservableCollection<MovieDataModel>()); }
        }

        public override sealed async void LoadData()
        {
            if (NumberRequested > 100 || IsProcessing) return;
            IsDataLoaded = false;
            ErrorGettingData = false;
            var recommendedMovies = await CoreServices.Movie.GetRecommendations();
            switch (recommendedMovies.Result)
            {
                case StandardResults.Ok:
                    ProcessRecommended(recommendedMovies.Data);
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

        private async void ProcessRecommended(IReadOnlyList<IMovie> x)
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
                        RecommendedMovies.Add(new MovieDataModel(movie, TileType.Big));
                        break;
                    case 1:
                        if (IsToShowAds && !AddShowed && i == 1)
                        {
                            RecommendedMovies.Add(new MovieDataModel(movie, TileType.Normal, true));
                            AddShowed = true;
                            i--;
                        }
                        else
                        {
                            RecommendedMovies.Add(new MovieDataModel(movie, TileType.Normal));
                        }
                        break;
                    case 2:
                        RecommendedMovies.Add(new MovieDataModel(movie, TileType.Normal));
                        break;
                    case 3:
                        RecommendedMovies.Add(new MovieDataModel(movie, TileType.Normal));
                        break;
                    case 4:
                        RecommendedMovies.Add(new MovieDataModel(movie, TileType.DoubleHeight));
                        break;
                }
                count++;
                if (count == 5) count = 0;
            }
             NumberRequested += PageSize; _pageSize = -1;
            OnPropertyChanged("RecommendedMovies");
            IsDataLoaded = true;
            await Task.Delay(1500);
            IsProcessing = false;
        }


    }
}