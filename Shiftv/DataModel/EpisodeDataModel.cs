using System;
using Autofac;
using Shiftv.Common;
using Shiftv.Contracts.Domain.Images;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Services;
using Shiftv.Global;
using Shiftv.Helpers;

namespace Shiftv.DataModel
{
    public class EpisodeDataModel : ViewModelBase
    {
        private IEpisode _model;
        private StatisticsDataModel _stats;
        private bool _isSelected;
        private bool _watched;
        private bool _isLoved;
        private bool _isHated;
        private bool _isInWatchlist;
        private int _loved;
        private int _hated;
        private int _plays;
        private bool _isLoadingStats;
        private bool _isLoadToView;
        private double _imageOpacity;
        private bool _isProcessing;

        public EpisodeDataModel(IEpisode model, bool isToLoadStats = true, IFanart showFanart = null)
        {
            if (model == null) return;
            _model = model;
            //IsLoved = _model.IsLoveOrHate && _model.UserRating;
            //IsHated = _model.IsLoveOrHate && !_model.UserRating;
            //InWatchlist = _model.InWatchlist;
            //Loved = _model.Rating.Loved;
            //Hated = _model.Rating.Hated;
            //Plays = _model.Plays;
            ImageOpacity = 1;
            if (isToLoadStats) Load();
            if (_model.Images.Screenshot.Full == null && showFanart != null)
            {
                _model.Images.Screenshot.Full = showFanart.Full;
                _model.Images.Screenshot.Medium = showFanart.Medium;
                _model.Images.Screenshot.Thumb = showFanart.Thumb;
            }
        }



        private async void Load()
        {
            IsLoadingStats = true;
            //var stats = await CoreServices.Stats.GetEpisodeStats(_model.Season, _model.Number);
            //if (stats.IsOk) Statistics = new StatisticsDataModel(stats.Data);
            IsLoadingStats = false;
        }

        public bool IsLoadingStats
        {
            get { return _isLoadingStats; }
            set { SetProperty(ref _isLoadingStats, value); }
        }

        public EpisodeDataModel()
        {
            _model = Ioc.Container.Resolve<IEpisode>();
        }

        public int Season { get { return _model.Season; } }
        public int Number { get { return _model.Number; } }
        public int TvDbId
        {
            get
            {
                return _model.Ids.TvDbId != null ? _model.Ids.TvDbId.Value : 0;
            }
        }
        public string ImdbId { get { return _model.Ids.ImdbId; } }
        public string Title { get { return string.IsNullOrEmpty(_model.Title) ? "TBA" : _model.Title.ToUpper(); } }
        public string Overview { get { return _model.Overview; } }
        public DateTime FirstAired { get { return DateTime.Parse(_model.FirstAired); } }
        public string Screen { get { return _model.Images.Screenshot.Full; } }
        public IImage Image { get { return _model.Images; } }
        public StatisticsDataModel Statistics { get { return _stats; } set { SetProperty(ref _stats, value); } }

        public string SeasonDescription
        {
            get { return string.Format("S{0}E{1}", Season.ToString("00"), Number.ToString("00")); }
        }

        public string FullTitle
        {
            get { return string.Format("{0}x{1} {2}", Season.ToString("00"), Number.ToString("00"), Title); }
        }

        public string AirComplete
        {
            get
            {
                if (_model.FirstAired == null) return ShiftvHelpers.GetTranslation("Tba_Upper");
                if (FirstAired < DateTime.Now) return string.Format("{0} ({1})", FirstAired.ToString("dd-MM-yyyy HH:mm"), ShiftvHelpers.GetTimeZone());
                if (DateTime.Now.Subtract(FirstAired).Days > 7)
                    return string.Format("{0} {1} ({2})", FirstAired.DayOfWeek.ToString().ToUpper(),
                       FirstAired.ToString("HH:mm"), ShiftvHelpers.GetTimeZone());
                return string.Format("{0} ({1})", FirstAired.ToString("dd-MM-yyyy HH:mm"), ShiftvHelpers.GetTimeZone());
            }
        }

        public IEpisode Model { get { return _model; } }

        public string NumberFull
        {
            get { return string.Format("E{0}", Number); }
        }

        public string NumberWithSeason
        {
            get { return string.Format("S{0}E{1}", Season, Number); }
        }

        public string EpisodeLink { get; set; }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        public bool Watched
        {
            get { return _model.Watched; }
        }


        public bool IsInWatchlist
        {
            get { return _isInWatchlist; }
            set { SetProperty(ref _isInWatchlist, value); }
        }
    
        public int Plays
        {
            get { return _plays; }
            set { SetProperty(ref _plays, value); }
        }

        public bool IsLoadToView
        {
            get { return _isLoadToView; }
            set { SetProperty(ref _isLoadToView, value); }
        }

        public double ImageOpacity
        {
            get { return _imageOpacity; }
            set { SetProperty(ref _imageOpacity, value); }
        }

        public bool IsProcessing
        {
            get { return _isProcessing; }
            set { SetProperty(ref _isProcessing, value); }
        }

        public string ShowName { get { return _model.ShowName.ToUpper(); } }

        public string Votes
        {
            get
            {
                if (_model.Votes != null) return string.Format("({0})", ShiftvHelpers.IntToString(_model.Votes.Value));
                return null;
            }
        }

        public string Rating
        {
            get
            {
                if (_model.Rating != null) return Math.Round(_model.Rating.Value, 1).ToString();
                return null;
            }
        }

        public int? RatedValue
        {
            get { return _model.RatedValue; }
        }

        public bool IsRated
        {
            get { return _model.RatedValue != null; }
        }


        public void RefreshData()
        {
            OnPropertyChanged("Watched");
            OnPropertyChanged("RatedValue");
            OnPropertyChanged("IsRated");
            OnPropertyChanged("InWatchlist");
        }

        public void RefreshData(IEpisode data)
        {
            if (data == null) return;
            //IsLoved = data.IsLoveOrHate && data.UserRating;
            //IsHated = data.IsLoveOrHate && !data.UserRating;
            //InWatchlist = data.InWatchlist;
            //Loved = data.Rating.Loved;
            //Hated = data.Rating.Hated;
            //Plays = data.Plays;
            //Watched = data.Watched;
            //var stats = await CoreServices.Stats.GetEpisodeStats(_model.Season, _model.Number);
            //if (stats.IsOk) Statistics = new StatisticsDataModel(stats.Data);
        }
    }
}