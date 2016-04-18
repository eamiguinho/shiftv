using System.Collections.Generic;
using System.Threading.Tasks;
using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Contracts.Services.Crawler
{
    public interface IAnimeRealmCrawler
    {
       Task<List<ILinkInfo>> GetEpisodes(int episodeTvDbId, string key, string imdbId);
        Task<List<string>> GetLinksToSearch(string showName, int episode);
    }
    public interface IAnimeTwistCrawler
    {
       Task<List<ILinkInfo>> GetEpisodes(int episodeTvDbId, string key, string imdbId);
        Task<List<string>> GetLinksToSearch(string showName, int episode);
    }
}   