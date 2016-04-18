using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac;
using Shiftv.Contracts.DataServices.Crawler;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Services;
using Shiftv.Contracts.Services.Crawler;
using Shiftv.Global;

namespace Shiftv.Services.Implementation.Crawler
{
    class AnimeRealmCrawler : IAnimeRealmCrawler
    {
        private List<ILinkInfo> LinkDetail;
        private ICrawlerHelper _helper;
        private ICrawlerShiftvDataService _dataService;

        public AnimeRealmCrawler(ICrawlerHelper helper, ICrawlerShiftvDataService dataService)
        {
            _helper = helper;
            _dataService = dataService;
        }

        public async Task<List<ILinkInfo>> GetEpisodes(int episodeTvDbId, string key, string imdbId)
        {
            LinkDetail = new List<ILinkInfo>();
            await GetStream(episodeTvDbId, key, imdbId);
            return LinkDetail;
        }

        public async Task<List<string>> GetLinksToSearch(string showName, int episode)
        {
            return await Task.Run(() =>
            {
                var linksToSearch = new List<string>();
                try
                {
                    if (showName == "Naruto Shippuuden") showName = "Naruto Shippuden";
                    showName = string.Format(@"http://www.anime-realm.com/anime-{0}.html", showName.Replace(" ", "-").Replace("(", "").Replace(")", "").ToLower());
                    linksToSearch.Add(showName);
                    return linksToSearch;
                }
                catch (Exception)
                {
                    return linksToSearch;
                }
            });
         
        }

        private async Task<string> GetAnimeEpisodeNumber(int episodeTvDbId)
        {
            var s = await _helper.GetHtml("http://thetvdb.com/?tab=episode&id=" + episodeTvDbId);
            var split1 = Regex.Split(s, "absolute_number");
            var split2 = Regex.Split(split1[1], "\"");
            string result = Regex.Replace(split2[2], @"[^\d]", "");
            return result;
        }

        private async Task GetStream(int episodeTvDbId, string key, string imdbId)
        {
            try
            {
                await GetStreamWithKey(episodeTvDbId, key);
                if (LinkDetail.Count == 0)
                {
                    var getPossibleName = await _dataService.GetPossibleNames(imdbId, "animerealm");
                    if (getPossibleName != null)
                    {
                        foreach (var nameMap in getPossibleName)
                        {
                            if (!nameMap.IsCountRestart)
                                await GetStreamWithKey(episodeTvDbId, nameMap.NameMapped);
                            else
                            {
                                var epirestartNum = episodeTvDbId - (nameMap.RestartAt - 1);
                                await GetStreamWithKey(epirestartNum, nameMap.NameMapped);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task GetStreamWithKey(int episodeTvDbId, string key)
        {
            key = key.Replace(" ", "-");
            var number = episodeTvDbId;
            const string linkBase = "http://www.anime-realm.com/{0}-episode-{1}{2}.html";
            string testMatch = string.Format(linkBase, key, number, "");
            await GetStream2(testMatch);
            testMatch = string.Format(linkBase, key, number, "-2-sub");
            await GetStream2(testMatch);
            testMatch = string.Format(linkBase, key, number, "-3-sub");
            await GetStream2(testMatch);
            testMatch = string.Format(linkBase, key, number, "-4-sub");
            await GetStream2(testMatch);
        }

        private async Task GetStream2(string episodeStreamLink)
        {
            try
            {
                var s = await _helper.GetHtml(episodeStreamLink);

                await MakeLink(s, "embed", episodeStreamLink);
            }
            catch (Exception)
            {

                throw;
            }
        }


        private async Task MakeLink(string txt, string delimiter, string episodeStreamLink)
        {
            if (string.IsNullOrEmpty(txt)) return;
            var regx =
                new Regex(
                    "http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?",
                    RegexOptions.IgnoreCase);
            var mactches = regx.Matches(txt);
            foreach (var match in mactches.Cast<Match>().Where(match => match.ToString().Contains(delimiter)))
            {
                if (delimiter == "embed") await LoadMovieStream(match.ToString(), episodeStreamLink);
                else
                {
                    if (!string.IsNullOrEmpty(match.ToString()))
                    {
                        var str = match.ToString();
                        str = str.Replace("%2F", "/").Replace(",", "").Replace("'", "");
                        Uri test = new Uri(str);
                        var linkDetail = Ioc.Container.Resolve<ILinkInfo>();
                        if (test.Host.Contains(".mp4"))
                        {
                            if (test.AbsolutePath.Contains(".mp4"))
                            {
                                linkDetail.StreamLink = str;
                                linkDetail.OriginalLink = episodeStreamLink;
                                LinkDetail.Add(linkDetail);
                            }
                        }
                        else
                        {
                            linkDetail.StreamLink = str;
                            linkDetail.OriginalLink = episodeStreamLink;
                            LinkDetail.Add(linkDetail);
                        }

                    }
                }
            }

        }


        private async Task LoadMovieStream(string url, string episodeStreamLink)
        {
            try
            {
                var s = await _helper.GetHtml(url);
                await MakeLink(s, ".mp4", episodeStreamLink);
            }
            catch (Exception )
            {
                //Console.Write(e);

            }
        }

    }
}
