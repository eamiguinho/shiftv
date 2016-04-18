using System;
using Shiftv.Common;
using Shiftv.Contracts.Domain.Activity;
using Shiftv.Helpers;

namespace Shiftv.DataModel
{
    public class ActivityDataModel : ViewModelBase
    {
        private bool _isLoadingData;
        private double _imageOpacity;

        public ActivityDataModel(IActivityItem activity)
        {
            User = new UserDataModel(activity.User);
            ActivityType = activity.Type;
            ActivityAction = activity.Action;
            Id = activity.Id;
            Day = activity.When.Day;
            Time = activity.When.Time;
            Elapsed = activity.Elapsed.Short;
            ImageOpacity = 1;
            switch (ActivityType)
            {
                case ActivityTypes.Episode:
                    Show = new ShowDataModel(activity.Show);
                    Episode = new EpisodeDataModel(activity.Episode);
                    TextActivity = string.Format("{0} {1}", ShiftvHelpers.GetTranslation("A"), ShiftvHelpers.GetTranslation("Episode"));
                    Image = Episode.Image.Fanart.Thumb;
                    Title = Episode.FullTitle;
                    Subtitle = Episode.ShowName;
                    break;
                case ActivityTypes.Show:
                    Show = new ShowDataModel(activity.Show);
                    TextActivity = string.Format("{0} {1}", ShiftvHelpers.GetTranslation("A"), ShiftvHelpers.GetTranslation("Show"));
                    Image = Show.Image.Fanart.Thumb;
                    Title = Show.Title;
                    break;
                case ActivityTypes.Movie:
                    Movie = new MovieDataModel(activity.Movie);
                    TextActivity = string.Format("{0} {1}", ShiftvHelpers.GetTranslation("A"), ShiftvHelpers.GetTranslation("Movie"));
                    Image = Movie.Image.Fanart.Thumb;
                    Title = Movie.Title;
                    break;
                case ActivityTypes.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            switch (ActivityAction)
            {
                case ActivityActions.Watching:
                    TextAction = ShiftvHelpers.GetTranslation("StartedWatching");
                    break;
                case ActivityActions.Scrobble:
                    TextAction = ShiftvHelpers.GetTranslation("Scrobbled");
                    break;
                case ActivityActions.Checkin:
                    TextAction = ShiftvHelpers.GetTranslation("Checkin");
                    break;
                case ActivityActions.Seen:
                    TextAction = ShiftvHelpers.GetTranslation("JustSeen");
                    break;
                case ActivityActions.Collection:
                    TextAction = ShiftvHelpers.GetTranslation("AddedToCollection");
                    break;
                case ActivityActions.Rating:
                    TextAction = ShiftvHelpers.GetTranslation("Rated");
                    break;
                case ActivityActions.Watchlist:
                    TextAction =  ShiftvHelpers.GetTranslation("Watchlisted");
                    break;
                case ActivityActions.Shout:
                    TextAction =  ShiftvHelpers.GetTranslation("Shouted");
                    break;
                case ActivityActions.Review:
                    TextAction = ShiftvHelpers.GetTranslation("Reviewed");
                    break;
                case ActivityActions.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        public string Elapsed { get; set; }

        public string Time { get; set; }

        public string Day { get; set; }

        public ActivityActions ActivityAction { get; set; }

        public ActivityTypes ActivityType { get; set; }

        public string Image { get; set; }

        public EpisodeDataModel Episode { get; set; }

        public MovieDataModel Movie { get; set; }

        public ShowDataModel Show { get; set; }

        public UserDataModel User { get; set; }

        public string TextAction { get; set; }
        public string TextActivity { get; set; }

        public string Title { get; set; }
        public string Subtitle { get; set; }
        public int Id { get; set; }

        public string ElapsedFormated
        {
            get {
                return Elapsed == "0s" ? ShiftvHelpers.GetTranslation("JustNow") : string.Format("{0} {1}", Elapsed, ShiftvHelpers.GetTranslation("Ago"));
            }
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
    }
}