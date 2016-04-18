using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac;
using Shiftv.Contracts.DataServices.Crawler;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Services.Crawler;
using Shiftv.Global;

namespace Shiftv.Services.Implementation.Crawler
{
    class AnimeTwistCrawler : IAnimeTwistCrawler
    {
        private List<ILinkInfo> LinkDetail;
        private ICrawlerHelper _helper;
        private ICrawlerShiftvDataService _dataService;

        public AnimeTwistCrawler(ICrawlerHelper helper, ICrawlerShiftvDataService dataService)
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
                    showName = string.Format(@"https://animetwist.net/a/{0}/{1}", showName.Replace(" ", "").Replace("(", "").Replace(")", "").ToLower(), episode);
                    linksToSearch.Add(showName);
                    return linksToSearch;
                }
                catch (Exception)
                {
                    return linksToSearch;
                }
            });

        }

    
        private async Task GetStream(int episodeTvDbId, string key, string imdbId)
        {
            await GetStreamWithKey(episodeTvDbId, key);
            if (LinkDetail.Count == 0)
            {
                var getPossibleName = await _dataService.GetPossibleNames(imdbId, "animetwist");
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

        private async Task GetStreamWithKey(int episodeTvDbId, string key)
        {
            key = key.Replace(" ", "");
            var number = episodeTvDbId;
            const string linkBase = "https://animetwist.net/a/{0}/{1}";
            string testMatch = string.Format(linkBase, key, number);
            await LoadMovieStream(testMatch, testMatch);
        }
      


        private async Task MakeLink(string txt, string delimiter, string episodeStreamLink)
        {
            if (string.IsNullOrEmpty(txt)) return;
            var regx =
                new Regex(
                    @"(?<=src="")[^""]+",
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
                        var linkDetail = Ioc.Container.Resolve<ILinkInfo>();
                        {
                            linkDetail.StreamLink = string.Format("https://animetwist.net/{0}",str);
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
            catch (Exception)
            {
                //Console.Write(e);

            }
        }

    }
}
