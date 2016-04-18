using System.Threading.Tasks;

namespace Shiftv.Contracts.DataServices.Crawler
{
    public interface ICrawlerShiftvQueryService
    {
        Task<string> GetPossibleNames(string imdbId, string crawlerSource);
    }
}