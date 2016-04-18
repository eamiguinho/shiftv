using System.Collections.Generic;
using System.Threading.Tasks;
using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Contracts.Services.Crawler
{
    public interface ICrawlerService
    {
        Task<List<ILinkInfo>> GetLinks(string key, int season, int episode, int runtime, int year, CrawlerType type, int episodeName, string imdbId);
        void SaveOnAzure(IMediaStream downloadlink);
        void SaveOnAzure(List<ILinkInfo> downloadlink, string imdbId, int season, int number);
    }

    public enum CrawlerType
    {
        Episode,
        Movie,
        Anime
    }
}