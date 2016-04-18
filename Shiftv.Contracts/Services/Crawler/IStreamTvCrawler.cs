using System.Collections.Generic;
using System.Threading.Tasks;
using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Contracts.Services.Crawler
{
    public interface IStreamTvCrawler
    {
        Task<bool> CheckStreamMe(string url);
        Task<List<ILinkInfo>> GetEpisodes(string url, int season, int episode, List<ILinkInfo> listCachedCrawler);
        Task<IEnumerable<string>> GetLinksToSearch(string show);
    }

    public interface ISeriesCravingsCrawler
    {
        Task<List<ILinkInfo>> GetEpisodes(string url, int season, int episode, List<ILinkInfo> listCachedCrawler, string imdbId);
        Task<IEnumerable<string>> GetLinksToSearch(string showName, string imdbId);
    }


    public interface IOnlineHdMoviesCrawler
    {
        Task<List<ILinkInfo>> GetMovie(string url);
        Task<IEnumerable<string>> GetLinksToSearch(string movieName, int year);
    }

    public interface IWatchMoviesCrawler
    {
        Task<List<ILinkInfo>> GetMovie(string url);
        Task<IEnumerable<string>> GetLinksToSearch(string movieName, int year);
    }

    public interface IWatchSeriesCrawler
    {
        Task<List<ILinkInfo>> GetEpisodes(string url, List<ILinkInfo> listCachedCrawler);
        Task<IEnumerable<string>> GetLinksToSearch(string movieName, int season, int episode);
    }

    public interface ICouchtunerSeriesCrawler
    {
        Task<List<ILinkInfo>> GetEpisodes(string url, List<ILinkInfo> listCachedCrawler);
        Task<IEnumerable<string>> GetLinksToSearch(string movieName, int season, int episode);
    }
    public interface IMoviesMazeCrawler
    {
        Task<List<ILinkInfo>> GetMovie(string url);
        Task<IEnumerable<string>> GetLinksToSearch(string movieName, int year);
    }

    public interface IJustClickToWatchCrawler
    {
        Task<List<ILinkInfo>> GetMovie(string url);
        Task<IEnumerable<string>> GetLinksToSearch(string movieName, int year);
    }
    public interface IPullvideosTvCrawler
    {
        Task<List<ILinkInfo>> GetEpisodes(string url, int season, int episode, List<ILinkInfo> listCachedCrawler);
        Task<IEnumerable<string>> GetLinksToSearch(string showName);
    }
}