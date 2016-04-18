using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftvAPI.Infrastucture.Trakt.Implementation.Helpers
{
    namespace Shiftv.Infrastucture.Trakt.Implementation.Helpers
    {
        public class TraktConstants
        {
            public static string TraktKey { get { return "73a66219d4b25eba8b2ef444c2405352"; } }

            public static string TraktDevKey { get { return "503e0d9e43165e57559bcd13940b96f9404f3578"; } }
            public static string TvDbKey { get { return "45E75355C2EA5807"; } }

            public static string BaseApiUrl { get { return "https://api-v2launch.trakt.tv"; } }

            public static string CheaterApiUrl { get { return "http://trakt.tv/api"; } }


            public static string AccountResource { get { return "account"; } }
            public static string UserResource { get { return "user"; } }
            public static string CalendarResource { get { return "calendar"; } }
            public static string MoviesResource { get { return "movies"; } }
            public static string MovieResource { get { return "movie"; } }
            public static string ShowResource { get { return "show"; } }
            public static string ShowsResource { get { return "shows"; } }
            public static string ExtendedData { get { return "extended"; } }
            public static string ExtendedImagesData { get { return "extended=images,full"; } }
            public static string SettingsAction { get { return "settings"; } }
            public static string TestAction { get { return "test"; } }
            public static string CreateAction { get { return "create"; } }
            public static string TredingAction { get { { return "trending"; } } }
            public static string QueryType { get { { return ".json"; } } }
            public static string SummaryAction { get { { return "summary"; } } }
            public static string RecommendationsResource { get { return "recommendations"; } }
            public static string ShowsAction { get { return "shows"; } }
            public static string SearchResource { get { return "search"; } }
            public static string GenresResource { get { return "genres"; } }
            public static string CommentsAction { get { return "comments"; } }
            public static string RateResource { get { return "rate"; } }
            public static string ShowAction { get { return "show"; } }
            public static string CommentResource { get { return "comment"; } }
            public static string ShoutResource { get { return "shout"; } }
            public static string EditAction { get { return "edit"; } }
            public static string TraktAction { get { return "trakt"; } }
            public static string WatchlistAction { get { return "watchlist"; } }
            public static string UnwatchlistAction { get { return "unwatchlist"; } }
            public static string SeasonsAction { get { return "seasons"; } }
            public static string SeenMethod { get { return "seen"; } }
            public static string UnseenMethod { get { return "unseen"; } }
            public static string EpisodeAction { get { return "episode"; } }
            public static string StatsMethod { get { return "stats"; } }
            public static string WatchingnowAction { get { return "watchingnow"; } }
            public static string CheckinMethod { get { return "checkin"; } }
            public static string ActivityResource { get { return "activity"; } }
            public static string CommunityAction { get { return "community"; } }
            public static string FriendsAction { get { return "friends"; } }
            public static string EpisodesAction { get { return "episodes"; } }
            public static string NetworkAction { get { return "network"; } }
            public static string FollowingAction { get { return "following"; } }
            public static string FollowersAction { get { return "followers"; } }
            public static string RequestsAction { get { return "requests"; } }
            public static string ApproveAction { get { return "approve"; } }
            public static string DenyAction { get { return "deny"; } }
            public static string OmdbApi { get { return "http://www.omdbapi.com/?i="; } }
            public static string ProgressAction { get { return "progress"; } }
            public static string WatchedAction { get { return "watched"; } }
            public static string RatingsResource { get { return "ratings"; } }
            public static string ProfileAction { get { return "profile"; } }
            public static string FollowAction { get { return "follow"; } }
            public static string UnFollowAction { get { return "unfollow"; } }
            public static string UsersAction { get { return "users"; } }
            public static string BaseShiftApiUrl { get { return "http://shiftwebservice.azurewebsites.net"; } }
            public static string ApiResource { get { return "api"; } }
            public static string ServerResource { get { return "server"; } }
            public static string TimeAction { get { return "time"; } }
            public static string BaseTvDbApiUrl { get { return "http://thetvdb.com/api"; } }
            public static string GetEpisodeByAirDate { get { return "GetEpisodeByAirDate.php"; } }
            public static string NameMapResoure { get { return "namemap"; } }
            public static string BaseApiV2Url { get { return "http://api.v2.trakt.tv/"; } }
            public static string PopularAction { get { return "popular"; } }
            public static string CancelCheckin { get { return "cancelcheckin"; } }
            public static string TitleString { get { return "recently-aired"; } }
            public static string NormalString { get { return "normal"; } }
            public static string AllString { get { return "all"; } }
            public static string Page { get { return "page"; } }
            public static string Limit { get { return "limit"; } }
            public static string Updates { get { return "updates"; } }
            public static string People { get { return "people"; } }
            public static string Lists { get { return "lists"; } }
            public static string Items { get { return "items"; } }
            public static string CommentsResource { get { return "comments"; } }
            public static string OAuthClientId
            {
                get
                {
                    return "233fcb9838282957f4d5b6f4fdd7d0167bb8344bcd2463eaaa9cfc4a659da9b5";
                }
            }

            public static string OAuthClientSecret
            {
                get
                {
                    return "f8f0a8de8db231e3138f319908ed91bb56152c3357221437fe93b35f8d3cfcc6";
                }
            }

            public static string OAuthRedirectUri { get { return "shiftv://login.com"; } }

            public static string OAuthGrantTypeAuth
            {
                get { return "authorization_code"; }
            }

            public static string OAuthGrantTypeRefresh
            {
                get { return "refresh_token"; }
            }

            public static string OAuth { get { return "oauth"; } }
            public static string OAuthToken { get { return "token"; } }
            public static string CalendarsResource { get { return "calendars"; } }
            public static string SyncResource { get { return "sync"; } }
            public static string LastActivities { get { return "last_activities"; } }
            public static string HistoryResource { get { return "history"; } }
            public static string RemoveResource { get { return "remove"; } }
        }
    }
}
