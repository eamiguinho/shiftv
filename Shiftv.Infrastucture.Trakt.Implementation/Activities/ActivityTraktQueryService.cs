using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shiftv.Contracts.DataServices.Activities;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Activities
{
    public class ActivityTraktQueryService : IActivityTraktQueryService
    {
        public Task<string> GetCommunityActivities()
        {
            //http://api.trakt.tv/activity/community.json/73a66219d4b25eba8b2ef444c2405352
            return Task.Run(() => string.Format("{0}/{1}/{2}{3}/{4}",
            TraktConstants.BaseApiUrl,
            TraktConstants.ActivityResource,
            TraktConstants.CommunityAction,
            TraktConstants.QueryType,
            TraktConstants.TraktKey));
        }

        public Task<string> GetFriendsActivities()
        {
            //http://api.trakt.tv/activity/friends.json/73a66219d4b25eba8b2ef444c2405352
            return Task.Run(() => string.Format("{0}/{1}/{2}{3}/{4}",
            TraktConstants.BaseApiUrl,
            TraktConstants.ActivityResource,
            TraktConstants.FriendsAction,
            TraktConstants.QueryType,
            TraktConstants.TraktKey));
        }

        public Task<string> GetUserActivities(string username)
        {
            //http://api.trakt.tv/activity/user.json/73a66219d4b25eba8b2ef444c2405352/justin
            return Task.Run(() => string.Format("{0}/{1}/{2}{3}/{4}/{5}",
            TraktConstants.BaseApiUrl,
            TraktConstants.ActivityResource,
            TraktConstants.UserResource,
            TraktConstants.QueryType,
            TraktConstants.TraktKey,
            username));
        }
    }
}
