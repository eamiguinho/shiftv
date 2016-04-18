using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class MyShowsViewModel : TvShowGridViewBase
    {
        private ObservableCollection<ShowDataModel> _myShows;
        

        public MyShowsViewModel()
        {
            LoadData();
            //BugSenseHandler.Instance.SendEventAsync("TvShows/MyShows");
            var tc = new TelemetryClient();
            tc.TrackPageView("TvShows/MyShows");
        }

        public ObservableCollection<ShowDataModel> MyShows { get { return _myShows ?? (_myShows = new ObservableCollection<ShowDataModel>()); } }

        public override sealed async void LoadData()
        {
            if (NumberRequested > 100 || IsProcessing) return;
            var user = CoreServices.User.GetCurrentUser();
            if(user == null) return;
            CurrentUserAccount =  new UserDataModel(user.UserSettings.User);
            IsDataLoaded = false;
            ErrorGettingData = false;
            var myShows = await CoreServices.Show.GetLovedByUser(CurrentUserAccount.Username);
            switch (myShows.Result)
            {
                case StandardResults.Ok:
                    ProcessMyShows(myShows.Data);
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

        private async void ProcessMyShows(List<IShow> myShows)
        {
            if (myShows == null)
            {
                ErrorGettingData = true;
                IsDataLoaded = true;
                return;
            }
            if (myShows.Count == 0)
            {
                NoDataAvailable = true;
                IsDataLoaded = true;
                return;
            }
            if (NumberRequested >= myShows.Count)
            {
                IsDataLoaded = true;
                return;
            }
            if (IsProcessing) return;
            IsProcessing = true;
            var count = 0;
            var numberToBeRequest = NumberRequested + PageSize >= myShows.Count ? myShows.Count : NumberRequested + PageSize;
            for (int i = NumberRequested; i < numberToBeRequest; i++)
            {
                var show = myShows[i];
                switch (count)
                {
                    case 0:
                        MyShows.Add(new ShowDataModel(show, TileType.Big, true));
                        break;
                    case 1:
                          if (IsToShowAds && !AddShowed && i == 1)
                        {
                            MyShows.Add(new ShowDataModel(show, TileType.Normal, false, true));
                            AddShowed = true;
                            i--;
                        }
                        else
                        {
                            MyShows.Add(new ShowDataModel(show, TileType.Normal));
                        }
                        break;
                    case 2:
                        MyShows.Add(new ShowDataModel(show, TileType.Normal, true));
                        break;
                    case 3:
                        MyShows.Add(new ShowDataModel(show, TileType.Normal, true));
                        break;
                    case 4:
                        MyShows.Add(new ShowDataModel(show, TileType.DoubleHeight, true));
                        break;
                }
                count++;
                if (count == 5) count = 0;
            }
             NumberRequested += PageSize; _pageSize = -1;
            OnPropertyChanged("MyShows");
            IsDataLoaded = true;
            await Task.Delay(1500);
            IsProcessing = false;
        }
    }
}
