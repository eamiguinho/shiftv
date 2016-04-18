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
    class MoviesMazeCrawler : IMoviesMazeCrawler
    {
        public List<ILinkInfo> LinkDetail;
        private ICrawlerHelper _helper;
        private List<ILinkInfo> _linkDetail;

        public MoviesMazeCrawler(ICrawlerHelper helper)
        {
            _helper = helper;
        }
        public async Task<bool> CheckStreamMe(string url)
        {
            return await Task.Run(() => !string.IsNullOrEmpty(url) && url.Contains("class=\"post error404 no-results not-found\""));
        }

        public async Task<List<ILinkInfo>> GetMovie(string url)
        {
            try
            {
                LinkDetail = new List<ILinkInfo>();
                await GetStream(url);
                return LinkDetail;
            }
            catch (Exception)
            {
                return LinkDetail;
            }
        }

        public async Task<IEnumerable<string>> GetLinksToSearch(string movieName, int year)
        {
            return await Task.Run(async () =>
            {
                var linksToSearch = new List<string>();
                try
                {
                    var url = "http://moviesmaze.com/" + string.Format("{0}-full-movie-{1}-online", movieName.Replace(" ", "-"), year);
                    var html = await _helper.GetHtml(url);
                    if (string.IsNullOrEmpty(html))
                    {
                        url = "http://moviesmaze.com/" + string.Format("{0}-{1}-full-movie-online", movieName.Replace(" ", "-"), year);
                        html = await _helper.GetHtml(url);
                        if (string.IsNullOrEmpty(html))
                        {
                            url = "http://moviesmaze.com/" + string.Format("{0}-{1}-full-movie", movieName.Replace(" ", "-"), year);
                            html = await _helper.GetHtml(url);
                            if (string.IsNullOrEmpty(html))
                            {
                                return null;
                            }
                        }
                    }
                    linksToSearch.Add(url);
                    return linksToSearch;
                }
                catch (Exception)
                {
                    return linksToSearch;
                }
            });
        }

        private async Task GetStream(string episodeStreamLink)
        {
            try
            {
                var html = await _helper.GetHtml(episodeStreamLink, true);
                var watch2 = GetWatchLinks(html.Replace("\r\n", ""));
                foreach (var tuple in watch2)
                {
                    if (tuple.Item2.Contains("ONE CLICK DIRECT"))
                    {
                        var linkDetail = Ioc.Container.Resolve<ILinkInfo>();
                        linkDetail.EmbbedLink = tuple.Item1;
                        await LoadMovieStream(linkDetail, tuple.Item1);
                    }
                }
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

        private async Task MakeLink(string txt, string delimiter, string episodeStreamLink, ILinkInfo embbed = null)
        {
            if (string.IsNullOrEmpty(txt)) return;
            var regx = new Regex("http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?", RegexOptions.IgnoreCase);
            var mactches = regx.Matches(txt);
            foreach (var match in mactches.Cast<Match>().Where(match => match.ToString().Contains(delimiter)))
            {
                if (string.IsNullOrEmpty(match.ToString()) || embbed == null) continue;
                var str = match.ToString();
                str = str.Replace("%2F", "/").Replace(",", "").Replace("'", "");
                embbed.StreamLink = str;
                if (string.IsNullOrEmpty(embbed.OriginalLink)) embbed.OriginalLink = episodeStreamLink;
                LinkDetail.Add(embbed);
            }

        }


        private async Task LoadMovieStream(ILinkInfo url, string episodeStreamLink)
        {
            try
            {
                var s = await _helper.GetHtml(url.EmbbedLink, true);
                await MakeLink(s, ".mp4", episodeStreamLink, url);
            }
            catch (Exception)
            {
                //Console.Write(e);
            }
        }
    }
}
