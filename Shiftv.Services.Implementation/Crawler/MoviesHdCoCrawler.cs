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
    class MoviesHdCoCrawler : IMoviesHdCoCrawler
    {
        public List<ILinkInfo> LinkDetail;
        private ICrawlerHelper _helper;

        public MoviesHdCoCrawler(ICrawlerHelper helper)
        {
            _helper = helper;
        }

        public async Task<List<string>> GetLinksToSearchMovies(string key, int year)
        {
            return await Task.Run(() =>
            {
                var linksToSearch = new List<string>();
                try
                {
                    var link = string.Format("http://movieshd.co/watch-online/{0}-{1}.html", key.Replace(" ", "-").ToLower(), year);
                    linksToSearch.Add(link);
                    return linksToSearch;
                }
                catch (Exception)
                {
                    return linksToSearch;
                }
            });
        
        }

        private async Task<bool> RemoteFileExists(string url)
        {
            try
            {
                var s = await _helper.GetHtml(url);
                if (url.StartsWith("http://stream-tv.me/"))
                {
                    // return await CheckStreamMe(s);
                }
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
            return false;
        }
        private async Task GetStream(string episodeStreamLink)
        {

            try
            {
                var s = await _helper.GetHtml(episodeStreamLink, true);
                var t = MakeLink2(s, "http://videomega.tv/validatehash.php");
                var tref = await _helper.GetHtml(t, true);
                var tresplit = Regex.Split(tref, "ref=\"");
                var t1 = Regex.Split(tresplit[tresplit.Length - 1], "\"");
                var newsLink = "http://videomega.tv/iframe.php?ref=" + t1[0];
                var linkDetail = Ioc.Container.Resolve<ILinkInfo>();
                linkDetail.EmbbedLink = newsLink;

                await LoadMovieStream(linkDetail, episodeStreamLink);
            }
            catch (Exception)
            {

                throw;
            }
        }


        private string MakeLink2(string txt, string delimiter)
        {
            var regx = new Regex("http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?", RegexOptions.IgnoreCase);
            var mactches = regx.Matches(txt);
            return mactches.Cast<Match>().Where(match => match.ToString().Contains(delimiter)).Select(match => match.ToString()).FirstOrDefault();
        }

        private async Task MakeLink(string txt, string delimiter, string episodeStreamLink, ILinkInfo embbed = null)
        {
            if (string.IsNullOrEmpty(txt)) return;
            var regx = new Regex("http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?", RegexOptions.IgnoreCase);
            var mactches = regx.Matches(txt);
            foreach (var match in mactches.Cast<Match>().Where(match => match.ToString().Contains(delimiter)))
            {
                if (delimiter == "iframe.php")
                {
                    var linkDetail = Ioc.Container.Resolve<ILinkInfo>();
                    linkDetail.EmbbedLink = match.ToString();
                    await LoadMovieStream(linkDetail, episodeStreamLink);
                }
                else
                {
                    if (!string.IsNullOrEmpty(match.ToString()) && embbed != null)
                    {
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

        }

        private async Task LoadMovieStream(ILinkInfo url, string episodeStreamLink)
        {
            try
            {
                var s = await _helper.GetHtml(url.EmbbedLink);
                var t = Regex.Split(s, "unescape()");
                t.ToList().RemoveAll(x => x == "");
                foreach (var s1 in t)
                {
                    var mystring = s1.Trim().Replace("\"", "").Replace("(", "").Replace(")", "");
                    if (mystring.StartsWith("%"))
                    {
                        var ll = Regex.Split(mystring, ";");
                        var unescape = Uri.UnescapeDataString(ll[0]);
                        await MakeLink(unescape, ".mp4", episodeStreamLink, url);
                    }
                }
            }
            catch (Exception )
            {
                //Console.Write(e);
            }
        }

        public async Task<List<ILinkInfo>> GetMovie(string url, List<ILinkInfo> listCachedCrawler)
        {
            try
            {
                LinkDetail = new List<ILinkInfo>();
                if (listCachedCrawler != null && listCachedCrawler.Count > 0)
                {
                    foreach (var embed in listCachedCrawler.Where(x => x.OriginalLink == url))
                    {
                        await LoadMovieStream(embed, url);
                    }
                }
                else await GetStream(url);
                return LinkDetail;
            }
            catch (Exception)
            {
                return LinkDetail;
            }
        }
    }
	
	
}
