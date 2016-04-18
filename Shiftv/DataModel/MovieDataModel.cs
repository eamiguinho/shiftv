using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using MyToolkit.Multimedia;
using Shiftv.Common;
using Shiftv.Contracts.Domain.Images;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Domain.Stats;
using Shiftv.Contracts.Services;
using Shiftv.Helpers;

namespace Shiftv.DataModel
{
    public class MovieDataModel : ViewModelBase
    {
        private IMovie _model;
        private BitmapImage _poster;
        private IStatistics _stats;
        private double _imdbRating;
        private bool _isAdd;
        private bool _isLoadingData;
        private double _imageOpacity;
        private int _loved;
        private int _hated;
        private bool _inWatchlist;
        private bool _isLoved;
        private bool _isHated;
        private bool _isLoveOrHate;
        private bool _watched;
        private int _watchers;

        public MovieDataModel(IMovie movie)
        {
            if (movie == null) return;
            _model = movie;
            LoadImage();
            LoadData();
        }

        public MovieDataModel(IMovie movie, TileType tileType, bool isAdd = false)
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

        private async void LoadData()
        {
            //var stats = await CoreServices.Stats.GetShowStats(_model.TvDbId);
            //if (stats.IsOk) Statistics = stats.Data;
            if (_model == null) return;
            //IsLoveOrHate = _model.IsLoveOrHate;
            //IsLoved = _model.IsLoveOrHate && _model.UserRating;
            //IsHated = _model.IsLoveOrHate && !_model.UserRating;
            //Watched = _model.Watched;
           // InWatchlist = _model.InWatchlist;
            //Watchers = _model.Stats != null ? _model.Stats.Watchers : 0;
            //Loved = _model.Ratings != null ? _model.Ratings.Loved : 0;
            //Hated = _model.Ratings != null ? _model.Ratings.Hated : 0;
            var imdbRating = await CoreServices.Movie.GetImdbRanting(_model.Ids.ImdbId);
            if (imdbRating.IsOk && imdbRating.Data != null) ImdbRating = imdbRating.Data.Value;
        }

        public int Watchers
        {
            get { return _watchers; }
            set { _watchers = value; }
        }


        public double ImdbRating
        {
            get { return _imdbRating; }
            set { SetProperty(ref _imdbRating, value); }
        }

        private async void LoadImage()
        {
            ImageLoaded = true;
            ImageOpacity = 1;
            Poster = new BitmapImage(new Uri("ms-appx:///Assets/background.jpg"));
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


        public IImage Image
        {
            get { return _model.Images; }
        }

        public IStatistics Statistics
        {
            get { return _stats; }
            set { SetProperty(ref _stats, value); }
        }


        public string Sinopse
        {
            get { return _model.Overview; }
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


        public List<ActorDataModel> Actors
        {

            get
            {
                return null;
                //return _model != null ? _model.People.Actors.Where(x => !string.IsNullOrEmpty(x.Name) && !string.IsNullOrEmpty(x.Character)).Select(x => new ActorDataModel(x)).ToList() : new List<ActorDataModel>();
            }
        }

        public IMovie ToModel()
        {
            return _model;
        }

        public TileType TileType { get; set; }

        public bool ImageLoaded { get; set; }

        public bool InWatchlist
        {
            get { return _model.InWatchlist; }
        }

        public string Runtime
        {
            get { return string.Format("{0} {1}", _model.Runtime, ShiftvHelpers.GetTranslation("Minutes")); }
        }

        public bool IsLoved
        {
            get { return _isLoved; }
            set { SetProperty(ref _isLoved, value); }
        }

        public bool IsHated
        {
            get { return _isHated; }
            set { SetProperty(ref _isHated, value); }
        }

        public bool IsLoveOrHate
        {
            get { return _isLoveOrHate; }
            set { SetProperty(ref _isLoveOrHate, value); }
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

        public string Released
        {
            get { return DateTime.Parse(_model.Released).ToString("G"); }
        }


        public string TagLine
        {
            get
            {
                return string.IsNullOrEmpty(_model.Tagline) ? string.Empty : string.Format("\"{0}\"", _model.Tagline);
            }
        }

        public async Task<Uri> GetTrailerUrl()
        {
            try
            {
                var youtubeid = GetYoutubeId(_model.Trailer);
                var youtubeUri = await YouTube.GetVideoUriAsync(youtubeid, YouTubeQuality.QualityHigh);
                return youtubeUri.Uri;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private string GetYoutubeId(string trailer)
        {
            var splits = trailer.Split(new[] { "watch?v=" }, StringSplitOptions.RemoveEmptyEntries);
            if (splits.Count() >= 2)
            {
                return splits[1];
            }
            return string.Empty;
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

        public async void RefreshRatings()
        {
            var stats = await CoreServices.Stats.GetMoviewStats(_model.Ids.ImdbId);
            if (stats.IsOk) Statistics = stats.Data;
            OnPropertyChanged("Statistics");
        }
        public void RefreshData()
        {
            OnPropertyChanged("IsRated");
            OnPropertyChanged("RatedValue");
            OnPropertyChanged("Watched");
            OnPropertyChanged("InWatchlist");

        }

        public void RefreshData(IMovie updatedMovie)
        {
            if (updatedMovie == null) return;
            _model = updatedMovie;
            OnPropertyChanged("IsRated");
            OnPropertyChanged("RatedValue");
            OnPropertyChanged("Watched");
            OnPropertyChanged("InWatchlist");
            //IsLoveOrHate = updatedMovie.IsLoveOrHate;
            //IsLoved = updatedMovie.IsLoveOrHate && updatedMovie.UserRating;
            //IsHated = updatedMovie.IsLoveOrHate && !updatedMovie.UserRating;
            //Watched = updatedMovie.Watched;
            //InWatchlist = updatedMovie.InWatchlist;
            //Loved = updatedMovie.Ratings != null ? _model.Ratings.Loved : 0;
            //Hated = updatedMovie.Ratings != null ? _model.Ratings.Hated : 0;
            //Watchers = updatedMovie.Stats != null ? _model.Stats.Watchers : 0;
        }

        public IMovie Model
        {
            get { return _model; }
        }

        public bool IsAdd
        {
            get { return _isAdd; }
            set { SetProperty(ref _isAdd, value); }
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

        public bool IsRated
        {
            get { return _model.UserRating != null; }
        }

        public int? RatedValue
        {
            get { return _model.UserRating; }
        }


        public double ImageOpacity
        {
            get { return _imageOpacity; }
            set { SetProperty(ref _imageOpacity, value); }
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
    }
}
