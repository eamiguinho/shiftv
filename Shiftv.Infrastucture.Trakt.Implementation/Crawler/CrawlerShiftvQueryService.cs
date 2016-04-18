using System.Threading.Tasks;
using Shiftv.Contracts.DataServices.Crawler;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Crawler
{
    class CrawlerShiftvQueryService : ICrawlerShiftvQueryService
    {
        public Task<string> GetPossibleNames(string imdbId, string crawlerSource)
        {
            //http://shiftwebservice.azurewebsites.net/api/namemap/tt1528406/animerealm
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}/{4}/",
          TraktConstants.ShiftvSubtitlesApiUrl,
          TraktConstants.ApiResource,
          TraktConstants.NameMapResoure,
          imdbId,
          crawlerSource
          ));
        }
    }
}