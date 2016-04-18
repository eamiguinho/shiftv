using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Services.Crawler;
using Shiftv.Global;

namespace Shiftv.Services.Implementation.Crawler
{
    class CouchtunerSeriesCrawler : ICouchtunerSeriesCrawler
    {
        public List<ILinkInfo> LinkDetail;
        private ICrawlerHelper _helper;
        private List<ILinkInfo> _linkDetail;

        public CouchtunerSeriesCrawler(ICrawlerHelper helper)
        {
            _helper = helper;
        }
        public async Task<bool> CheckStreamMe(string url)
        {
            return await Task.Run(() => !string.IsNullOrEmpty(url) && url.Contains("Sorry"));
        }
   
        

        private async Task GetStream(string episodeStreamLink)
        {
            try
            {
                var html = await _helper.GetHtml(episodeStreamLink);
                if (string.IsNullOrEmpty(html))
                {
                    if (episodeStreamLink.ToLower().Contains("the-"))
                    {
                        html = await _helper.GetHtml(episodeStreamLink.ToLower().Replace("the-", ""));
                        if (string.IsNullOrEmpty(html)) return;
                    }
                   
                }
                var tfo = html.Replace("\r\n", "");
                tfo = Regex.Replace(tfo, @"<span\s+style=""margin-left: 1px"">.*?</span>", "");
                await MakeLink(tfo, "allmyvideos", episodeStreamLink);
                await MakeLink(tfo, "vidspot", episodeStreamLink);
                await MakeLink(tfo, "thevideo", episodeStreamLink);
                //var watch2 = GetWatchLinks(tfo);
                //foreach (var tuple in watch2)
                //{
                //    if (tuple.Item2.Contains("allmyvideos"))
                //    {
                //        var htmlres = await _helper.GetHtmlUtf(tuple.Item1);
                //        if (!string.IsNullOrEmpty(htmlres))
                //        {
                //            if (tuple.Item2.Contains("allmyvideos")) 
                //        }
                //    }
                //}
            }
            catch (Exception)
            {

            }
        }

        private IEnumerable<Tuple<string, string>> GetWatchLinks(string episodeStreamLink)
        {
            Regex r = new Regex(@"<a.*?href=(""|')(?<href>.*?)(""|').*?>(?<value>.*?)</a>");

            foreach (Match match in r.Matches(episodeStreamLink))
                yield return new Tuple<string, string>(
                    match.Groups["href"].Value, match.Groups["value"].Value);
        }

        private async Task MakeLink(string txt, string delimiter, string episodeStreamLink, ILinkInfo embbed = null, bool isSecure = false)
        {
            try
            {
                if (string.IsNullOrEmpty(txt)) return;
                var regx = !isSecure ? new Regex("http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?", RegexOptions.IgnoreCase) : new Regex("https://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?", RegexOptions.IgnoreCase);
                var mactches = regx.Matches(txt);
                foreach (var match in mactches.Cast<Match>().Where(match => match.ToString().Contains(delimiter)).Where(x=> !x.ToString().Contains(".css")).Where(x=>!x.ToString().Contains(".js")))
                {
                    if (delimiter != ".mp4" && delimiter != ".mkv")
                    {
                        var linkDetail = Ioc.Container.Resolve<ILinkInfo>();
                        linkDetail.EmbbedLink = match.ToString();
                       if (delimiter == "allmyvideos" || delimiter == "vidspot"|| delimiter == "thevideo")
                        {
                            //var uri = new Uri(match.Value.Replace("'", ""));
                            linkDetail.EmbbedLink = match.Value;
                            await LoadMovieStream(linkDetail, episodeStreamLink);
                        }
                }
                    else
                    {

                        if (string.IsNullOrEmpty(match.ToString()) || embbed == null) continue;
                        var linkDetail = Ioc.Container.Resolve<ILinkInfo>();
                        linkDetail.EmbbedLink = embbed.EmbbedLink;
                        var str = match.ToString();
                        str = str.Replace("%2F", "/").Replace(",", "").Replace("'", "");
                        linkDetail.StreamLink = str;
                        if (string.IsNullOrEmpty(linkDetail.OriginalLink)) linkDetail.OriginalLink = episodeStreamLink;
                        LinkDetail.Add(linkDetail);
                    }
                }

            }
            catch (Exception)
            {
            }
      
        }


        private async Task LoadMovieStream(ILinkInfo url, string episodeStreamLink)
        {
            try
            {
                var s = await _helper.GetHtml(url.EmbbedLink);
                await MakeLink(s, ".mp4", episodeStreamLink, url);
            }
            catch (Exception)
            {
                //Console.Write(e);
            }
        }

        public async Task<List<ILinkInfo>> GetEpisodes(string url, List<ILinkInfo> listCachedCrawler)
        {
            try
            {
                LinkDetail = new List<ILinkInfo>();
                if (url != null)
                {
                    if (listCachedCrawler != null && listCachedCrawler.Count > 0 && listCachedCrawler.Count(x => x.OriginalLink == url) > 4) 
                    {
                        foreach (var embed in listCachedCrawler.DistinctBy(a => a.EmbbedLink).Where(x => x.OriginalLink == url))
                        {
                            await LoadMovieStream(embed, url);
                        }
                    }
                    else
                    {
                        await GetStream(url);
                        
                    }
                }
                return LinkDetail;
            }
            catch (Exception)
            {
                return LinkDetail;
            }
        }

        public async  Task<IEnumerable<string>> GetLinksToSearch(string showname, int season, int episode)
        {
            return await Task.Run(async () =>
            {
                var linksToSearch = new List<string>();
                try
                {
                    var url = "http://couchtuner.city/5/" + string.Format("{0}-s{1}-e{2}", showname.Replace(" ", "-"), season.ToString("0"), episode.ToString("0"));
                    linksToSearch.Add(url);
                    var url2 = "http://couchtuner.city/5/" + string.Format("{0}-season-{1}-episode-{2}", showname.Replace(" ", "-"), season.ToString("0"), episode.ToString("0"));
                    linksToSearch.Add(url2);
                    return linksToSearch;
                }
                catch (Exception)
                {
                    return linksToSearch;
                }
            });
        }
    }
}
