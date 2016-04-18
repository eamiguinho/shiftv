using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Media.Imaging;
using Shiftv.Common;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Services;
using Shiftv.Helpers;

namespace Shiftv.DataModel
{
    public class MiniMovieDataModel : ViewModelBase
    {
        private IMiniMovie _model;
        private double _imdbRating;
        private BitmapImage _poster;
        private int _imageOpacity;
        private bool _isAdd;
        private bool _isLoadingData;

        private async void LoadImage()
        {
            ImageLoaded = true;
            ImageOpacity = 1;
            Poster = new BitmapImage(new Uri("ms-appx:///Assets/background.jpg"));
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

        public int ImageOpacity
        {
            get { return _imageOpacity; }
            set { SetProperty(ref _imageOpacity, value); }
        }


        public MiniMovieDataModel(IMiniMovie movie)
        {
            if (movie == null) return;
            _model = movie;
            LoadImage();
            LoadData();
        }

        public MiniMovieDataModel(IMiniMovie movie, TileType tileType, bool isAdd = false)
        {
            if (movie == null) return;
            _model = movie;
            TileType = tileType;
            IsAdd = isAdd;
            var inAppPurchase = App.InAppPurchases;
            if (inAppPurchase != null && inAppPurchase.ProductLicenses["NoAds"].IsActive)
            {
                IsAdd = false;
            }
            if (!isAdd) LoadImage();
            if (!isAdd) LoadData();
        }

        public bool IsAdd
        {
            get { return _isAdd; }
            set { SetProperty(ref _isAdd, value); }
        }

        private async void LoadData()
        {
            //var stats = await CoreServices.Stats.GetShowStats(_model.TvDbId);
            //if (stats.IsOk) Statistics = stats.Data;
            if (_model == null) return;
            //IsLoveOrHate = _model.IsLoveOrHate;
            //IsLoved = _model.IsLoveOrHate && _model.UserRating;
            //IsHated = _model.IsLoveOrHate && !_model.UserRating;
            //Watched = _model.Watched;
            //InWatchlist = _model.InWatchlist;
            //Watchers = _model.Stats != null ? _model.Stats.Watchers : 0;
            //Loved = _model.Ratings != null ? _model.Ratings.Loved : 0;
            //Hated = _model.Ratings != null ? _model.Ratings.Hated : 0;
            var imdbRating = await CoreServices.Movie.GetImdbRanting(_model.Ids.ImdbId);
            if (imdbRating.IsOk && imdbRating.Data != null) ImdbRating = imdbRating.Data.Value;
        }

        public double ImdbRating
        {
            get { return _imdbRating; }
            set { SetProperty(ref _imdbRating, value); }
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



        public string Genres
        {
            get
            {
                return String.Join(", ", _model.Genres.ToArray());
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

        public IMiniMovie ToModel()
        {
            return _model;
        }

        public TileType TileType { get; set; }

        public bool ImageLoaded { get; set; }


        public string Runtime
        {
            get { return string.Format("{0} {1}", _model.Runtime, ShiftvHelpers.GetTranslation("Minutes")); }
        }


        public string MovieInfo
        {
            get
            {
                var genres = GetGenres();
                return string.Format("{0}", genres).ToUpper();
            }
        }

        public string Duration
        {
            get
            {
                return string.Format("{0} {1}", _model.Runtime, ShiftvHelpers.GetTranslation("MinutesReduced"));
            }
        }


        private string GetGenres()
        {
            var genres = _model.Genres;
            if (!genres.Any())
            {
                return string.Empty;
            }
            if (genres.Count == 1)
            {
                return genres.First();
            }
            var firstGenre = genres.First();
            var secondGenre = string.Empty;

            if (genres.Count >= 2)
            {
                foreach (var genre in genres)
                {
                    if (genre != firstGenre)
                    {
                        secondGenre = ", " + genre;
                    }
                }
            }

            return string.Format("{0}{1}", firstGenre, secondGenre);
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
            get { return _model.InWatchlist; }
        }

        public bool Watched
        {
            get { return _model.Watched; }
        }

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

        public bool IsLoadingData
        {
            get { return _isLoadingData; }
            set { SetProperty(ref _isLoadingData, value); }
        }

        public void UpdateData()
        {
            OnPropertyChanged("Rating");
            OnPropertyChanged("IsRated");
            OnPropertyChanged("RatedValue");
            OnPropertyChanged("InWatchlist");
            OnPropertyChanged("Watched");
        }
    }
}