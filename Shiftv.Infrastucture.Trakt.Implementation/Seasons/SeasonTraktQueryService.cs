using System.Threading.Tasks;
using Shiftv.Contracts.DataServices.Seasons;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Seasons
{
    class SeasonTraktQueryService : ISeasonTraktQueryService
    {
        public Task<string> SetSeasonAsSeen()
        {
            //http://api.trakt.tv/show/season/seen/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}/{4}",
          TraktConstants.BaseApiUrl,
          TraktConstants.ShowResource,
          TraktConstants.SeasonAction,
          TraktConstants.SeenMethod,
          TraktConstants.TraktKey));
        }

        public Task<string> SetSeasonAsUnSeen()
        {
            //http://api.trakt.tv/show/season/unseen/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}/{4}",
          TraktConstants.BaseApiUrl,
          TraktConstants.ShowResource,
          TraktConstants.SeasonAction,
          TraktConstants.UnseenMethod,
          TraktConstants.TraktKey));
        }
    }
}
