using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Data.Xml.Dom;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media;
using Autofac;
using BugSense;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
//using Microsoft.ApplicationInsights.Telemetry.WindowsStore;
using Microsoft.PlayerFramework;
using Newtonsoft.Json;
using SharpCompress.Reader;
using Shiftv.Common;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Services;
using Shiftv.Contracts.Services.PlatformServices;
using Shiftv.Core.Models.Shows;
using Shiftv.DataModel;
using Shiftv.Global;
using Shiftv.Helpers;
using Shiftv.Views.Network;

namespace Shiftv.ViewModels.Movies.Player
{
    public class MoviePlayerViewModel : ViewModelBase
    {
        private bool _isLoadingLinks;
        private bool _noLinksAvailable;
        private double _x;
        private double _y;
        private MultiObservableCollection<CommentsDataModel> _comments;
        private RelayCommand _addComment;
        //private RelayCommand _setLove;
        //private RelayCommand _setHate;
        //private RelayCommand _setAsUnseen;
        //private RelayCommand _setAsSeen;
        //private RelayCommand _removeFromWatchList;
        //private RelayCommand _addToWatchList;
        private bool _isLoved;
        private UserDataModel _currentUserAccount;
        private bool _isSpoiler;
        private bool _isReview;
        private bool _isLovedOrHated;
        private bool _isInWatchList;
        private bool _watched;
        //private Uri _episodeSource;
        private StatisticsDataModel _stats;
        private bool _isTopBarEnabled;
        //private RelayCommand _download;
        private double _progressValue;
        private IMediaStream _downloadLink;
        private IDownloadService _downloadService;
        private RelayCommand _checkin;
        private MovieDataModel _movie;
        private bool _canChooseSubtitles;
        private ObservableCollection<LinkInfoDataModel> _availableStreams;
        private RelayCommand _removeFromWatchList;
        private RelayCommand _setAsUnseen;
        private RelayCommand _addToWatchList;
        private RelayCommand _setAsSeen;
        private RelayCommand _setLove;
        private RelayCommand _setHate;
        private bool _alreadyCheckIn;
        private bool _isLoadingSubtitles;
        private bool _subtitlesNotAvailable;
        private bool _isRatingOrWatchlisting;
        private ObservableCollection<Caption> _captions;
        private MediaPlayer _player;
        private bool _isLoadingComments;
        private bool _noSubtitlesLanguageOnSettings;
        private bool _isStreamsVisible;
        private RelayCommand _streamsClicked;
        private bool _isTryingToComment;
        private string _newComment;
        private bool _isRatingVisible;
        private RelayCommand _openRating;
        private bool _isAppBarOpen;
        private string _numLetters;
        private SolidColorBrush _numLettersColorBrush;


        public MoviePlayerViewModel()
        {
            _downloadService = Ioc.Container.Resolve<IDownloadService>();
            if (DesignMode.DesignModeEnabled) LoadEpisodeDesignData();
        }

        public void LoadMovieData(MediaPlayer player)
        {
            _player = player;
            var movie = CoreServices.Movie.GetCurrentMovie();
            Movie = new MovieDataModel(movie);
            LoadUser();
            LoadComments();
        }

        private void LoadUser()
        {
            var user = CoreServices.User.GetCurrentUser();
            if (user != null)
            {
                CurrentUserAccount = new UserDataModel(user.UserSettings.User);
            }
        }



        private async void LoadComments()
        {
            IsLoadingComments = true;
            var list = new List<CommentsDataModel>();
            var comments = await CoreServices.Comments.GetCommentsMovie();
            if (comments.IsOk && comments.Data != null && comments.Data.Count > 0)
            {
                list.AddRange(comments.Data.Select(comment => new CommentsDataModel(comment)));
            }
            if (list.Count > 0)
            {
                Comments = new MultiObservableCollection<CommentsDataModel>(list.OrderByDescending(x => x.CommentDateTime).Take(10), list.OrderByDescending(x => x.CommentDateTime));
            }
            IsLoadingComments = false;

        }

        public async void LoadEpisodeDesignData()
        {
            var show = CoreServices.Movie.GetCurrentMovie();
            Movie = new MovieDataModel(show);
            var comments = await CoreServices.Comments.GetCommentsMovie();
            if (!comments.IsOk || comments.Data == null) return;
            Comments.Clear();
            foreach (var comment in comments.Data)
            {
                Comments.Add(new CommentsDataModel(comment));
            }
            OnPropertyChanged("NoCommentsAvailable");
            var user = CoreServices.User.GetCurrentUser();
            if (user == null) return;
            CurrentUserAccount = new UserDataModel(user.UserSettings.User);
        }


        public MovieDataModel Movie
        {
            get { return _movie; }
            set
            {
                SetProperty(ref _movie, value);
            }
        }

        public bool IsLoadingLinks
        {
            get { return _isLoadingLinks; }
            set { SetProperty(ref _isLoadingLinks, value); }
        }
        public bool IsLoadingComments
        {
            get { return _isLoadingComments; }
            set
            {
                SetProperty(ref _isLoadingComments, value);
                OnPropertyChanged("NoCommentsAvailable");
            }
        }

        public bool NoLinksAvailable
        {
            get { return _noLinksAvailable; }
            set { SetProperty(ref _noLinksAvailable, value); }
        }

        public double X
        {
            get { return _x; }
            set { SetProperty(ref _x, value); }
        }

        public double Y
        {
            get { return _y; }
            set { SetProperty(ref _y, value); }
        }

        public MultiObservableCollection<CommentsDataModel> Comments
        {
            get { return _comments; }
            set { SetProperty(ref _comments, value); }
        }
        public bool IsLoadingSubtitles
        {
            get { return _isLoadingSubtitles; }
            set { SetProperty(ref _isLoadingSubtitles, value); }
        }
        public bool NoSubtitlesLanguageOnSettings
        {
            get { return _noSubtitlesLanguageOnSettings; }
            set { SetProperty(ref _noSubtitlesLanguageOnSettings, value); }
        }
        public UserDataModel CurrentUserAccount
        {
            get { return _currentUserAccount; }
            set { SetProperty(ref _currentUserAccount, value); }
        }

        public string NewComment
        {
            get { return _newComment; }
            set
            {
                SetProperty(ref _newComment, value);
                CommentTextChangedAction();
            }
        }


        public RelayCommand AddComment
        {
            get { return _addComment ?? (_addComment = new RelayCommand(AddNewCommentToEpisode)); }
        }

        public bool IsSpoiler
        {
            get { return _isSpoiler; }
            set { SetProperty(ref _isSpoiler, value); }
        }

        public bool IsReview
        {
            get { return _isReview; }
            set { SetProperty(ref _isReview, value); }
        }




        public double ProgressValue
        {
            get { return _progressValue; }
            set { SetProperty(ref _progressValue, value); }
        }

        public bool CanLove
        {
            get
            {
                if (!Movie.IsLoveOrHate) return true;
                return Movie.IsLoveOrHate && !Movie.IsLoved;
            }
        }

        public bool CanHate
        {
            get
            {
                if (!Movie.IsLoveOrHate) return true;
                return Movie.IsLoveOrHate && Movie.IsLoved;
            }
        }
        public bool IsInWatchList
        {
            get { return Movie.InWatchlist; }
        }

        public bool Watched
        {
            get { return Movie != null && Movie.Watched; }
        }

        public bool NoCommentsAvailable
        {
            get { return !IsLoadingComments && (Comments == null || Comments.Count == 0); }
        }

        public StatisticsDataModel Statistics { get { return _stats; } set { SetProperty(ref _stats, value); } }


    
        public RelayCommand SetAsSeen
        {
            get { return _setAsSeen ?? (_setAsSeen = new RelayCommand(SetMovieAsSeen)); }
        }
        public RelayCommand SetAsUnseen
        {
            get { return _setAsUnseen ?? (_setAsUnseen = new RelayCommand(SetEpisodeAsUnSeen)); }
        }

        public RelayCommand AddToWatchList
        {
            get { return _addToWatchList ?? (_addToWatchList = new RelayCommand(AddMovieToWatchList)); }
        }

        public RelayCommand RemoveFromWatchList
        {
            get { return _removeFromWatchList ?? (_removeFromWatchList = new RelayCommand(RemoveMovieFromWatchList)); }
        }

        public bool IsTopBarEnabled
        {
            get { return _isTopBarEnabled; }
            set { SetProperty(ref _isTopBarEnabled, value); }
        }

        //public RelayCommand Download
        //{
        //    get { return _download ?? (_download = new RelayCommand(DoDownload)); }
        //}

        public bool CanDownload
        {
            get { return AvailableStreams != null && AvailableStreams.Count > 0; }
        }

        public IMediaStream Downloadlink
        {
            get { return _downloadLink; }
            set
            {
                SetProperty(ref _downloadLink, value);
                //  OnPropertyChanged("CanDownload");
            }
        }

        public bool CanCheckIn
        {
            get { return true; }
        }

        public RelayCommand CheckIn
        {
            get { return _checkin ?? (_checkin = new RelayCommand(CheckInExecute)); }
        }

        public ObservableCollection<LinkInfoDataModel> AvailableStreams
        {
            get
            {
                return _availableStreams ?? (_availableStreams = new ObservableCollection<LinkInfoDataModel>());
            }
        }

        private async void CheckInExecute()
        {
            if (!_alreadyCheckIn)
                await DoCheckIn(false);
            //if (_alreadyCheckIn) return;
            //var res = await CoreServices.Movie.CheckIn();
            //if (res.IsOk && res.Data.Status == RequestResults.Success)
            //{
            //    _alreadyCheckIn = true;
            //    SendToast(Movie.Model, "just checked in to");
            //}
            //else
            //{
            //    var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorCheckInText_Capital"), ShiftvHelpers.GetTranslation("ErrorCheckInTitle_Capital"));
            //    messageDialog.ShowAsync();
            //}
        }

        private async Task DoCheckIn(bool isAuto)
        {
            try
            {
                if (_alreadyCheckIn) return;
                _alreadyCheckIn = true;
                if (isAuto) await CoreServices.Movie.CancelCheckIn();
                var res = await CoreServices.Movie.CheckIn();
                switch (res.Result)
                {
                    case StandardResults.Ok:
                        switch (res.Data.Status)
                        {
                            case RequestResults.Success:
                                _alreadyCheckIn = true;
                                SendToast(ShiftvHelpers.GetTranslation("JustCheckedInTo"));
                                break;
                            case RequestResults.Failure:
                                var dlg =
                                    new MessageDialog(
                                        ShiftvHelpers.GetTranslation(string.Format("{0} {1} {2}",
                                            "ErrorCheckInTextRetry_Capital", res.Data.Wait, "Minutes")), ShiftvHelpers.GetTranslation("ErrorCheckInTitle_Capital"));
                                dlg.Commands.Add(new UICommand("Yes", CommandInvokedHandler));
                                dlg.Commands.Add(new UICommand("No", CommandInvokedHandler));
                                await ShiftvHelpers.ShowDialog(dlg);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;
                    case StandardResults.Offline:
                        _alreadyCheckIn = false;
                        var messageDialog2 = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorCheckInText_Capital"),
                            ShiftvHelpers.GetTranslation("ErrorCheckInTitle_Capital"));
                        messageDialog2.ShowAsync();
                        break;
                    case StandardResults.Error:
                        _alreadyCheckIn = false;
                        var messageDialog3 = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorCheckInText_Capital"),
                            ShiftvHelpers.GetTranslation("ErrorCheckInTitle_Capital"));
                        messageDialog3.ShowAsync();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                var messageDialog3 = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorCheckInText_Capital"),
                       ShiftvHelpers.GetTranslation("ErrorCheckInTitle_Capital"));
                messageDialog3.ShowAsync();
            }
        }

        private async void CommandInvokedHandler(IUICommand command)
        {
            if (command.Label == "Yes")
            {
                await CoreServices.Movie.CancelCheckIn();
                await DoCheckIn(false);
            }
            else
            {
                _alreadyCheckIn = false;
            }
        }
        //private async void DoDownload()
        //{
        //    var link = await GetLinks();
        //    var uri = new Uri(link.Links[0].StreamLink);
        //    _downloadService.DoDownload(uri, SelectedEpisode.Model);
        //}


        private async void AddMovieToWatchList()
        {
            IsRatingOrWatchlisting = true;
            OnPropertyChanged("CanUseAppBarButtons");
            var res = await CoreServices.Movie.AddToWatchlist();
            if (res.IsOk && res.Data.Status == RequestResults.Success)
            {
                UpdatePermissions();
            }
            else
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorAddToWatchListText_Capital"), ShiftvHelpers.GetTranslation("ErrorAddToWatchListTitle_Capital"));
                messageDialog.ShowAsync();
            }
            IsRatingOrWatchlisting = false;
            OnPropertyChanged("CanUseAppBarButtons");
        }
        private async void RemoveMovieFromWatchList()
        {
            IsRatingOrWatchlisting = true;
            OnPropertyChanged("CanUseAppBarButtons");
            var res = await CoreServices.Movie.RemoveFromWatchlist();
            if (res.IsOk && res.Data.Status == RequestResults.Success)
            {
                UpdatePermissions();
            }
            else
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorRemoveFromWatchListText_Capital"), ShiftvHelpers.GetTranslation("ErrorRemoveFromWatchListTitle_Capital"));
                messageDialog.ShowAsync();
            }
            IsRatingOrWatchlisting = false;
            OnPropertyChanged("CanUseAppBarButtons");
        }

        private async void SetMovieAsSeen()
        {
            IsRatingOrWatchlisting = true;
            OnPropertyChanged("CanUseAppBarButtons");
            var res = await CoreServices.Movie.SetAsSeen();
            if (res.IsOk && res.Data.Status == RequestResults.Success)
            {
                UpdatePermissions();
                Movie.RefreshData();
            }
            else
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorSetAsSeenText_Capital"), ShiftvHelpers.GetTranslation("ErrorSetAsSeenTitle_Capital"));
                messageDialog.ShowAsync();
            }
            IsRatingOrWatchlisting = false;
            OnPropertyChanged("CanUseAppBarButtons");
        }

        private async void RefreshRatings()
        {
            var stats = await CoreServices.Stats.GetMoviewStats(Movie.Model.Ids.ImdbId);
            if (stats.IsOk) Statistics = new StatisticsDataModel(stats.Data);
        }

        private async void SetEpisodeAsUnSeen()
        {
            IsRatingOrWatchlisting = true;
            OnPropertyChanged("CanUseAppBarButtons");
            var res = await CoreServices.Movie.SetAsUnseen();
            if (res.IsOk && res.Data.Status == RequestResults.Success)
            {
                UpdatePermissions();
                Movie.RefreshData();
            }
            else
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorSetAsUnseenText_Capital"), ShiftvHelpers.GetTranslation("ErrorSetAsUnseenTitle_Capital"));
                messageDialog.ShowAsync();
            }
            IsRatingOrWatchlisting = false;
            OnPropertyChanged("CanUseAppBarButtons");
        }

        private void UpdatePermissions()
        {
            var updatedMovie = CoreServices.Movie.GetCurrentMovie();
            if (updatedMovie != null)
            {
                Movie.RefreshData(updatedMovie);
            }
            OnPropertyChanged("CanLove");
            OnPropertyChanged("CanHate");
            OnPropertyChanged("IsInWatchList");
            OnPropertyChanged("Watched");
            OnPropertyChanged("CanUseAppBarButtons");
        }

        public bool IsRatingOrWatchlisting
        {
            get { return _isRatingOrWatchlisting; }
            set { SetProperty(ref _isRatingOrWatchlisting, value); }
        }

        public bool CanUseAppBarButtons
        {
            get
            {
                if (!IsUserLogged) return false;
                if (IsRatingOrWatchlisting) return false;
                return true;
            }
        }

        public bool IsCommentButtonEnabled
        {
            get
            {
                if (IsTryingToComment) return false;
                var intNumLetters = Convert.ToInt32(NumLetters);
                if (intNumLetters <= 4) return false;
                return true;
            }
        }

        public string NumLetters
        {
            get { return _numLetters; }
            set { SetProperty(ref _numLetters, value); }
        }

        public SolidColorBrush NumLettersColorBrush
        {
            get { return _numLettersColorBrush; }
            set { SetProperty(ref _numLettersColorBrush, value); }
        }   

        private void CommentTextChangedAction()
        {
            var countWords = NewComment.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Count();
            NumLetters = countWords.ToString();
            if (countWords <= 4)
            {
                NumLettersColorBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                NumLettersColorBrush = new SolidColorBrush(Colors.White);
            }
            OnPropertyChanged("IsCommentButtonEnabled");
        }

      
     
        private async void AddNewCommentToEpisode()
        {
            if (string.IsNullOrEmpty(NewComment)) return;
            IsTryingToComment = true;
            var resComment = await CoreServices.Comments.CommentsMovie(NewComment, IsSpoiler, IsReview);
            if (!resComment.IsOk)
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorAddingCommentText_Capital"), ShiftvHelpers.GetTranslation("ErrorAddingCommentTitle_Capital"));
                messageDialog.ShowAsync();
            }
            else
            {
                IsSpoiler = false;
                IsReview = false;
                NewComment = "";
                LoadComments();
            }
            IsTryingToComment = false;
        }


        public async Task<IMediaStream> GetLinks()
        {
            //if (CanDownload) return Downloadlink;
            IsLoadingLinks = true;
            if (DateTime.Parse(Movie.Model.Released) > DateTime.Now)
            {
                NoLinksAvailable = true;
                IsLoadingLinks = false;
                return null;
            }

            var links = await CoreServices.Movie.GetMovieLink();
            if (!links.IsOk || links.Data.Links.Count == 0)
            {
                IsLoadingLinks = false;
                NoLinksAvailable = true;
                return null;
            }
            var localSettings = ApplicationData.Current.LocalSettings;
            var primarySubtitleLanguage = localSettings.Values["PrimaryLanguageSubtitles"];
            var secondarySubtitleLanguage = localSettings.Values["SecondaryLanguageSubtitles"];
            var subtitlesLanguages = "";
            if (primarySubtitleLanguage != null)
            {
                subtitlesLanguages = primarySubtitleLanguage.ToString();
            }
            if (secondarySubtitleLanguage != null)
            {
                subtitlesLanguages += "," + secondarySubtitleLanguage;
            }

            Downloadlink = links.Data;
            if (Downloadlink.Links != null && Downloadlink.Links.Count > 0 &&
                Downloadlink.Links.Any(x => x.IsCached == false))
            {
                Downloadlink.Links =
                    await ShiftvHelpers.CheckFileSizesAndQuality(Downloadlink.Links, Movie.Model.Runtime.Value);
            }
            //if (Downloadlink.Links != null && Downloadlink.Links.Count > 0 && Downloadlink.Links.Any(x => x.IsCached == false))
            //{
            //    CoreServices.Crawler.SaveOnAzure(Downloadlink.Links.Where(x => x.Quality != StreamQuality.ND).OrderByDescending(x => x.FileSizeFormatted).ToList(), Movie.ToModel().Ids.ImdbId, 0, 0);
            //}
            AvailableStreams.Clear();
            foreach (var linkInfo in Downloadlink.Links.Where(x => x.Quality != StreamQuality.ND).OrderByDescending(x => x.FileSize))
            {
                AvailableStreams.Add(new LinkInfoDataModel(linkInfo, AvailableStreams.Count + 1));
            }
            IsLoadingLinks = false;
            OnPropertyChanged("CanDownload");
            if (AvailableStreams.Count == 0) NoLinksAvailable = true;
            else
            {
                //                Insights.Track("GetEpisodeStream", new Dictionary<string, string> {
                //    {"ShowName", Movie.Title},
                //    {"Subtitles", subtitlesLanguages},
                //    {"Streams", JsonConvert.SerializeObject(AvailableStreams)}
                //});
                //BugSenseHandler.Instance.SendEventAsync(string.Format("GetMovieStream: {0}", Movie.Title));
                //BugSenseHandler.Instance.Flush();
                //var properties = new Windows.Foundation.Collections.PropertySet();
                //properties["MovieName"] = Movie.Title;
                //properties["Subtitles"] = subtitlesLanguages;
                //properties["Streams"] = JsonConvert.SerializeObject(AvailableStreams);
                //ClientAnalyticsChannel.Default.LogEvent("Player/Movies", properties);
                //BugSenseHandler.Instance.SendEventAsync("Player/Movies");
                var tc = new TelemetryClient();
                var test = new PageViewTelemetry
                {
                    Name = "Player/Movies",
                    Metrics = { { "NumLinks", AvailableStreams.Count } },
                    Properties =
                    {
                        {"name", Movie.Model.Title},
                        {"movie-id", Movie.Model.Title},
                    }
                };
                tc.TrackPageView(test);
            }
            return Downloadlink;
        }

        public async void LoadCaptions()
        {
            IsLoadingSubtitles = true;
            var localSettings = ApplicationData.Current.LocalSettings;
            var primarySubtitleLanguage = localSettings.Values["PrimaryLanguageSubtitles"];
            var secondarySubtitleLanguage = localSettings.Values["SecondaryLanguageSubtitles"];
            var subtitlesLanguages = "";
            if (primarySubtitleLanguage != null)
            {
                subtitlesLanguages = primarySubtitleLanguage.ToString();
            }
            if (secondarySubtitleLanguage != null)
            {
                if (string.IsNullOrEmpty(subtitlesLanguages)) subtitlesLanguages = secondarySubtitleLanguage.ToString();
                else subtitlesLanguages += "," + secondarySubtitleLanguage;
            }
            if (primarySubtitleLanguage == null && secondarySubtitleLanguage == null)
            {
                IsLoadingSubtitles = false;
                NoSubtitlesLanguageOnSettings = true;
                return;
            }
            var reqSubs = await CoreServices.Movie.GetMovieSubtitles(subtitlesLanguages);
            if (reqSubs.IsOk && reqSubs.Data != null && reqSubs.Data.Subtitles != null &&
                reqSubs.Data.Subtitles.Count > 0)
            {
                await LoadSubs(reqSubs.Data);
            }
            else
            {
                IsLoadingSubtitles = false;
                SubtitlesNotAvailable = true;
            }
        }

        private async Task LoadSubs(IMediaStream link)
        {
            var captions = new List<Caption>();
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            var count = 0;
            var currenLang = "";
            if (link.Subtitles == null || link.Subtitles.Count == 0)
            {
                IsLoadingSubtitles = false;
                SubtitlesNotAvailable = true;
                return;
            }
            foreach (
                var subtitlesInfo in link.Subtitles.Where(x => x.SubtitleFileName.EndsWith(".srt")).OrderBy(x => x.LanguageId))
            {
                try
                {
                    if (captions.Count(x => x.Language == subtitlesInfo.LanguageId) >= 10) continue;
                    if (subtitlesInfo.Language != currenLang)
                    {
                        count = 1;
                        currenLang = subtitlesInfo.Language;
                    }
                    var fileZip =
                        await SaveAsync(new Uri(subtitlesInfo.SubtitlesLink), folder, Guid.NewGuid().ToString());
                    var fileUnZip = await UnZipSubtitleFileToFile(fileZip, subtitlesInfo.SubtitleFileName);
                    var subWTT = await ShiftvHelpers.ConvertAndUpload(fileUnZip);
                    await fileZip.DeleteAsync();
                    await fileUnZip.DeleteAsync();
                    var isSubtitleOk = ShiftvHelpers.CheckSubtitle(subWTT);
                    if (isSubtitleOk)
                    {
                        captions.Add(new Caption
                        {
                            Payload = subWTT,
                            Language = subtitlesInfo.LanguageId,
                            Description = string.Format("{0} {1}", subtitlesInfo.Language, count)
                        });
                        count++;
                    }
                }
                catch (Exception)
                {
                    //System.Console.WriteLine(e);
                }
            }
            IsLoadingSubtitles = false;
            foreach (var caption in captions)
            {
                _player.AvailableCaptions.Add(caption);
            }
            if (captions.Count > 0) CanChooseSubtitles = true;
            else SubtitlesNotAvailable = true;
        }

        public bool CanChooseSubtitles
        {
            get { return _canChooseSubtitles; }
            set { SetProperty(ref _canChooseSubtitles, value); }
        }

        public bool SubtitlesNotAvailable
        {
            get { return _subtitlesNotAvailable; }
            set { SetProperty(ref _subtitlesNotAvailable, value); }
        }

        public ObservableCollection<Caption> Captions
        {
            get { return _captions ?? (_captions = new ObservableCollection<Caption>()); }
        }

        public bool IsStreamsVisible
        {
            get { return _isStreamsVisible; }
            set { SetProperty(ref _isStreamsVisible, value); }
        }

        public RelayCommand StreamsClicked
        {
            get { return _streamsClicked ?? (_streamsClicked = new RelayCommand(StreamsClick)); }
        }

        public bool IsTryingToComment
        {
            get { return _isTryingToComment; }
            set
            {
                SetProperty(ref _isTryingToComment, value);
                OnPropertyChanged("IsCommentButtonEnabled");
            }
        }

        private void StreamsClick()
        {
            IsStreamsVisible = !IsStreamsVisible;
        }

        public async static Task<StorageFile> SaveAsync(
  Uri fileUri,
  StorageFolder folder,
  string fileName)
        {
            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            var downloader = new BackgroundDownloader();
            var download = downloader.CreateDownload(
                fileUri,
                file);
            await download.StartAsync();
            return file;
        }

        private async Task<StorageFile> UnZipSubtitleFileToFile(StorageFile file, string subtitleFileName)
        {
            using (Stream originalFileStream = await file.OpenStreamForReadAsync())
            {
                var fileN = await ApplicationData.Current.LocalFolder.CreateFileAsync(subtitleFileName, CreationCollisionOption.ReplaceExisting);
                var reader = ReaderFactory.Open(originalFileStream);
                while (reader.MoveToNextEntry())
                {
                    if (!reader.Entry.IsDirectory)
                    {
                        Stream newFileStream = await fileN.OpenStreamForWriteAsync();

                        MemoryStream streamEntry = new MemoryStream();
                        reader.WriteEntryTo(streamEntry);
                        // buffer for extraction data
                        byte[] data = streamEntry.ToArray();

                        newFileStream.Write(data, 0, data.Length);
                        newFileStream.Flush();
                        newFileStream.Dispose();
                    }
                }
                originalFileStream.Flush();
                originalFileStream.Dispose();
                return fileN;
            }
        }

        private void SendToast(string text)
        {
            try
            {
                ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText04;
                XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
                XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
                toastTextElements[0].AppendChild(toastXml.CreateTextNode(Movie.Title.ToUpper()));
                toastTextElements[2].AppendChild(toastXml.CreateTextNode(text));
                XmlNodeList toastImageAttributes = toastXml.GetElementsByTagName("image");
                ((XmlElement)toastImageAttributes[0]).SetAttribute("src", Movie.Image.Fanart.Full);
                ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", Movie.Title);
                ToastNotification toast = new ToastNotification(toastXml);
                ToastNotificationManager.CreateToastNotifier().Show(toast);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CommentClick(CommentsDataModel comment)
        {
            if (comment == null) return;
            App.RootFrame.Navigate(typeof(UserProfileView), comment.User.Username);
        }

        public async void AutoCheckinCheck()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values["AutoCheckIn"] == null)
                return;
            bool isAutoCheckInOn;
            var b = bool.TryParse(localSettings.Values["AutoCheckIn"].ToString(), out  isAutoCheckInOn);
            if (b && isAutoCheckInOn && !_alreadyCheckIn)
            {
                await DoCheckIn(true);
            }
        }



        public RelayCommand OpenRating
        {
            get { return _openRating ?? (_openRating = new RelayCommand(OpenRatingPopUp)); }
        }

        public bool IsRatingVisible
        {
            get { return _isRatingVisible; }
            set { SetProperty(ref _isRatingVisible, value); }
        }

        public int RatingValue { get; set; }

        //public RelayCommand RatingValueChanged
        //{
        //    get { return _ratingValueChanged ?? (_ratingValueChanged = new RelayCommand(RatingValueChangedSubmit)); }
        //}

        public async Task RatingValueChangedSubmit(double newValue)
        {
            var res = await CoreServices.Movie.RateMovie((int)newValue);
            if (!res.IsOk)
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorRatingText_Capital"), ShiftvHelpers.GetTranslation("ErrorRatingTitle_Capital"));
                messageDialog.ShowAsync();
            }
            else
            {
                Movie.RefreshData();
            }
            await Task.Delay(400);
            IsRatingVisible = false;
        }

        private void OpenRatingPopUp()
        {
            IsAppBarOpen = false;
            IsRatingVisible = true;
        }

        public bool IsAppBarOpen
        {
            get { return _isAppBarOpen; }
            set { SetProperty(ref _isAppBarOpen, value); }
        }
    }
}
