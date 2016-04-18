using System;
using System.Threading.Tasks;

namespace Shiftv.Contracts.DataServices.Episodes
{
    public interface IEpisodeTraktQueryService
    {
        Task<string> GetEpisodeBySeason(int tvDbId, int season);
        Task<string> GetWatchingNow(int tvDbId, int season, int episode);
        Task<string> GetSetAsSeen();
        Task<string> GetSetAsUnseen();
        Task<string> GetRateEpisode();
        Task<string> GetCheckIn();
        Task<string> AddEpisodeToWatchList();
        Task<string> RemoveEpisodeFromWatchlist();
        Task<string> GetRateEpisodes();
        Task<string> GetLinks(string imdbId, int season, int episode, string lang);
        Task<string> GetFullEpisodeInfo(int imdbId, int season, int episode);
        Task<string> GetAbsoluteNumberFromTvDb(int showTvDbId, DateTime episodeAirDate);
        Task<string> GetCancelCheckIn();
    }
}