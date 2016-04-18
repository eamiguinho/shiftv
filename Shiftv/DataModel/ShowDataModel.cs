using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Media.Imaging;
using Shiftv.Common;
using Shiftv.Contracts.Domain.Images;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Domain.Stats;
using Shiftv.Contracts.Services;
using Shiftv.Core.Models.Images;
using Shiftv.Helpers;

namespace Shiftv.DataModel
{
    public class ShowDataModel : ViewModelBase
    {
        private readonly IShow _model;
        private BitmapImage _poster;
        private IStatistics _stats;
        private double _imdbRating;
        private ShowProgressDataModel _progress;
        private bool _isAdd;
        private bool _isLoadingData;
        private double _imageOpacity;
        private bool _isLoved;
        private bool _isHated;
        private bool _isLoveOrHate;
        private int _loved;
        private int _hated;
        private bool _inWatchlist;
        private bool _isSelected;

        public ShowDataModel(IShow show)
        {
            if (show == null) return;
            _model = show;
            LoadImage();
            LoadData();
        }

        public ShowDataModel(IShow show, TileType tileType, bool showProgress = false, bool isAdd = false)
        {
            if (show == null) return;
            _model = show;
            TileType = tileType;
            IsAdd = isAdd;
            var inAppPurchase = App.InAppPurchases;
            if (inAppPurchase != null && inAppPurchase.ProductLicenses["NoAds"].IsActive)
            {
                IsAdd = false;
            }
            if (!IsAdd) LoadImage();
            if (!IsAdd) LoadData(showProgress);
        }


        public bool IsAdd
        {
            get { return _isAdd; }
            set { SetProperty(ref _isAdd, value); }
        }

        public string Title { get { return _model.Title.ToUpper(); } }

        public BitmapImage Poster
        {
            get
            {
                return _poster;
            }
            set
            {
                SetProperty(ref _poster, value);

            }
        }

        public string Network { get { return string.IsNullOrEmpty(_model.Network) ? string.Empty : _model.Network.ToUpper(); } }
        public string NetworkCountry { get { return string.Format("{0} ({1})", _model.Network.ToUpper(), _model.Country); } }

        public IImage Image
        {
            get { return _model.Images; }
        }

        public IStatistics Statistics
        {
            get { return _stats; }
            set { SetProperty(ref _stats, value); }
        }

        public ShowProgressDataModel Progress
        {
            get { return _progress; }
            set { SetProperty(ref _progress, value); }
        }

        public string Live
        {
            get { return string.Format("{0} {1}", _model.Airs.Day.ToUpper(), _model.Airs.Time.ToUpper(), string.Format("({0})", ShiftvHelpers.GetTimeZone())); }
        }

        public string Sinopse
        {
            get { return _model.Overview; }
        }

        public string AirDayFormatted
        {
            get { return string.Format("{0}, {1}", _model.Airs.Day, _model.Airs.Time, string.Format("({0})", ShiftvHelpers.GetTimeZone())); }
        }

        public string Status
        {
            get { return _model.Status; }
        }

        public string Year
        {
            get { return _model.Year.ToString(); }
        }

        public string Genres
        {
            get
            {
                return String.Join(", ", _model.Genres.ToArray());
            }
        }

        public List<SeasonDataModel> Seasons
        {
            get
            {
                return _model != null && _model.Seasons != null ? _model.Seasons.Select(x => new SeasonDataModel(x, _model.Year != null ? _model.Year.Value : DateTime.Now.Year)).ToList() : new List<SeasonDataModel>();
            }
        }

        public List<ActorDataModel> Actors
        {
            get
            {
                return null;
                //return _model != null ? _model.People.Actors.Where(x => !string.IsNullOrEmpty(x.Name) && !string.IsNullOrEmpty(x.Character)).Select(x => new ActorDataModel(x)).ToList() : new List<ActorDataModel>();
            }
        }

        public TileType TileType { get; set; }

        public bool ImageLoaded { get; set; }

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

        public bool InWatchlist
        {
            get { return _inWatchlist; }
            set { SetProperty(ref _inWatchlist, value); }
        }

        public string Runtime
        {
            get { return string.Format("{0} {1}", _model.Runtime, ShiftvHelpers.GetTranslation("Minutes")); }
        }

        public string Country
        {
            get { return _model.Country.ToUpper(); }
        }

       
        public int? RatedValue
        {
            get { return _model.UserRating; }
        }

        public bool IsRated
        {
            get { return _model.UserRating != null; }
        }
   
  
        public double ImdbRating
        {
            get { return _imdbRating; }
            set { SetProperty(ref _imdbRating, value); }
        }

        public bool IsLoadingData
        {
            get { return _isLoadingData; }
            set
            {
                SetProperty(ref _isLoadingData, value);
                ImageOpacity = 0.5;
            }
        }

        public double ImageOpacity
        {
            get { return _imageOpacity; }
            set { SetProperty(ref _imageOpacity, value); }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }


        public IShow ToModel()
        {
            return _model;
        }


        public async void RefreshRatings()
        {
            var stats = await CoreServices.Stats.GetShowStats(_model.Ids.TvDbId.Value);
            if (stats.IsOk) Statistics = stats.Data;
            OnPropertyChanged("Statistics");
        }


        public void RefreshData(IShow updatedShow)
        {
            if (updatedShow == null) return;
            //IsLoveOrHate = updatedShow.IsLoveOrHate;
            //IsLoved = updatedShow.IsLoveOrHate && updatedShow.UserRating;
            //IsHated = updatedShow.IsLoveOrHate && !updatedShow.UserRating;
            //InWatchlist = updatedShow.InWatchlist;
            //Loved = updatedShow.Rating.Loved;
            //Hated = updatedShow.Rating.Hated;
        }

        public void RefreshData()
        {
            OnPropertyChanged("RatedValue");
            OnPropertyChanged("IsRated");
        }



        private async void LoadData(bool showProgress = false)
        {
            //var stats = await CoreServices.Stats.GetShowStats(_model.TvDbId);
            //if (stats.IsOk) Statistics = stats.Data;
            if (_model == null) return;
            //IsLoveOrHate = _model.IsLoveOrHate;
            //IsLoved = _model.IsLoveOrHate && _model.UserRating;
            //IsHated = _model.IsLoveOrHate && !_model.UserRating;
            //InWatchlist = _model.InWatchlist;
            //Loved = _model.Rating != null ? _model.Rating.Loved : 0;
            //Hated = _model.Rating != null ? _model.Rating.Hated : 0;
            if(!showProgress)LoadImdb();
            //if (showProgress)
            //{
            //    var progress = await CoreServices.Show.GetShowProgress(_model.ImdbId);

            //    var x = await CoreServices.Show.GetByImdbId(_model.ImdbId);

            //    //Progress = new ShowProgressDataModel(progress.Data, x.Data);

            //}
        }

        private async void LoadImdb()
        {
            var imdbRating = await CoreServices.Show.GetImdbRanting(_model.Ids.ImdbId);
            if (imdbRating.IsOk && imdbRating.Data != null) ImdbRating = imdbRating.Data.Value;
        }

        private async void LoadImage()
        {
            if (_model == null || _model.Images == null)
            {
                Poster = new BitmapImage(new Uri("ms-appx:///Assets/background.jpg"));
                return;
            }
            ImageOpacity = 1;
            ImageLoaded = true;
            string uri;
            switch (TileType)
            {
                case TileType.Big:
                    uri = _model.Images.Fanart.Full;
                    break;
                case TileType.Normal:
                    uri = _model.Images.Fanart.Medium;
                    break;
                case TileType.DoubleHeight:
                    uri = _model.Images.Fanart.Medium;
                    break;
                default:
                    uri = _model.Images.Fanart.Medium;
                    break;
            }
            if (string.IsNullOrEmpty(uri))
            {
                Poster = new BitmapImage(new Uri("ms-appx:///Assets/noimagethumb.png"));
                ImageLoaded = false;
                OnPropertyChanged("ImageLoaded");
                return;
            }
            Poster = await ImageHelper.GetOtherImageAsync(new Uri(uri));
            Poster.DownloadProgress += (sender, args) =>
            {
                if (args.Progress == 100)
                {
                    ImageLoaded = false;
                    OnPropertyChanged("ImageLoaded");
                }
            };
        }



    }
}
