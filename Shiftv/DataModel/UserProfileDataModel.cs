using System.Collections.Generic;
using System.Linq;
using Shiftv.Common;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Contracts.Services;
using Shiftv.Helpers;

namespace Shiftv.DataModel
{
    public class UserProfileDataModel :ViewModelBase
    {
        private bool _isGold;
        private bool _isSilver;

        public UserProfileDataModel(IUserProfile user)
        {
            if (user.Username == null)
            {
                Aka = string.Format("{0} :(", ShiftvHelpers.GetTranslation("ProtectedProfile_Upper"));
                LatestName = ShiftvHelpers.GetTranslation("NoIdea_Upper");
                Age = ShiftvHelpers.GetTranslation("NoAge_Upper");
                Location = ShiftvHelpers.GetTranslation("NoAge_Upper"); ;
                Avatar = ShiftvHelpers.GetTranslation("Neverland_Upper"); ;
                IsProtected = true;
                return;
            }
            Username = user.Username.ToUpper();
            Avatar = user.Avatar;
            FullName = string.IsNullOrEmpty(user.FullName) ? "" : user.FullName.ToUpper();
            var firstname = string.IsNullOrEmpty(user.FullName) ? new string[0] : user.FullName.Split(' ');
            Aka = firstname.Length > 0 ? firstname[0].ToUpper() + " aka " + user.Username.ToUpper() : user.Username.ToUpper();
            Location = string.IsNullOrEmpty(user.Location) ? ShiftvHelpers.GetTranslation("Neverland_Upper") : user.Location.ToUpper();
            Age = user.Age != null ? user.Age.Value.ToString() : ShiftvHelpers.GetTranslation("Over9000_Upper");
            Gender = string.IsNullOrEmpty(user.Gender) ? ShiftvHelpers.GetTranslation("Hybrid_Upper") : user.Gender.ToUpper();
            IsMale = Gender == ShiftvHelpers.GetTranslation("Male_Upper") || Gender == ShiftvHelpers.GetTranslation("Hybrid_Upper");
            IsFemale = Gender == ShiftvHelpers.GetTranslation("Female_Upper");
            IsVip = user.IsVip;
            About = string.IsNullOrEmpty(user.About) ? "" : string.Format("\"" + user.About + "\"");
            Info = Age + ", " + Gender;
            LoadStatus(user);
            if (user.Stats != null)
            {
                TotalWatchedEpisodes = user.Stats.Episodes != null ? user.Stats.Episodes.Watched.ToString() : "";
                TotalWatchedMovies = user.Stats.Movies != null ? user.Stats.Movies.Watched.ToString() : "";
                TotalWatchedShows = user.Stats.Shows != null ? user.Stats.Shows.Watched.ToString() : "";
                TotalLovedShows = user.Stats.Shows != null ? user.Stats.Shows.Loved.ToString() : "";
                TotalLovedEpisodes = user.Stats.Episodes != null ? user.Stats.Episodes.Loved.ToString() : "";
                TotalLovedMovies = user.Stats.Movies != null ? user.Stats.Movies.Loved.ToString() : "";
                TotalHatedShows = user.Stats.Shows != null ? user.Stats.Shows.Hated.ToString() : "";
                TotalHatedEpisodes = user.Stats.Episodes != null ? user.Stats.Episodes.Hated.ToString() : "";
                TotalHatedMovies = user.Stats.Movies != null ? user.Stats.Movies.Hated.ToString() : "";
                TotalShoutsShows = user.Stats.Shows != null ? user.Stats.Shows.Shouts.ToString() : "";
                TotalShoutsEpisodes = user.Stats.Episodes != null ? user.Stats.Episodes.Shouts.ToString() : "";
                TotalShoutsMovies = user.Stats.Movies != null ? user.Stats.Movies.Shouts.ToString() : "";

                TotalFriends = user.Stats.Friends.ToString();

                WatchedShows = new List<ShowDataModel>();
                WatchedMovies = new List<MovieDataModel>();
                foreach (var watched in user.Watched.OrderBy(x => ShiftvHelpers.GetDateTimeFromUtcTicks(x.Watched)))
                {
                    if (watched.Type == "episode" || watched.Type == "show")
                    {
                        WatchedShows.Add(new ShowDataModel(watched.Show));
                        if (string.IsNullOrEmpty(UriLatest))
                        {
                            UriLatest = watched.Show.Images.Fanart.Full;
                            LatestName = watched.Show.Title.ToUpper();
                        }
                    }
                    else
                    {
                        WatchedMovies.Add(new MovieDataModel(watched.Movie));
                        if (string.IsNullOrEmpty(UriLatest))
                        {
                            UriLatest = watched.Movie.Images.Fanart.Full;
                            LatestName = watched.Movie.Title.ToUpper();
                        }
                    }
                }
            }

        }

        private async void LoadStatus(IUserProfile user)
        {
            var status = await CoreServices.User.GetUserStats(user.Username);
            if (status.IsOk && status.Data != null)
            {
                IsGold = status.Data.IsGold;
                IsSilver = status.Data.IsSilver;
            }
        }
        public bool IsGold
        {
            get { return _isGold; }
            set { SetProperty(ref _isGold, value); }
        }

        public bool IsSilver
        {
            get { return _isSilver; }
            set { SetProperty(ref _isSilver, value); }
        }
        public string TotalShoutsMovies { get; set; }

        public string TotalShoutsShows { get; set; }

        public string TotalShoutsEpisodes { get; set; }

        public string TotalHatedMovies { get; set; }

        public string TotalHatedEpisodes { get; set; }

        public string TotalHatedShows { get; set; }

        public string TotalLovedMovies { get; set; }

        public string TotalLovedEpisodes { get; set; }

        public string TotalLovedShows { get; set; }

        public string TotalFriends { get; set; }

        public string TotalWatchedShows { get; set; }

        public string TotalWatchedMovies { get; set; }

        public string TotalWatchedEpisodes { get; set; }

        public List<ShowDataModel> WatchedShows { get; set; }
        public List<MovieDataModel> WatchedMovies { get; set; }

        public string Aka { get; set; }
        public string UriLatest { get; set; }

        public string Avatar { get; set; }

        public string Username { get; set; }
        public string FullName { get; set; }
        public string Location { get; set; }
        public string Gender { get; set; }
        public string About { get; set; }
        public string Age { get; set; }
        public bool IsVip { get; set; }

        public bool IsMale { get; set; }
        public bool IsFemale { get; set; }

        public string LatestName { get; set; }
        public string Info { get; set; }
        public bool IsProtected { get; set; }
    }
}