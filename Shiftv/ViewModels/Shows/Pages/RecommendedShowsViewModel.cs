using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using BugSense;
using Microsoft.ApplicationInsights;
//using Microsoft.ApplicationInsights.Telemetry.WindowsStore;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Services;
using Shiftv.DataModel;
using Shiftv.Global;

namespace Shiftv.ViewModels.Shows.Pages
{
    public class RecommendedShowsViewModel : TvShowGridViewBase
    {
        private ObservableCollection<ShowDataModel> _recommendedShows;
        

        public RecommendedShowsViewModel()
        {
            LoadData();
            //BugSenseHandler.Instance.SendEventAsync("TvShows/Recommended");
            var tc = new TelemetryClient();
            tc.TrackPageView("TvShows/Recommended");
        }

        public ObservableCollection<ShowDataModel> RecommendedShows { get { return _recommendedShows ?? (_recommendedShows = new ObservableCollection<ShowDataModel>()); } }

        public override sealed async void LoadData()
        {
            if (NumberRequested > 100 || IsProcessing) return;
            var user = CoreServices.User.GetCurrentUser();
            if (user != null) CurrentUserAccount = new UserDataModel(user.UserSettings.User);
            var recommendations = await CoreServices.Show.GetRecommendations();
            IsDataLoaded = false;
            ErrorGettingData = false;
            switch (recommendations.Result)
            {
                case StandardResults.Ok:
                    ProcessRecommended(recommendations.Data);
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

        private async void ProcessRecommended(List<IShow> recommendations)
        {
            if (recommendations == null)
            {
                ErrorGettingData = true;
                IsDataLoaded = true;
                return;
            }
            if (recommendations.Count == 0)
            {
                NoDataAvailable = true;
                IsDataLoaded = true;
                return;
            }
            if (NumberRequested >= recommendations.Count)
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
            var numberToBeRequest = NumberRequested + PageSize >= recommendations.Count ? recommendations.Count : NumberRequested + PageSize;
            for (var i = NumberRequested; i < numberToBeRequest; i++)
            {
                var show = recommendations[i];
                switch (count)
                {
                    case 0:
                        RecommendedShows.Add(new ShowDataModel(show, TileType.Big));
                        break;
                    case 1:
                         if (IsToShowAds && !AddShowed && i == 1)
                        {
                            RecommendedShows.Add(new ShowDataModel(show, TileType.Normal, false, true));
                            AddShowed = true;
                            i--;
                        }
                        else
                        {
                            RecommendedShows.Add(new ShowDataModel(show, TileType.Normal));
                        }
                        break;
                    case 2:
                        RecommendedShows.Add(new ShowDataModel(show, TileType.Normal));
                        break;
                    case 3:
                        RecommendedShows.Add(new ShowDataModel(show, TileType.Normal));
                        break;
                    case 4:
                        RecommendedShows.Add(new ShowDataModel(show, TileType.DoubleHeight));
                        break;
                }
                count++;
                if (count == 5) count = 0;
            }
             NumberRequested += PageSize; _pageSize = -1;
            OnPropertyChanged("RecommendedShows");
            IsDataLoaded = true;
            await Task.Delay(1500);
            IsProcessing = false;
        }


    }
}