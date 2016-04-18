using System.Threading.Tasks;
using Shiftv.Contracts.DataServices.Users;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Users
{
    class UserTraktQueryService : IUserTraktQueryService
    {
        public Task<string> GetFollowingByUsername(string username)
        {
            //http://api.trakt.tv/user/network/following.json/73a66219d4b25eba8b2ef444c2405352/justin
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}{4}/{5}/{6}",
              TraktConstants.BaseApiUrl,
              TraktConstants.UserResource,
              TraktConstants.NetworkAction,
              TraktConstants.FollowingAction,
              TraktConstants.QueryType,
              TraktConstants.TraktKey,
              username));
        }

        public Task<string> GetFollowersByUsername(string username)
        {
            //http://api.trakt.tv/user/network/followers.json/73a66219d4b25eba8b2ef444c2405352/justin
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}{4}/{5}/{6}",
              TraktConstants.BaseApiUrl,
              TraktConstants.UserResource,
              TraktConstants.NetworkAction,
              TraktConstants.FollowersAction,
              TraktConstants.QueryType,
              TraktConstants.TraktKey,
              username));
        }

        public Task<string> GetFriendsByUsername(string username)
        {
            //http://api.trakt.tv/user/network/friends.json/73a66219d4b25eba8b2ef444c2405352/justin
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}{4}/{5}/{6}",
              TraktConstants.BaseApiUrl,
              TraktConstants.UserResource,
              TraktConstants.NetworkAction,
              TraktConstants.FriendsAction,
              TraktConstants.QueryType,
              TraktConstants.TraktKey,
              username));
        }

        public Task<string> GetUserProfileByUsername(string username)
        {
            //http://api.trakt.tv/user/profile.json/73a66219d4b25eba8b2ef444c2405352/justin
            return Task.Run(() => string.Format("{0}/{1}/{2}{3}/{4}/{5}",
            TraktConstants.BaseApiUrl,
            TraktConstants.UserResource,
            TraktConstants.ProfileAction,
            TraktConstants.QueryType,
            TraktConstants.TraktKey,
            username));
        }

        public Task<string> SearchUserByKey(string queryText)
        {
            //http://api.trakt.tv/search/users.json/73a66219d4b25eba8b2ef444c2405352?query=justin
            return Task.Run(() => string.Format("{0}/{1}/{2}{3}/{4}?query={5}",
            TraktConstants.BaseApiUrl,
            TraktConstants.SearchResource,
            TraktConstants.UsersAction,
            TraktConstants.QueryType,
            TraktConstants.TraktKey,
            queryText.Replace(" ", "+")));
        }

        public Task<string> GetToken(string authCode)
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
             TraktConstants.ShiftvBaseApiUrl,
             TraktConstants.LoginResource,
             TraktConstants.GetTokenResource,
            authCode));
        }
    }
}
