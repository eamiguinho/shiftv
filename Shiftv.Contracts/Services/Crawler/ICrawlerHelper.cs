using System.Threading.Tasks;

namespace Shiftv.Contracts.Services.Crawler
{
    public interface ICrawlerHelper
    {
        Task<string> GetHtml(string url, bool b = false);
        Task<string> GetHtmlUtf(string url, bool b = false);
    }
}