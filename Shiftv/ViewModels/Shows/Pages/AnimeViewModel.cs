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
    public class AnimeViewModel : TvShowGridViewBase
    {
        private ObservableCollection<MiniShowDataModel> _animes;
        private DataResult<List<IMiniShow>> _animesDownload;


        public AnimeViewModel()
        {
            LoadData();
            //BugSenseHandler.Instance.SendEventAsync("TvShows/Anime");
            var tc = new TelemetryClient();
            tc.TrackPageView("TvShows/Anime");
        }

        public ObservableCollection<MiniShowDataModel> Animes { get { return _animes ?? (_animes = new ObservableCollection<MiniShowDataModel>()); } }


        public override sealed async void LoadData()
        {
            if (NumberRequested > 100 || IsProcessing) return;
            var user = CoreServices.User.GetCurrentUser();
            if (user != null && CurrentUserAccount == null) CurrentUserAccount = new UserDataModel(user.UserSettings.User);
            IsDataLoaded = false;
            ErrorGettingData = false;
            if(_animesDownload == null || _animesDownload.Data == null) _animesDownload = await CoreServices.Show.GetAnime();
            switch (_animesDownload.Result)
            {
                case StandardResults.Ok:
                    ProcessAnimes(_animesDownload.Data);
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

        private async void ProcessAnimes(List<IMiniShow> animes)
        {
            if (animes == null)
            {
                ErrorGettingData = true;
                IsDataLoaded = true;
                return;
            }
            if (animes.Count == 0)
            {
                NoDataAvailable = true;
                IsDataLoaded = true;
                return;
            }
            if (NumberRequested >= animes.Count)
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
            var numberToBeRequest = NumberRequested + PageSize >= animes.Count ? animes.Count : NumberRequested + PageSize;
            for (int i = NumberRequested; i < numberToBeRequest; i++)
            {
                var show = animes[i];
                switch (count)
                {
                    case 0:
                        Animes.Add(new MiniShowDataModel(show, TileType.Big));
                        break;
                    case 1:
                        if (IsToShowAds && !AddShowed && i == 1)
                        {
                            Animes.Add(new MiniShowDataModel(show, TileType.Normal, false, true));
                            AddShowed = true;
                            i--;
                        }
                        else
                        {
                            Animes.Add(new MiniShowDataModel(show, TileType.Normal));
                        }
                        break;
                    case 2:
                        Animes.Add(new MiniShowDataModel(show, TileType.Normal));
                        break;
                    case 3:
                        Animes.Add(new MiniShowDataModel(show, TileType.Normal));
                        break;
                    case 4:
                        Animes.Add(new MiniShowDataModel(show, TileType.DoubleHeight));
                        break;
                }
                count++;
                if (count == 5) count = 0;
            }
             NumberRequested += PageSize; _pageSize = -1;
            OnPropertyChanged("Animes");
            IsDataLoaded = true;
            await Task.Delay(1500);
            IsProcessing = false;
        }


        public void UpdateChangedItem()
        {
            if (Animes != null)
            {
                var currentShow = CoreServices.Show.GetCurrentShow();
                if (currentShow == null) return;
                var show = Animes.FirstOrDefault(x =>
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
