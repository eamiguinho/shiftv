using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Global;

namespace Shiftv.Contracts.Services.Episodes
{
    public interface IEpisodeService
    {
        IEpisode GetNextEpisode(IShow show);
       // Task<DataResult<List<IEpisode>>> GetEpisodesBySeason(int season);
        Task<DataResult<List<IUser>>> GetWatchingNow(int season, int episode);
        Task<DataResult<IGenericPostResult>> SetAsSeen(int season, int episode);
        Task<DataResult<List<ISetAsSeenResult>>> SetAsSeen(List<IEpisode> episodes, IMiniShow selectedShow = null);
        Task<DataResult<ISetAsSeenResult>> SetAsSeen(IEpisode episode, IMiniShow miniShow = null);
        Task<DataResult<IGenericPostResult>> SetAsUnseen(int season, int episode);
        Task<DataResult<IRateResult>> RateEpisode(bool love, int season, int episode);
        Task<DataResult<ICheckinResult>> CheckIn(int season, int episode);
        Task<DataResult<IMediaStream>> GetEpisodeLink(int season, int episode, DateTime episodeName, string subtitlesLanguages, int? numberAbs);
        Task<DataResult<IGenericPostResult>> AddToWatchlist(IEpisode episode);

        Task<DataResult<IGenericPostResult>> AddEpisodesToWatchlist(List<IEpisode> episodes);
        Task<DataResult<IGenericPostResult>> RemoveAllFromWatchlist(List<IEpisode> episodes);
        Task<DataResult<List<ISetAsSeenResult>>> SetAsUnseen(List<IEpisode> episodes);
        Task<DataResult<List<IRateResult>>> RateEpisodes(int value, List<IEpisode> episodes);
        Task<DataResult<IGenericPostResult>> RemoveFromWatchlist(IEpisode season);
        Task<DataResult<IEpisode>> GetFullEpisodeInfo(int imdbId, int season, int episode);
        List<ISubtitlesLanguage> GetListSubtitlesLanguage();
        Task<DataResult<int?>> GetAbsoluteNumberFromTvDb(int showTvDbId, DateTime episodeAirDate);
        Task<DataResult<ICheckinResult>> CancelCheckIn();
        Task<DataResult<IMediaStream>> GetEpisodeSubtitles(string subtitlesLanguages, int season, int number);

        Task<DataResult<IGenericPostResult>> SetAsSeen(int season, int episode, int tvdbId, string imdbid, string title,
            int year);

        Task<DataResult<IGenericPostResult>> SetAsSeen(List<IEpisode> episodes, int tvdbId, string imdbid, string title,
            int year);

        Task<DataResult<IRateResult>> RateEpisode(int newValue, IEpisode season);
    }   
}       