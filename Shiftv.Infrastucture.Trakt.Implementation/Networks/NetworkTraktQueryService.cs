using System.Threading.Tasks;
using Shiftv.Contracts.DataServices.Networks;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Networks
{
    public class NetworkTraktQueryService : INetworkTraktQueryService
    {
        public Task<string> GetFollowRequests()
        {
            //http://api.trakt.tv/network/requests/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
       TraktConstants.BaseApiUrl,
       TraktConstants.NetworkAction,
       TraktConstants.RequestsAction,
       TraktConstants.TraktKey));
        }

        public Task<string> ApproveFollowerRequest()
        {
            //http://api.trakt.tv/network/approve/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
       TraktConstants.BaseApiUrl,
       TraktConstants.NetworkAction,
       TraktConstants.ApproveAction,
       TraktConstants.TraktKey));
        }

        public Task<string> DenyFollowerRequest()
        {
            //http://api.trakt.tv/network/deny/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
       TraktConstants.BaseApiUrl,
       TraktConstants.NetworkAction,
       TraktConstants.DenyAction,
       TraktConstants.TraktKey));
        }

        public Task<string> Follow()
        {
            //http://api.trakt.tv/network/follow/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
 TraktConstants.BaseApiUrl,
 TraktConstants.NetworkAction,
 TraktConstants.FollowAction,
 TraktConstants.TraktKey));
        }

        public Task<string> Unfollow()
        {
            //http://api.trakt.tv/network/unfollow/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
 TraktConstants.BaseApiUrl,
 TraktConstants.NetworkAction,
 TraktConstants.UnFollowAction,
 TraktConstants.TraktKey));
        }
    }
}