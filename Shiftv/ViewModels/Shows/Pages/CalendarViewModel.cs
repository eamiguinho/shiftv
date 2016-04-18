using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BugSense;
using Microsoft.ApplicationInsights;
//using Microsoft.ApplicationInsights.Telemetry.WindowsStore;
using Newtonsoft.Json;
using Shiftv.Common;
using Shiftv.Contracts.Domain.Calendars;
using Shiftv.Contracts.Services;
using Shiftv.DataModel;
using Shiftv.Global;
using Shiftv.Helpers;
using Shiftv.Views.Shows.Player;

namespace Shiftv.ViewModels.Shows.Pages
{
    public class CalendarViewModel : TvShowGridViewBase
    {
        private readonly ItemCollection _collection = new ItemCollection();
        private ObservableCollection<GroupInfoList> _groupsOrganized;
        private bool _canReadMoreDays;
        private RelayCommand _seeAll;
        private bool _isLoadingData;
        private int _currentDay = 0;
        private DateTime _startDate = new DateTime(DateTime.Now.AddDays(-8).Year, DateTime.Now.AddDays(-8).Month, DateTime.Now.AddDays(-8).Day);
        private DateTime _endDate = DateTime.Now;
        private List<ICalendar> _calendarSaved;

        public CalendarViewModel()
        {
            LoadCalendar();
            //BugSenseHandler.Instance.SendEventAsync("TvShows/Calendar");
            var tc = new TelemetryClient();
            tc.TrackPageView("TvShows/Calendar");
        }
        
        public ObservableCollection<GroupInfoList> GroupsOrganized
        {
            get
            {
                return _groupsOrganized ?? (_groupsOrganized = new ObservableCollection<GroupInfoList>());
            }
        }

        public bool IsLoadingData
        {
            get { return _isLoadingData; }
            set { SetProperty(ref _isLoadingData, value); }
        }

        public class GroupInfoList : List<object>
        {
            public DateTime Key { get; set; }

            public string FullDate
            {
                get
                {
                    if (IsToday()) return ShiftvHelpers.GetTranslation("Today");
                    if (IsTomorrow()) return ShiftvHelpers.GetTranslation("Tomorrow");
                    return IsYesterday() ? ShiftvHelpers.GetTranslation("Yesterday") : string.Format("{0}, {1} {2}", DayOfWeek, Month, Day);
                }
            }

            private bool IsYesterday()
            {
                var yesterday = DateTime.Today.AddDays(-1);
                return Key.Day == yesterday.Day &&
                    Key.Month == yesterday.Month &&
                    Key.Year == yesterday.Year;
            }

            private bool IsTomorrow()
            {
                var tomorrow = DateTime.Today.AddDays(1);
                return Key.Day == tomorrow.Day &&
                    Key.Month == tomorrow.Month &&
                    Key.Year == tomorrow.Year;
            }

            private bool IsToday()
            {
                return Key.Day == DateTime.Now.Day &&
                       Key.Month == DateTime.Now.Month &&
                       Key.Year == DateTime.Now.Year;
            }

            public string Day { get { return Key.Day.ToString("00"); } }

            public string DayOfWeek
            {
                get
                {
                    switch (Key.DayOfWeek)
                    {
                        case System.DayOfWeek.Friday:
                            return ShiftvHelpers.GetTranslation("Fri_Upper");
                        case System.DayOfWeek.Monday:
                            return ShiftvHelpers.GetTranslation("Mon_Upper");
                        case System.DayOfWeek.Saturday:
                            return ShiftvHelpers.GetTranslation("Sat_Upper");
                        case System.DayOfWeek.Sunday:
                            return ShiftvHelpers.GetTranslation("Sun_Upper");
                        case System.DayOfWeek.Thursday:
                            return ShiftvHelpers.GetTranslation("Thu_Upper");
                        case System.DayOfWeek.Tuesday:
                            return ShiftvHelpers.GetTranslation("Tue_Upper");
                        case System.DayOfWeek.Wednesday:
                            return ShiftvHelpers.GetTranslation("Wed_Upper");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            public string Month
            {
                get
                {
                    switch (Key.Month)
                    {
                        case 1:
                            return ShiftvHelpers.GetTranslation("Jan_Upper");
                        case 2:
                            return ShiftvHelpers.GetTranslation("Feb_Upper");
                        case 3:
                            return ShiftvHelpers.GetTranslation("Mar_Upper");
                        case 4:
                            return ShiftvHelpers.GetTranslation("Apr_Upper");
                        case 5:
                            return ShiftvHelpers.GetTranslation("May_Upper");
                        case 6:
                            return ShiftvHelpers.GetTranslation("Jun_Upper");
                        case 7:
                            return ShiftvHelpers.GetTranslation("Jul_Upper");
                        case 8:
                            return ShiftvHelpers.GetTranslation("Aug_Upper");
                        case 9:
                            return ShiftvHelpers.GetTranslation("Sep_Upper");
                        case 10:
                            return ShiftvHelpers.GetTranslation("Oct_Upper");
                        case 11:
                            return ShiftvHelpers.GetTranslation("Nov_Upper");
                        case 12:
                            return ShiftvHelpers.GetTranslation("Dec_Upper");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            public new IEnumerator<object> GetEnumerator()
            {
                return base.GetEnumerator();
            }
        }


        public ItemCollection Collection
        {
            get
            {
                return _collection;
            }
        }


        private async void LoadCalendar()
        {
            if (_currentDay > 150) return;
            ErrorGettingData = false;
            IsDataLoaded = false;
            if (_calendarSaved != null) ProcessCalendar(_calendarSaved);
            else
            {
                var test = await CoreServices.Calendar.GetCalendar();
                switch (test.Result)
                {
                    case StandardResults.Ok:
                        if (_calendarSaved == null) _calendarSaved = test.Data;
                        ProcessCalendar(test.Data);
                        break;
                    case StandardResults.Offline:
                        ErrorGettingData = true;
                        IsDataLoaded = true;
                        break;
                    case StandardResults.Error:
                        ErrorGettingData = true;
                        IsDataLoaded = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void ProcessCalendar(List<ICalendar> test)
        {
            var list = new List<CalendarDataModel>();
            for (int i = 0; i < 7; i++)
            {
                var ping = false;
                _startDate = _startDate.AddDays(1);
                foreach (var calendar in test)
                {
                    foreach (var episode in calendar.Episodes)
                    {
                        if (episode.FirstAiredDate == null) continue;
                        var episodeDay = new DateTime(episode.FirstAiredDate.Value.Year,
                            episode.FirstAiredDate.Value.Month, episode.FirstAiredDate.Value.Day);
                        if (episodeDay != _startDate) continue;
                        ping = true;
                        list.Add(new CalendarDataModel(episode, episodeDay, calendar.Show));
                    }
                }
                if (!ping) list.Add(new CalendarDataModel(null, _startDate, null));
            }

            GenerateGroupList(list);
            IsDataLoaded = true;
        }

        private void GenerateGroupList(List<CalendarDataModel> t)
        {
            var query = from item in t
                        orderby (item).Date
                        group item by (item).Date
                            into g
                            select new { GroupName = g.Key, Items = g };
            foreach (var g in query)
            {
                var info = new GroupInfoList { Key = g.GroupName };
                info.AddRange(g.Items);
                GroupsOrganized.Add(info);
            }
            _canReadMoreDays = true;
        }


        public class ItemCollection : IEnumerable<object>
        {
            private ObservableCollection<ICalendar> itemCollection = new ObservableCollection<ICalendar>();

            public IEnumerator<object> GetEnumerator()
            {
                return itemCollection.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Add(ICalendar item)
            {
                itemCollection.Add(item);
            }
        }

        public void ReadMoreDays()
        {
            if (!_canReadMoreDays) return;
            _canReadMoreDays = false;
            LoadCalendar();
        }

        public async void ClickCommand(object sender, object parameter)
        {
            var arg = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            if (arg == null) return;
            var item = arg.ClickedItem as CalendarDataModel;
            if (item == null) return;
            item.IsLoadingData = true;
            await CoreServices.Show.SetCurrent(item.Show.Model);
            var show = CoreServices.Show.GetCurrentShow();
            if (show == null) return;
            var season = show.Seasons.FirstOrDefault(x => x.Number == item.Episode.Season);
            if (season == null) return;
            if (season.Number == null) return;
            var data = new EpisodeViewerDataModelMini(season.Number.Value, item.Episode.Number);
            item.IsLoadingData = false;
            App.RootFrame.Navigate(typeof(EpisodeViewer), JsonConvert.SerializeObject(data));
        }

        public override void LoadData()
        {
            LoadCalendar();
        }
    }
}
