using System;
using Windows.UI.Xaml.Media.Imaging;
using Shiftv.Common;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Services;
using Shiftv.Helpers;

namespace Shiftv.DataModel
{
    public class MiniShowDataModel : ViewModelBase
    {
        private IMiniShow _model;
        private bool _isAdd;
        private BitmapImage _poster;
        private double _imageOpacity;
        private double _imdbRating;
        private bool _isLoadingData;
        private bool _isSelected;

        public MiniShowDataModel(IMiniShow show)
        {
            if (show == null) return;
            _model = show;
            LoadImage();
            LoadImdb();
        }

        public MiniShowDataModel(IMiniShow show, TileType tileType, bool showProgress = false, bool isAdd = false)
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
            LoadImdb();
        }

        public IMiniShow Model
        {
            get
            {
                return _model;
            }
        }

        public bool ImageLoaded { get; set; }


        public string Network { get { return string.IsNullOrEmpty(_model.Network) ? string.Empty : _model.Network.ToUpper(); } }

        public bool IsAdd
        {
            get { return _isAdd; }
            set { SetProperty(ref _isAdd, value); }
        }
        public TileType TileType { get; set; }

        private async void LoadImdb()
        {
            var imdbRating = await CoreServices.Show.GetImdbRanting(_model.Ids.ImdbId);
            if (imdbRating.IsOk && imdbRating.Data != null) ImdbRating = imdbRating.Data.Value;
        }

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

        private async void LoadImage()
        {
            if (_model == null || _model.Fanart == null)
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
                    uri = _model.Fanart.Full;
                    break;
                case TileType.Normal:
                    uri = _model.Fanart.Medium;
                    break;
                case TileType.DoubleHeight:
                    uri = _model.Fanart.Medium;
                    break;
                default:
                    uri = _model.Fanart.Medium;
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
        public string Title { get { return _model.Title.ToUpper(); } }

        public double ImdbRating
        {
            get { return _imdbRating; }
            set { SetProperty(ref _imdbRating, value); }
        }

        public string Votes
        {
            get
            {
                if (_model.Votes != null) return string.Format("({0})",ShiftvHelpers.IntToString(_model.Votes.Value));
                return null;
            }
        }

        public string Rating
        {
            get
            {
                if (_model.Rating != null) return Math.Round(_model.Rating.Value,1).ToString();
                return null;
            }
        }


        public double ImageOpacity
        {
            get { return _imageOpacity; }
            set { SetProperty(ref _imageOpacity, value); }
        }

        public bool IsLoadingData
        {
            get { return _isLoadingData; }
            set { SetProperty(ref _isLoadingData, value); }
        }


        public bool IsRated
        {
            get { return _model.UserRating != null; }
        }

        public int? RatedValue
        {
            get { return _model.UserRating; }
        }

   

    
        public bool InWatchlist
        {
            get { return false; }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected , value); }
        }

        public void UpdateData()
        {
            OnPropertyChanged("IsRated");
            OnPropertyChanged("RatedValue");
        }
    }
}