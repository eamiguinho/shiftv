﻿using System;
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
    public class TopImdbShowsPageViewModel : TvShowGridViewBase
    {
        private ObservableCollection<MiniShowDataModel> _topShows;
        private DataResult<List<IMiniShow>> _topShowsDownload;


        public TopImdbShowsPageViewModel()
        {
            LoadData();
            //BugSenseHandler.Instance.SendEventAsync("TvShows/TopImdb");
            var tc = new TelemetryClient();
            tc.TrackPageView("TvShows/TopImdb");
        }

        public ObservableCollection<MiniShowDataModel> TopShows { get { return _topShows ?? (_topShows = new ObservableCollection<MiniShowDataModel>()); } }

        public override sealed async void LoadData()
        {
            if (NumberRequested > 100 || IsProcessing) return;
            IsDataLoaded = false;
            ErrorGettingData = false;
            if (_topShowsDownload == null || _topShowsDownload.Data == null) _topShowsDownload = await CoreServices.Show.GetTopImdb();
            switch (_topShowsDownload.Result)
            {
                case StandardResults.Ok:
                    ProcessTop(_topShowsDownload.Data);
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

        private async void ProcessTop(List<IMiniShow> topShows)
        {
            if (topShows == null)
            {
                ErrorGettingData = true;
                IsDataLoaded = true;
                return;
            }
            if (topShows.Count == 0)
            {
                NoDataAvailable = true;
                IsDataLoaded = true;
                return;
            }
            if (NumberRequested >= topShows.Count)
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
            var numberToBeRequest = NumberRequested + PageSize >= topShows.Count ? topShows.Count : NumberRequested + PageSize;
            for (var i = NumberRequested; i < numberToBeRequest; i++)
            {
                var show = topShows[i];
                switch (count)
                {
                    case 0:
                        TopShows.Add(new MiniShowDataModel(show, TileType.Big));
                        break;
                    case 1:
                        if (IsToShowAds && !AddShowed && i == 1)
                        {
                            TopShows.Add(new MiniShowDataModel(show, TileType.Normal, false, true));
                            AddShowed = true;
                            i--;
                        }
                        else
                        {
                            TopShows.Add(new MiniShowDataModel(show, TileType.Normal));
                        }
                        break;
                    case 2:
                        TopShows.Add(new MiniShowDataModel(show, TileType.Normal));
                        break;
                    case 3:
                        TopShows.Add(new MiniShowDataModel(show, TileType.Normal));
                        break;
                    case 4:
                        TopShows.Add(new MiniShowDataModel(show, TileType.DoubleHeight));
                        break;
                }
                count++;
                if (count == 5) count = 0;
            }
             NumberRequested += PageSize; _pageSize = -1;
            OnPropertyChanged("TopShows");
            IsDataLoaded = true;
            await Task.Delay(1500);
            IsProcessing = false;
        }
        public void UpdateChangedItem()
        {
            if (TopShows != null)
            {
                var currentShow = CoreServices.Show.GetCurrentShow();
                if (currentShow == null) return;
                var show = TopShows.FirstOrDefault(x =>
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
