using System.Collections.Generic;
using System.Threading.Tasks;
using Shiftv.Contracts.Domain.Crawler;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Services.Crawler;

namespace Shiftv.Contracts.DataServices.Crawler
{
    public interface ICrawlerShiftvDataService
    {
        Task<List<INameMap>> GetPossibleNames(string imdbId, string crawlerSource);
        void SaveAzureEmbbeds(List<string> toList, string link);
        Task<List<string>> GetAzureEmbbeds(string link);
        void SaveAzureEmbbeds2(List<ILinkInfo> links);
        void SaveAzureEmbbeds2(List<ILinkInfo> downloadlink, string imdbId, int season, int episode);
        Task<List<ILinkInfo>> GetAzureEmbbeds2(string imdbId, int season, int episode, CrawlerType type);
    }
}