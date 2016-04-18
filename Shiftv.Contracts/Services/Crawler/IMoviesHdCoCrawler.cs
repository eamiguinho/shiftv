using System.Collections.Generic;
using System.Threading.Tasks;
using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Contracts.Services.Crawler
{
    public interface IMoviesHdCoCrawler
    {
        Task<List<ILinkInfo>> GetMovie(string url, List<ILinkInfo> listCachedCrawler);
        Task<List<string>> GetLinksToSearchMovies(string key, int year);

    }
}