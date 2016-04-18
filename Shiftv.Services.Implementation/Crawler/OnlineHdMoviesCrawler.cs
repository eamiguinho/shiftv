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
    class OnlineHdMoviesCrawler : IOnlineHdMoviesCrawler
    {
        public List<ILinkInfo> LinkDetail;
        private ICrawlerHelper _helper;
        private List<ILinkInfo> _linkDetail;

        public OnlineHdMoviesCrawler(ICrawlerHelper helper)
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
                    var url = "http://onlinehdmovies.org/" + string.Format("{0}-{1}-watch-online", movieName.Replace(" ", "-").Replace(":", ""), year);
                    linksToSearch.Add(url);
                    url = "http://onlinehdmovies.org/" + string.Format("{0}-{1}-watch-online", movieName.Replace(" ", "-").Replace(":", ""), year - 1);
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
                if (string.IsNullOrEmpty(html)) return;
                var tfo = html.Replace("\r\n", "");
                tfo = Regex.Replace(tfo, @"<span\s+style=""margin-left: 1px"">.*?</span>", "");
                var watch2 = GetWatchLinks(tfo);
                foreach (var tuple in watch2)
                {
                    if (tuple.Item2.Contains("CLICK HERE"))
                    {
                        if (tuple.Item1.Contains("videowood"))
                        {
                            var replink = tuple.Item1.Replace("/video/", "/embed/");
                            var htmlres = await _helper.GetHtml(replink);
                            var linkDetail = Ioc.Container.Resolve<ILinkInfo>();
                            linkDetail.EmbbedLink = tuple.Item1;
                            await MakeLink(htmlres, ".mp4", episodeStreamLink, linkDetail);
                        }
                        else
                        {
                            var htmlres = await _helper.GetHtmlUtf(tuple.Item1);
                            if (!string.IsNullOrEmpty(htmlres))
                            {
                                if (tuple.Item1.Contains("filehoot")) await MakeLink(htmlres, "filehoot", episodeStreamLink);
                                //if (tuple.Item1.Contains("vidzi")) await MakeLink(htmlres, "vidzi", episodeStreamLink);
                                if (tuple.Item1.Contains("vodlocker")) await MakeLink(htmlres, "vodlocker", episodeStreamLink);
                                if (tuple.Item1.Contains("bestreams")) await MakeLink(htmlres, "bestreams", episodeStreamLink);
                                if (tuple.Item1.Contains("openload.io"))
                                {
                                    var linkDetail = Ioc.Container.Resolve<ILinkInfo>();
                                    linkDetail.EmbbedLink = tuple.Item1;
                                    await MakeLink(htmlres, ".mp4", episodeStreamLink, linkDetail, true);
                                }
                             
                            }
                        }
                     
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

        private async Task MakeLink(string txt, string delimiter, string episodeStreamLink, ILinkInfo embbed = null, bool isSecure = false)
        {
            try
            {
                if (string.IsNullOrEmpty(txt)) return;
                var regx = !isSecure
                    ? new Regex(
                        "http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?",
                        RegexOptions.IgnoreCase)
                    : new Regex(
                        "https://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?",
                        RegexOptions.IgnoreCase);
                var mactches = regx.Matches(txt);
                foreach (var match in mactches.Cast<Match>().Where(match => match.ToString().Contains(delimiter)))
                {
                    if (delimiter != ".mp4" && delimiter != ".mkv")
                    {
                        var linkDetail = Ioc.Container.Resolve<ILinkInfo>();
                        linkDetail.EmbbedLink = match.ToString();
                        //await LoadMovieStream(linkDetail, episodeStreamLink);
                        if (delimiter == "vodlocker")
                        {
                            var uri = new Uri(match.Value.Replace("'", ""));
                            var formatEmbed = string.Format("http://vodlocker.com/embed-{0}.html", uri.Segments[1]);
                            linkDetail.EmbbedLink = formatEmbed;
                            await LoadMovieStream(linkDetail, episodeStreamLink);
                        }
                        else if (delimiter == "bestreams")
                        {
                            var uri = new Uri(match.Value.Replace("'", ""));
                            var formatEmbed = string.Format("http://bestreams.net/embed-{0}.html", uri.Segments[1]);
                            linkDetail.EmbbedLink = formatEmbed;
                            await LoadMovieStream(linkDetail, episodeStreamLink);
                        }
                        else if (delimiter == "filehoot")
                        {
                            var uri = new Uri(match.Value.Replace("'", ""));
                            var formatEmbed = string.Format("http://filehoot.com/embed-{0}.html", uri.Segments[1].Replace(".html", ""));
                            linkDetail.EmbbedLink = formatEmbed;
                            await LoadMovieStream(linkDetail, episodeStreamLink);
                        }
                        else if (delimiter == "openload.io")
                        {
                            var uri = new Uri(match.Value.Replace("'", ""));
                            var formatEmbed = string.Format("http://openload.io/embed/{0}.html", uri.Segments[1].Replace(".html", ""));
                            linkDetail.EmbbedLink = formatEmbed;
                            await LoadMovieStream(linkDetail, episodeStreamLink);
                        }
                        else
                        {
                            linkDetail.EmbbedLink = match.Value;
                            await LoadMovieStream(linkDetail, episodeStreamLink);
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(match.ToString()) || embbed == null) continue;
                        var str = match.ToString();
                        str = str.Replace("%2F", "/").Replace(",", "").Replace("'", "");
                        embbed.StreamLink = str;
                        if (string.IsNullOrEmpty(embbed.OriginalLink)) embbed.OriginalLink = episodeStreamLink;
                        LinkDetail.Add(embbed);
                    }
                }
            }
            catch (Exception e)
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
    }
}
