using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Shiftv.Contracts.DataServices.Crawler;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Services.Crawler;

namespace Shiftv.Services.Implementation.Crawler
{
    class CrawlerService : ICrawlerService
    {
        private readonly IAnimeRealmCrawler _animeRealm;
        private static List<ILinkInfo> _linkDetail;
        private static List<string> _linksToSearch;

        private IAnimeTwistCrawler _animeTwistCrawler;
        private ICrawlerShiftvDataService _dataService;
        private IWatchMoviesCrawler _watchMoviesCrawler;
        private IMoviesMazeCrawler _moviesMazeCrawler;
        private IWatchSeriesCrawler _watchSeriesCrawler;
        private IOnlineHdMoviesCrawler _onlineHdMoviesCrawler;
        private readonly ICouchtunerSeriesCrawler _couchtunerSeriesCrawler;

        public CrawlerService(IAnimeRealmCrawler animeRealm, IAnimeTwistCrawler animeTwistCrawler, ICrawlerShiftvDataService dataService, IWatchMoviesCrawler watchMoviesCrawler, IMoviesMazeCrawler moviesMazeCrawler, IWatchSeriesCrawler watchSeriesCrawler, IOnlineHdMoviesCrawler onlineHdMoviesCrawler, ICouchtunerSeriesCrawler couchtunerSeriesCrawler)
        {
            _animeRealm = animeRealm;
            _animeTwistCrawler = animeTwistCrawler;
            _dataService = dataService;
            _watchMoviesCrawler = watchMoviesCrawler;
            _moviesMazeCrawler = moviesMazeCrawler;
            _watchSeriesCrawler = watchSeriesCrawler;
            _onlineHdMoviesCrawler = onlineHdMoviesCrawler;
            _couchtunerSeriesCrawler = couchtunerSeriesCrawler;
        }
        private List<string> LinksToSearch { get { return _linksToSearch ?? (_linksToSearch = new List<string>()); } }
        public async Task<List<ILinkInfo>> GetLinks(string key, int season, int episode, int runtime, int year, CrawlerType type, int episodeTvDbId, string imdbId)
        {
            key = Regex.Replace(key, @" ?\(.[0-9]*?\)", string.Empty);
            key = key.Replace("(", "").Replace(")", "").Replace("'", "");
            if (imdbId == "tt1586680") key = key + " us";
            await FillLinksToSearch(key, episode, year, type, imdbId,season);
            _linkDetail = new List<ILinkInfo>();
            const int iter = 0; 
            while (LinksToSearch.Count != 0)
            {
                var res = await ProcessStreams(season, episode, 0, type, episodeTvDbId, key, imdbId);
                if (res.Any())
                {
                    _linkDetail.AddRange(res);
                }

                LinksToSearch.RemoveAt(iter);
            }
            if (!_linkDetail.Any()) return new List<ILinkInfo>();
            var last = RemoveRedundant(_linkDetail);
            // await Cheaw23ckFileSizesAndQuality(last, runtime);
            return last;
        }

        public void SaveOnAzure(IMediaStream downloadlink)
        {
            _dataService.SaveAzureEmbbeds2(downloadlink.Links);
        }

        public void SaveOnAzure(List<ILinkInfo> downloadlink, string imdbId, int season, int episode)
        {
            _dataService.SaveAzureEmbbeds2(downloadlink, imdbId, season, episode);
        }

        private async Task<List<ILinkInfo>> ProcessStreams(int season, int episode, int iter, CrawlerType type, int episodeTvDbId, string key, string imdbId)
        {
            List<ILinkInfo> listCachedCrawler = new List<ILinkInfo>();
            //switch (type)
            //{
            //    case CrawlerType.Episode:
            //        listCachedCrawler = await _dataService.GetAzureEmbbeds2(imdbId, season, episode, type);
            //        if (listCachedCrawler != null && listCachedCrawler.Count < 4) listCachedCrawler = null;
            //        break;
            //    case CrawlerType.Anime:
            //        listCachedCrawler = await _dataService.GetAzureEmbbeds2(imdbId, season, episode, type);
            //        if (listCachedCrawler != null && listCachedCrawler.Count < 2) listCachedCrawler = null;
            //        break;
            //    case CrawlerType.Movie:
            //        listCachedCrawler = await _dataService.GetAzureEmbbeds2(imdbId, season, episode, type);
            //        if (listCachedCrawler != null && listCachedCrawler.Count < 2) listCachedCrawler = null;
            //        break;
            //    default:
            //        listCachedCrawler = null;
            //        break;
            //}

      
            if (LinksToSearch[iter].StartsWith("http://www.anime-realm.com"))
            {
                switch (type)
                {
                    case CrawlerType.Episode:
                        break;
                    case CrawlerType.Movie:
                        break;
                    case CrawlerType.Anime:
                        return await _animeRealm.GetEpisodes(episodeTvDbId, key, imdbId);
                    default:
                        throw new ArgumentOutOfRangeException("type");
                }
            }
            if (LinksToSearch[iter].StartsWith("https://animetwist.net/"))
            {
                switch (type)
                {
                    case CrawlerType.Episode:
                        break;
                    case CrawlerType.Movie:
                        break;
                    case CrawlerType.Anime:
                        return await _animeTwistCrawler.GetEpisodes(episodeTvDbId, key, imdbId);
                    default:
                        throw new ArgumentOutOfRangeException("type");
                }
            }

            if (LinksToSearch[iter].StartsWith("http://onlinehdmovies.org/"))
            {
                switch (type)
                {
                    case CrawlerType.Episode:
                        break;
                    case CrawlerType.Movie:
                        return await _onlineHdMoviesCrawler.GetMovie(LinksToSearch[iter]);
                    case CrawlerType.Anime:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("type");
                }
            }  
            
           if (LinksToSearch[iter].StartsWith("http://watchmovies-online.ch/"))
            {
                switch (type)
                {
                    case CrawlerType.Episode:
                        break;
                    case CrawlerType.Movie:
                        return await _watchMoviesCrawler.GetMovie(LinksToSearch[iter]);
                    case CrawlerType.Anime:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("type");
                }
            }
       
            if (LinksToSearch[iter].StartsWith("http://moviesmaze.com/"))
            {
                switch (type)
                {
                    case CrawlerType.Episode:
                        break;
                    case CrawlerType.Movie:
                        return await _moviesMazeCrawler.GetMovie(LinksToSearch[iter]);
                    case CrawlerType.Anime:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("type");
                }
            }
            if (LinksToSearch[iter].StartsWith("http://watchseries-online.ch/"))
            {
                switch (type)
                {
                    case CrawlerType.Episode:
                        return await _watchSeriesCrawler.GetEpisodes(LinksToSearch[iter], listCachedCrawler);
                    case CrawlerType.Movie:
                        break;
                    case CrawlerType.Anime:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("type");
                }
            }
            if (LinksToSearch[iter].StartsWith("http://couchtuner.city/"))
            {
                switch (type)
                {
                    case CrawlerType.Episode:
                        return await _couchtunerSeriesCrawler.GetEpisodes(LinksToSearch[iter], listCachedCrawler);
                    case CrawlerType.Movie:
                        break;
                    case CrawlerType.Anime:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("type");
                }
            }
            return new List<ILinkInfo>();
        }





        private async Task FillLinksToSearch(string key, int episode, int year, CrawlerType type, string imdbId, int season)
        {
            LinksToSearch.Clear();
            switch (type)
            {
                case CrawlerType.Episode:
                    await FillLinksToSearchShows(key, imdbId,season,episode);
                    break;
                case CrawlerType.Movie:
                    await FillLinksToSearchMovies(key, year);
                    break;
                case CrawlerType.Anime:
                    await FillLinksToSearchAnime(key, episode);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        private async Task FillLinksToSearchAnime(string showName, int episode)
        {
            var linksFromAnimeRealm = await _animeRealm.GetLinksToSearch(showName, episode);
            LinksToSearch.AddRange(linksFromAnimeRealm);

            var linksFromAnimeTwist = await _animeTwistCrawler.GetLinksToSearch(showName, episode);
            LinksToSearch.AddRange(linksFromAnimeTwist);
        }

        private async Task FillLinksToSearchShows(string show, string imdbId, int season, int episode)
        {
           
            
            var watchSeriesCrawler = await _watchSeriesCrawler.GetLinksToSearch(show, season, episode);
            LinksToSearch.AddRange(watchSeriesCrawler);
            var couchtunerSeriesCrawler = await _couchtunerSeriesCrawler.GetLinksToSearch(show, season, episode);
            LinksToSearch.AddRange(couchtunerSeriesCrawler);
        }
        private async Task FillLinksToSearchMovies(string key, int year)
        {
            var wathmoviescrawler = await _watchMoviesCrawler.GetLinksToSearch(key, year);
            if (wathmoviescrawler != null) LinksToSearch.AddRange(wathmoviescrawler);
            var onlineHdMoviesCrawler = await _onlineHdMoviesCrawler.GetLinksToSearch(key, year);
            if (onlineHdMoviesCrawler != null) LinksToSearch.AddRange(onlineHdMoviesCrawler);
            var moviesMazeCrawler = await _moviesMazeCrawler.GetLinksToSearch(key, year);
            if (moviesMazeCrawler != null) LinksToSearch.AddRange(moviesMazeCrawler);
        }



        private List<ILinkInfo> RemoveRedundant(IEnumerable<ILinkInfo> linkDetail)
        {
            var a = linkDetail.DistinctBy(x => x.StreamLink).ToList();
            a.RemoveAll(x => x.StreamLink.Length > 200);
            return a;
        }



    }

    public static class Test
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
    this IEnumerable<TSource> source,
    Func<TSource, TKey> keySelector)
        {
            var knownKeys = new HashSet<TKey>();
            return source.Where(element => knownKeys.Add(keySelector(element)));
        }
    }
}
