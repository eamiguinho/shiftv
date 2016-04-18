using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Data.Xml.Dom;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Autofac;
using BugSense;
using BugSense.Core.Model;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
//using Microsoft.ApplicationInsights.Telemetry.WindowsStore;
using Microsoft.PlayerFramework;
using Newtonsoft.Json;
using SharpCompress.Reader;
using Shiftv.Common;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Services;
using Shiftv.Contracts.Services.PlatformServices;
using Shiftv.Core.Models.Movies;
using Shiftv.DataModel;
using Shiftv.Global;
using Shiftv.Helpers;
using Shiftv.Views.Network;
using Shiftv.Views.Shows.Player;

namespace Shiftv.ViewModels.Shows.Player
{
    public class EpisodeViewerViewModel : ViewModelBase
    {
        #region PrivateProperties
        private SeasonDataModel _season;
        private EpisodeDataModel _selectedEpisode;
        private ShowDataModel _show;
        private bool _isLoadingLinks;
        private bool _noLinksAvailable;
        private double _x;
        private double _y;
        private MultiObservableCollection<CommentsDataModel> _comments;
        private RelayCommand _addComment;
        private RelayCommand _setLove;
        private RelayCommand _setHate;
        private RelayCommand _setAsUnseen;
        private RelayCommand _setAsSeen;
        private RelayCommand _removeFromWatchList;
        private RelayCommand _addToWatchList;
        private UserDataModel _currentUserAccount;
        private bool _isSpoiler;
        private bool _isReview;
        private Uri _episodeSource;
        private StatisticsDataModel _stats;
        private bool _isTopBarEnabled;
        private RelayCommand _download;
        private double _progressValue;
        private IMediaStream _downloadLink;
        private readonly IDownloadService _downloadService;
        private RelayCommand _checkin;
        private RelayCommand _previousEpisodeClicked;
        private bool _canChooseSubtitles;
        private RelayCommand _nextEpisodeClicked;
        private ObservableCollection<LinkInfoDataModel> _availableStreams;
        private bool _canGoNext;
        private bool _canGoPrevious;
        private bool _alreadyCheckIn;
        private EpisodeViewerDataModel _episodeViewerData;
        private bool _isLoadingSubtitles;
        private bool _subtitlesNotAvailable;
        private bool _isRatingOrWatchlisting;
        private bool _isLoadingComments;
        private bool _noSubtitlesLanguageOnSettings;
        private RelayCommand _setAllAsSeenAndPrevious;
        private RelayCommand _streamsClicked;
        private bool _isStreamsVisible;
        private bool _isTryingToComment;
        private string _newComment;
        private RelayCommand _sendEmailDMCA;
        private bool _isRatingVisible;
        private RelayCommand _openRating;
        private bool _isAppBarOpen;

        #endregion

        public EpisodeViewerViewModel()
        {
            _downloadService = Ioc.Container.Resolve<IDownloadService>();
            if (DesignMode.DesignModeEnabled) LoadEpisodeDesignData();
        }

        #region GenericProperties
        public SeasonDataModel Season
        {
            get { return _season; }
            set
            {
                SetProperty(ref _season, value);
            }
        }

        public ShowDataModel Show
        {
            get { return _show; }
            set
            {
                SetProperty(ref _show, value);
            }
        }

        public Uri EpisodeSource
        {
            get { return _episodeSource; }
            set
            {
                SetProperty(ref _episodeSource, value);
            }
        }

        public List<EpisodeDataModel> Episodes { get { return _season == null ? new List<EpisodeDataModel>() : _season.Episodes.Select(x => new EpisodeDataModel(x, true, Show.Image.Fanart)).ToList(); } }

        public EpisodeDataModel SelectedEpisode
        {
            get { return _selectedEpisode; }
            set
            {
                SetProperty(ref _selectedEpisode, value);
            }
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

        public UserDataModel CurrentUserAccount
        {
            get { return _currentUserAccount; }
            set { SetProperty(ref _currentUserAccount, value); }
        }

        public string NewComment
        {
            get { return _newComment; }
            set { SetProperty(ref _newComment, value); }
        }


        public double ProgressValue
        {
            get { return _progressValue; }
            set { SetProperty(ref _progressValue, value); }
        }


        public StatisticsDataModel Statistics { get { return _stats; } set { SetProperty(ref _stats, value); } }

        public IMediaStream Downloadlink
        {
            get { return _downloadLink; }
            set
            {
                SetProperty(ref _downloadLink, value);
                //OnPropertyChanged("CanDownload");
            }
        }

        public ObservableCollection<LinkInfoDataModel> AvailableStreams
        {
            get { return _availableStreams ?? (_availableStreams = new ObservableCollection<LinkInfoDataModel>()); }
        }
        #endregion

        #region BooleanProperties

        public bool IsLoadingLinks
        {
            get { return _isLoadingLinks; }
            set { SetProperty(ref _isLoadingLinks, value); }
        }

        public bool NoLinksAvailable
        {
            get { return _noLinksAvailable; }
            set { SetProperty(ref _noLinksAvailable, value); }
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


        public bool IsInWatchList
        {
            get { return SelectedEpisode.IsInWatchlist; }
        }

        public bool Watched
        {
            get { return SelectedEpisode != null && SelectedEpisode.Watched; }
        }

        public bool NoCommentsAvailable
        {
            get { return !IsLoadingComments && (Comments == null || Comments.Count == 0); }
        }

        public bool CanGoPrevious
        {
            get { return _canGoPrevious; }
            set { SetProperty(ref _canGoPrevious, value); }
        }
        public bool CanGoNext
        {
            get { return _canGoNext; }
            set { SetProperty(ref _canGoNext, value); }
        }

        public bool IsTopBarEnabled
        {
            get { return _isTopBarEnabled; }
            set { SetProperty(ref _isTopBarEnabled, value); }
        }

        public bool CanDownload
        {
            get { return AvailableStreams != null && AvailableStreams.Count > 0; }
        }

        public bool CanCheckIn
        {
            get { return true; }
        }
        public bool CanChooseSubtitles
        {
            get { return _canChooseSubtitles; }
            set { SetProperty(ref _canChooseSubtitles, value); }
        }

        public bool IsLoadingSubtitles
        {
            get { return _isLoadingSubtitles; }
            set { SetProperty(ref _isLoadingSubtitles, value); }
        }

        public bool SubtitlesNotAvailable
        {
            get { return _subtitlesNotAvailable; }
            set { SetProperty(ref _subtitlesNotAvailable, value); }
        }
        public bool NoSubtitlesLanguageOnSettings
        {
            get { return _noSubtitlesLanguageOnSettings; }
            set { SetProperty(ref _noSubtitlesLanguageOnSettings, value); }
        }

        public bool IsRatingOrWatchlisting
        {
            get { return _isRatingOrWatchlisting; }
            set { SetProperty(ref _isRatingOrWatchlisting, value); }
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

        public bool CanUseAppBarButtons
        {
            get
            {
                if (!IsUserLogged) return false;
                if (IsRatingOrWatchlisting) return false;
                return true;
            }
        }
        #endregion

        #region RelayCommands
        public RelayCommand CheckIn
        {
            get { return _checkin ?? (_checkin = new RelayCommand(CheckInExecute)); }
        }
        public RelayCommand PreviousEpisodeClicked
        {
            get
            {
                return _previousEpisodeClicked ?? (_previousEpisodeClicked = new RelayCommand(PreviousEpisodeClick));
            }
        }

        public RelayCommand NextEpisodeClicked
        {
            get
            {
                return _nextEpisodeClicked ?? (_nextEpisodeClicked = new RelayCommand(NextEpisodeClick));
            }
        }


        public RelayCommand SetLove
        {
            get { return _setLove ?? (_setLove = new RelayCommand(SetEpisodeLove)); }
        }

        public RelayCommand SetHate
        {
            get { return _setHate ?? (_setHate = new RelayCommand(SetEpisodeHate)); }
        }

        public RelayCommand SetAsSeen
        {
            get { return _setAsSeen ?? (_setAsSeen = new RelayCommand(SetEpisodeAsSeen)); }
        }
        public RelayCommand SetAsUnseen
        {
            get { return _setAsUnseen ?? (_setAsUnseen = new RelayCommand(SetEpisodeAsUnSeen)); }
        }

        public RelayCommand AddToWatchList
        {
            get { return _addToWatchList ?? (_addToWatchList = new RelayCommand(AddEpisodeToWatchList)); }
        }

        public RelayCommand RemoveFromWatchList
        {
            get { return _removeFromWatchList ?? (_removeFromWatchList = new RelayCommand(RemoveEpisodeFromWatchList)); }
        }


        public RelayCommand Download
        {
            get { return _download ?? (_download = new RelayCommand(DoDownload)); }
        }

        public RelayCommand AddComment
        {
            get { return _addComment ?? (_addComment = new RelayCommand(AddNewCommentToEpisode)); }
        }

        public RelayCommand StreamsClicked
        {
            get { return _streamsClicked ?? (_streamsClicked = new RelayCommand(StreamsClick)); }
        }

        public bool IsStreamsVisible
        {
            get { return _isStreamsVisible; }
            set { SetProperty(ref _isStreamsVisible, value); }
        }

        public bool IsTryingToComment
        {
            get { return _isTryingToComment; }
            set { SetProperty(ref _isTryingToComment, value); }
        }

        public RelayCommand SendEmailDMCA
        {
            get { return _sendEmailDMCA ?? (_sendEmailDMCA = new RelayCommand(SendEmailDMCAEvent)); }
        }

        private async void SendEmailDMCAEvent()
        {
          
            var playing = AvailableStreams.FirstOrDefault(x => x.IsPlayingNow);
            if(playing == null) return;
            var mail = @"
                Report link:" + playing.Model.EmbbedLink + " <br/> What should be included in this email: <br/>" +
                       @"  The following elements must be included in your copyright infringement claim:
1. An electronic or physical signature of the copyright owner or a person authorized to act on behalf of the owner of an exclusive right that is allegedly infringed.<br/>
2. Identification of the copyrighted work claimed to have been infringed, or if multiple copyrighted works at a single online site are covered by a single notice, a representative list of such works at that site.<br/>
3. Identification of the material that is claimed to be infringing or to be the subject of infringing activity and that is to be removed or access to which is to be disabled, and information reasonably sufficient to permit the Company to locate the material.<br/>
4. Information reasonably sufficient to permit the Company to contact the complaining party, such as an address, telephone number, and, if available, an electronic mail address at which the complaining party may be contacted.<br/>
5. A statement that the complaining party has a good faith belief that use of the material in the manner complained of is not authorized by the copyright owner, its agent, or the law.<br/>
6. A statement that the information in the notice is accurate, and under penalty of perjury, that the complaining party is authorized to act on behalf of the owner of an exclusive right that is allegedly infringed.<br/>
Failure to include all of the above information may result in a delay of the processing or the DCMA notification.<br/>";

            var mailto = new Uri(string.Format("mailto:?to={0}&subject={1}&body={2}", playing.Model.ReportLink, "Report copyright infringement", mail));             
            await Windows.System.Launcher.LaunchUriAsync(mailto);
        }

        private void StreamsClick()
        {
            IsStreamsVisible = !IsStreamsVisible;
        }

        #endregion

        #region PublicMethods
        public void LoadEpisodeData(EpisodeViewerDataModel episodeViewerData)
        {
            var show = CoreServices.Show.GetCurrentShow();
            _episodeViewerData = episodeViewerData;
            Show = new ShowDataModel(show);
            Season = episodeViewerData.Season;
            OnPropertyChanged("Season");
            OnPropertyChanged("Episodes");
            SelectedEpisode = Episodes.FirstOrDefault(x => x.Model.Number == episodeViewerData.SelectedEpisode);
            var getNext = GetNextEpisodeData();
            var getPrev = GetPreviousEpisodeData();
            CanGoNext = getNext != null;
            CanGoPrevious = getPrev != null;
            if (SelectedEpisode.RatedValue != null) RatingValue = SelectedEpisode.RatedValue.Value;
            LoadAsyncData(episodeViewerData, show);
        }


        private async void LoadAsyncData(EpisodeViewerDataModel episodeViewerData, IShow show)
        {
            LoadComments(show, episodeViewerData);
            var user = CoreServices.User.GetCurrentUser();
            if (user != null) CurrentUserAccount = new UserDataModel(user.UserSettings.User);
            var stats = await CoreServices.Stats.GetEpisodeStats(Season.Number, SelectedEpisode.Number);
            if (stats.IsOk) Statistics = new StatisticsDataModel(stats.Data);
        }

        public async void LoadEpisodeDesignData()
        {
            for (int i = 0; i < 4; i++)
            {
                AvailableStreams.Add(new LinkInfoDataModel());
            }
            var show = CoreServices.Show.GetCurrentShow();
            Show = new ShowDataModel(show);
            //Season = new SeasonDataModel(show.Seasons.First(a => a.Number == 1));
            OnPropertyChanged("Season");
            OnPropertyChanged("Episodes");
            SelectedEpisode = new EpisodeDataModel(Season.Episodes.First(a => a.Number == 1),true,Show.Image.Fanart);
            var comments = await CoreServices.Comments.GetEpisodeComments(show, Season.Number,
              1);
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

        public async Task<IMediaStream> GetLinks()
        {
            // if (CanDownload) return Downloadlink;
            IsLoadingLinks = true;
            if (SelectedEpisode.Model.FirstAiredDate > DateTime.Now)
            {
                NoLinksAvailable = true;
                IsLoadingLinks = false;
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


            var links = await CoreServices.Episode.GetEpisodeLink(Season.Number, SelectedEpisode.Number, (SelectedEpisode.FirstAired), subtitlesLanguages, SelectedEpisode.Model.NumberAbs);

            if (!links.IsOk || links.Data.Links.Count == 0)
            {
                NoLinksAvailable = true;
                IsLoadingLinks = false;
                return null;
            }
            Downloadlink = links.Data;
            if (Downloadlink.Links != null && Downloadlink.Links.Count > 0 && Downloadlink.Links.Any(x => x.IsCached == false))
            {
                var runtime = Show.ToModel().Runtime;
                if (runtime != null)
                    Downloadlink.Links = await ShiftvHelpers.CheckFileSizesAndQuality(Downloadlink.Links, runtime.Value != 0 ? runtime.Value : 40);
            }
            //if (Downloadlink.Links != null && Downloadlink.Links.Count > 0 && Downloadlink.Links.Any(x => x.IsCached == false))
            //{
            //    CoreServices.Crawler.SaveOnAzure(Downloadlink.Links.Where(x => x.Quality != StreamQuality.ND).OrderByDescending(x => x.FileSizeFormatted).ToList(), Show.ToModel().Ids.ImdbId, SelectedEpisode.Season, SelectedEpisode.Number);
            //}
            AvailableStreams.Clear();
            foreach (var linkInfo in Downloadlink.Links.Where(x => x.Quality != StreamQuality.ND).OrderBy(x => x.Velocity).ThenByDescending(x=>x.FileSize))
            {
                AvailableStreams.Add(new LinkInfoDataModel(linkInfo, AvailableStreams.Count + 1));
            }
            IsLoadingLinks = false;
            OnPropertyChanged("CanDownload");
            if (AvailableStreams.Count == 0) NoLinksAvailable = true;
            else
            {
                //                Insights.Track("GetEpisodeStream", new Dictionary<string, string> {
                //    {"ShowName", SelectedEpisode.ShowName},
                //    {"Season", SelectedEpisode.Season.ToString("00")},
                //    {"Episode", SelectedEpisode.Number.ToString("00")},
                //    {"Subtitles", subtitlesLanguages},
                //    {"Streams", JsonConvert.SerializeObject(AvailableStreams)}
                //});
                //BugSenseHandler.Instance.SendEventAsync(string.Format("GetEpisodeStream: {0}S{1}E{2}", SelectedEpisode.ShowName, SelectedEpisode.Season, SelectedEpisode.Number));
                //BugSenseHandler.Instance.Flush();
                //var properties = new Windows.Foundation.Collections.PropertySet();
                //properties["ShowName"] = SelectedEpisode.ShowName;
                //properties["Season"] = SelectedEpisode.Season;
                //properties["Episode"] = SelectedEpisode.Number;
                //properties["Subtitles"] = subtitlesLanguages;
                //properties["Streams"] = JsonConvert.SerializeObject(AvailableStreams);
                //ClientAnalyticsChannel.Default.LogEvent("Player/Episodes", properties);
                var tc = new TelemetryClient();
                //var properties = new Dictionary<string, string>();
                //properties.Add("name", Show.Title);
                //properties.Add();
                //properties.Add();

                //// Provide metrics associated with an event:
                //var measurements = new Dictionary<string, double>();
                //measurements.Add("NumLinks", AvailableStreams.Count);

                //tc.TrackPageView("Player/Episodes", properties, measurements);
                var test = new PageViewTelemetry
                {
                    Name = "Player/Episodes",
                    Metrics = {{"NumLinks", AvailableStreams.Count}},
                    Properties =
                    {
                        {"name", Show.Title},
                        {"show-id", Show.ToModel().Ids.ImdbId},
                        {"episodeNumber", SelectedEpisode.NumberWithSeason}
                    }

                };
                tc.TrackPageView(test);
            }
            return Downloadlink;
        }

        public async Task<List<Caption>> LoadCaptions()
        {
            if (Show != null && Show.ToModel().IsAnime) return new List<Caption>();
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
                subtitlesLanguages += "," + secondarySubtitleLanguage;
            }

            if (primarySubtitleLanguage == null && secondarySubtitleLanguage == null)
            {
                IsLoadingSubtitles = false;
                NoSubtitlesLanguageOnSettings = true;
                return new List<Caption>();
            }
            var reqSubs = await CoreServices.Episode.GetEpisodeSubtitles(subtitlesLanguages, SelectedEpisode.Season, SelectedEpisode.Number);
            if (reqSubs.IsOk && reqSubs.Data != null && reqSubs.Data.Subtitles != null &&
                reqSubs.Data.Subtitles.Count > 0)
            {
                return await LoadSubs(reqSubs.Data);
            }
            IsLoadingSubtitles = false;
            SubtitlesNotAvailable = true;
            return new List<Caption>();

        }

        private async Task<List<Caption>> LoadSubs(IMediaStream link)
        {
            var captions = new List<Caption>();
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            var count = 0;
            var currenLang = "";
            if (link.Subtitles == null || link.Subtitles.Count == 0)
            {
                IsLoadingSubtitles = false;
                SubtitlesNotAvailable = true;
                return captions;
            }
            foreach (var subtitlesInfo in link.Subtitles.OrderBy(x => x.LanguageId))
            {
                try
                {
                    if (captions.Count(x => x.Language == subtitlesInfo.Language) >= 4) continue;
                    if (subtitlesInfo.Language != currenLang)
                    {
                        count = 1;
                        currenLang = subtitlesInfo.Language;
                    }
                    var fileZip =
                        await SaveAsync(new Uri(subtitlesInfo.SubtitlesLink), folder, Guid.NewGuid().ToString());
                    var fileUnZip = await UnZipSubtitleFileToFile(fileZip, subtitlesInfo.SubtitleFileName);
                    var subWtt = await ShiftvHelpers.ConvertAndUpload(fileUnZip);
                    await fileZip.DeleteAsync();
                    await fileUnZip.DeleteAsync();
                    var isSubtitleOk = ShiftvHelpers.CheckSubtitle(subWtt);
                    if (isSubtitleOk)
                    {
                        captions.Add(new Caption
                        {
                            Payload = subWtt,
                            Language = subtitlesInfo.LanguageId,
                            Description = string.Format("{0} {1}", subtitlesInfo.Language, count)
                        });
                        count++;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
            IsLoadingSubtitles = false;
            if (captions.Count > 0) CanChooseSubtitles = true;
            else SubtitlesNotAvailable = true;
            return captions;
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
            var res = await CoreServices.Episode.RateEpisode((int)newValue, SelectedEpisode.Model);
            if (!res.IsOk)
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorRatingText_Capital"), ShiftvHelpers.GetTranslation("ErrorRatingTitle_Capital"));
                messageDialog.ShowAsync();
            }
            else
            {
                SelectedEpisode.RefreshData();
            }
            await Task.Delay(400);
            IsRatingVisible = false;
        }

        private void OpenRatingPopUp()
        {
            RatingValue = SelectedEpisode.RatedValue != null ? SelectedEpisode.RatedValue.Value : 0;
            IsAppBarOpen = false;
            IsRatingVisible = true;
        }

        public bool IsAppBarOpen
        {
            get { return _isAppBarOpen; }
            set { SetProperty(ref _isAppBarOpen, value); }
        }

        #endregion

        #region PrivateMethods
        private async void LoadComments(IShow show, EpisodeViewerDataModel episodeViewerData)
        {
            IsLoadingComments = true;
            var list = new List<CommentsDataModel>();
            var comments = await CoreServices.Comments.GetEpisodeComments(show, Season.Number,
                 episodeViewerData.SelectedEpisode);
            if (comments.IsOk && comments.Data != null && comments.Data.Count > 0)
            {
                list.AddRange(comments.Data.Select(comment => new CommentsDataModel(comment)));
            }
            if (list.Count > 0)
            {
                Comments = new MultiObservableCollection<CommentsDataModel>(list.OrderByDescending(x => x.CommentDateTime).Take(10), list.OrderByDescending(x => x.CommentDateTime));
            }
            IsLoadingComments = false;
            OnPropertyChanged("NoCommentsAvailable");
        }


        private void NextEpisodeClick()
        {
            var nextEpisode = GetNextEpisodeData();
            if (nextEpisode != null)
            {
                var data = new EpisodeViewerDataModelMini(nextEpisode.Season.Number, nextEpisode.SelectedEpisode, isNextOrPrevious: true);
                var x = JsonConvert.SerializeObject(data);
                App.RootFrame.Navigate(typeof(EpisodeViewer), x);
            }
        }

        private void PreviousEpisodeClick()
        {
            var previousEpisode = GetPreviousEpisodeData();
            if (previousEpisode != null)
            {
                var data = new EpisodeViewerDataModelMini(previousEpisode.Season.Number, previousEpisode.SelectedEpisode, isNextOrPrevious: true);
                var x = JsonConvert.SerializeObject(data);
                App.RootFrame.Navigate(typeof(EpisodeViewer), x);
            }
        }


        private EpisodeViewerDataModel GetPreviousEpisodeData()
        {
            try
            {
                if (Show == null) return null;
                if (SelectedEpisode == null) return null;
                if (Season == null) return null;
                var currentEpisode = SelectedEpisode.Number;
                if (Season.Episodes == null) return null;
                if (Season.Episodes.Any(x => x.Number == currentEpisode - 1))
                {
                    return new EpisodeViewerDataModel(Season, currentEpisode - 1);
                }
                if (Show.Seasons.Count >= Season.Number - 1)
                {
                    return new EpisodeViewerDataModel(Show.Seasons.FirstOrDefault(x => x.Number == Season.Number - 1), Show.Seasons[Season.Number - 1].Episodes.Count);
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private EpisodeViewerDataModel GetNextEpisodeData()
        {
            try
            {
                if (Show == null) return null;
                if (SelectedEpisode == null) return null;
                if (Season == null) return null;
                var currentEpisode = SelectedEpisode.Number;
                if (Season.Episodes == null) return null;
                if (Season.Episodes.Any(x => x.Number == currentEpisode + 1))
                {
                    return new EpisodeViewerDataModel(Season, currentEpisode + 1);
                }
                if (Show.Seasons.Count >= Season.Number + 1)
                {
                    var s = Show.Seasons.FirstOrDefault(x => x.Number == Season.Number + 1);
                    return s != null ? new EpisodeViewerDataModel(s, 1) : null;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async void CheckInExecute()
        {
            try
            {
                if (!_alreadyCheckIn)
                    await DoCheckIn(false);
            }
            catch (Exception)
            {
            }
        }

        private async Task DoCheckIn(bool isAuto)
        {
            if (_alreadyCheckIn) return;
            if (isAuto) await CoreServices.Episode.CancelCheckIn();
            var res = await CoreServices.Episode.CheckIn(SelectedEpisode.Season, SelectedEpisode.Number);
            switch (res.Result)
            {
                case StandardResults.Ok:
                    switch (res.Data.Status)
                    {
                        case RequestResults.Success:
                            _alreadyCheckIn = true;
                            SendToast(SelectedEpisode.Model, ShiftvHelpers.GetTranslation("JustCheckedInTo"));
                            break;
                        case RequestResults.Failure:

                            var messageDialog1 = new MessageDialog(ShiftvHelpers.GetTranslation(string.Format("{0} {1} {2}", "ErrorCheckInTextRetry_Capital", res.Data.Wait, "Minutes")),
                    ShiftvHelpers.GetTranslation("ErrorCheckInTitle_Capital"));
                            messageDialog1.Commands.Add(new UICommand("Yes", this.CommandInvokedHandler));
                            messageDialog1.Commands.Add(new UICommand("No", this.CommandInvokedHandler));
                            messageDialog1.ShowAsync();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case StandardResults.Offline:
                    var messageDialog2 = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorCheckInText_Capital"),
                   ShiftvHelpers.GetTranslation("ErrorCheckInTitle_Capital"));
                    messageDialog2.ShowAsync();
                    break;
                case StandardResults.Error:
                    var messageDialog3 = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorCheckInText_Capital"),
                    ShiftvHelpers.GetTranslation("ErrorCheckInTitle_Capital"));
                    messageDialog3.ShowAsync();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async void CommandInvokedHandler(IUICommand command)
        {
            if (command.Label == "Yes")
            {
                await CoreServices.Episode.CancelCheckIn();
                await DoCheckIn(false);
            }
        }

        private async void DoDownload()
        {
            var link = await GetLinks();
            var uri = new Uri(link.Links[0].StreamLink);
            _downloadService.DoDownload(uri, SelectedEpisode.Model);
        }


        private async void AddEpisodeToWatchList()
        {
            IsRatingOrWatchlisting = true;
            OnPropertyChanged("CanUseAppBarButtons");
            var res = await CoreServices.Episode.AddToWatchlist(SelectedEpisode.Model);
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
        private async void RemoveEpisodeFromWatchList()
        {
            IsRatingOrWatchlisting = true;
            OnPropertyChanged("CanUseAppBarButtons");
            var res = await CoreServices.Episode.RemoveFromWatchlist(SelectedEpisode.Model);
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

        private async void SetEpisodeAsSeen()
        {
            IsRatingOrWatchlisting = true;
            OnPropertyChanged("CanUseAppBarButtons");
            var res = await CoreServices.Episode.SetAsSeen(SelectedEpisode.Model);
            if (res.IsOk && res.Data.Success)
            {
                UpdatePermissions();
                SelectedEpisode.RefreshData();
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
            var stats = await CoreServices.Stats.GetEpisodeStats(Season.Number, SelectedEpisode.Number);
            if (stats.IsOk) Statistics = new StatisticsDataModel(stats.Data);
        }

        private async void SetEpisodeAsUnSeen()
        {
            IsRatingOrWatchlisting = true;
            OnPropertyChanged("CanUseAppBarButtons");
            var res = await CoreServices.Episode.SetAsUnseen(Season.Number, SelectedEpisode.Number);
            if (res.IsOk && res.Data.Status == RequestResults.Success)
            {
                UpdatePermissions();
                SelectedEpisode.RefreshData();
            }
            else
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorSetAsUnseenText_Capital"), ShiftvHelpers.GetTranslation("ErrorSetAsUnseenTitle_Capital"));
                messageDialog.ShowAsync();
            }
            IsRatingOrWatchlisting = false;
            OnPropertyChanged("CanUseAppBarButtons");
        }

        private async void SetEpisodeHate()
        {
            IsRatingOrWatchlisting = true;
            OnPropertyChanged("CanUseAppBarButtons");
            var res = await CoreServices.Episode.RateEpisode(false, Season.Number, SelectedEpisode.Number);
            if (res.IsOk && res.Data.Status)
            {
                UpdatePermissions();
            }
            else
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorRatingText_Capital"), ShiftvHelpers.GetTranslation("ErrorRatingTitle_Capital"));
                messageDialog.ShowAsync();
            }
            IsRatingOrWatchlisting = false;
            OnPropertyChanged("CanUseAppBarButtons");
        }

        private async void UpdatePermissions()
        {
            var tvDbId = Show.ToModel().Ids.TvDbId;
            if (tvDbId != null)
            {
                var updatedEpisode = await CoreServices.Episode.GetFullEpisodeInfo(tvDbId.Value, SelectedEpisode.Model.Season, SelectedEpisode.Model.Number);
                if (updatedEpisode != null && updatedEpisode.IsOk && updatedEpisode.Data != null)
                {
                    SelectedEpisode.RefreshData(updatedEpisode.Data);
                }
            }
            OnPropertyChanged("IsLoveOrHate");
            OnPropertyChanged("Watched");
            OnPropertyChanged("CanLove");
            OnPropertyChanged("CanHate");
            OnPropertyChanged("IsInWatchList");
            OnPropertyChanged("CanUseAppBarButtons");
        }

        private async void SetEpisodeLove()
        {
            IsRatingOrWatchlisting = true;
            OnPropertyChanged("CanUseAppBarButtons");
            var res = await CoreServices.Episode.RateEpisode(true, Season.Number, SelectedEpisode.Number);
            if (res.IsOk && res.Data.Status)
            {
                UpdatePermissions();
            }
            else
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorRatingText_Capital"), ShiftvHelpers.GetTranslation("ErrorRatingTitle_Capital"));
                messageDialog.ShowAsync();
            }
            IsRatingOrWatchlisting = false;
            OnPropertyChanged("CanUseAppBarButtons");
        }

        private async void AddNewCommentToEpisode()
        {
            if (string.IsNullOrEmpty(NewComment)) return;
            IsTryingToComment = true;
            var resComment = await CoreServices.Comments.CommentEpisode(NewComment, Season.Number, SelectedEpisode.Number, IsSpoiler, IsReview);
            if (!resComment.IsOk)
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorAddingCommentText_Capital"), ShiftvHelpers.GetTranslation("ErrorAddingCommentTitle_Capital"));
                messageDialog.ShowAsync();
            }
            else
            {
                LoadComments(Show.ToModel(), _episodeViewerData);
                IsSpoiler = false;
                IsReview = false;
                NewComment = "";
            }
            IsTryingToComment = false;
        }


        private async static Task<StorageFile> SaveAsync(
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
            using (var originalFileStream = await file.OpenStreamForReadAsync())
            {
                var fileN = await ApplicationData.Current.LocalFolder.CreateFileAsync(subtitleFileName, CreationCollisionOption.ReplaceExisting);
                var reader = ReaderFactory.Open(originalFileStream);
                while (reader.MoveToNextEntry())
                {
                    if (reader.Entry.IsDirectory) continue;
                    var newFileStream = await fileN.OpenStreamForWriteAsync();

                    var streamEntry = new MemoryStream();
                    reader.WriteEntryTo(streamEntry);
                    // buffer for extraction data
                    var data = streamEntry.ToArray();

                    newFileStream.Write(data, 0, data.Length);
                    newFileStream.Flush();
                    newFileStream.Dispose();
                }
                originalFileStream.Flush();
                originalFileStream.Dispose();
                return fileN;
            }
        }

        private void SendToast(IEpisode episode, string text)
        {
            const ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText04;
            var toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
            var toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(episode.Title.ToUpper()));
            toastTextElements[1].AppendChild(toastXml.CreateTextNode(episode.ShowName.ToUpper()));
            toastTextElements[2].AppendChild(toastXml.CreateTextNode(text));
            var toastImageAttributes = toastXml.GetElementsByTagName("image");
            ((XmlElement)toastImageAttributes[0]).SetAttribute("src", episode.Images.Screenshot.Thumb);
            ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", episode.Title);
            var toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        #endregion
    }
}
