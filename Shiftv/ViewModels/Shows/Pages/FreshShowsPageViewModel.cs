using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BugSense;
using Microsoft.ApplicationInsights;
//using Microsoft.ApplicationInsights.Telemetry.WindowsStore;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Services;
using Shiftv.DataModel;
using Shiftv.Global;

namespace Shiftv.ViewModels.Shows.Pages
{
    public class FreshShowsPageViewModel : TvShowGridViewBase
    {
        private ObservableCollection<MiniShowDataModel> _freshShows;
        

        public FreshShowsPageViewModel()
        {
            LoadData();
            //BugSenseHandler.Instance.SendEventAsync("TvShows/Fresh");
            var tc = new TelemetryClient();
            tc.TrackPageView("TvShows/Fresh");
        }

        public ObservableCollection<MiniShowDataModel> FreshShows { get { return _freshShows ?? (_freshShows = new ObservableCollection<MiniShowDataModel>()); } }

        public override sealed async void LoadData()
        {
            if (NumberRequested > 100 || IsProcessing) return;
            IsDataLoaded = false;
            ErrorGettingData = false;
            var freshShows = await CoreServices.Show.GetFresh();
            switch (freshShows.Result)
            {
                case StandardResults.Ok:
                    ProcessFreshShows(freshShows.Data);
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


        private async void ProcessFreshShows(List<IMiniShow> freshShows)
        {
            if (freshShows == null)
            {
                ErrorGettingData = true;
                IsDataLoaded = true;
                return;
            }
            if (freshShows.Count == 0)
            {
                NoDataAvailable = true;
                IsDataLoaded = true;
                return;
            }
            if (NumberRequested >= freshShows.Count)
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
                var show = freshShows[i];
                switch (count)
                {
                    case 0:
                        FreshShows.Add(new MiniShowDataModel(show, TileType.Big));
                        break;
                    case 1:
                           if (IsToShowAds && !AddShowed && i == 1)
                        {
                            FreshShows.Add(new MiniShowDataModel(show, TileType.Normal, false, true));
                            AddShowed = true;
                            i--;
                        }
                        else
                        {
                            FreshShows.Add(new MiniShowDataModel(show, TileType.Normal));
                        }
                        break;
                    case 2:
                        FreshShows.Add(new MiniShowDataModel(show, TileType.Normal));
                        break;
                    case 3:
                        FreshShows.Add(new MiniShowDataModel(show, TileType.Normal));
                        break;
                    case 4:
                        FreshShows.Add(new MiniShowDataModel(show, TileType.DoubleHeight));
                        break;
                }
                count++;
                if (count == 5) count = 0;
            }
             NumberRequested += PageSize; _pageSize = -1;
            OnPropertyChanged("FreshShows");
            IsDataLoaded = true;
            await Task.Delay(1500);
            IsProcessing = false;
        }

        public void UpdateChangedItem()
        {
            if (FreshShows != null)
            {
                var currentShow = CoreServices.Show.GetCurrentShow();
                if (currentShow == null) return;
                var show = FreshShows.FirstOrDefault(x =>
                {
                    var traktId = x.Model.Ids.TraktId;
                    return currentShow.Ids.TraktId != null && (traktId != null && traktId.Value == currentShow.Ids.TraktId.Value);
                });
                if (show != null)
                {
                    show.Model.UserRating = currentShow.UserRating;
                    show.UpdateData();
                }
            }
        }
    }
}