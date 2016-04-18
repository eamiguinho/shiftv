using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shiftv.Contracts.Data.JsonTrakt;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Domain.Users;

namespace Shiftv.Contracts.DataServices.Episodes
{
    public interface IEpisodeTraktDataService
    {
        Task<List<IEpisode>> GetEpisodeBySeason(UserTokenDto userAccount, int tvDbId, int season, string title);
        Task<List<IUser>> GetWatchingNow(int tvDbId, int season, int episode);
        Task<IGenericPostResult> SetAsSeen(UserTokenDto userAccount, int tvDbId, string imdbId, string title, int year, int season, int episode);

        Task<IGenericPostResult> SetAsSeen(UserTokenDto userAccount, int tvDbId, string imdbId, string title, int year,
            List<EpisodeDto> episodes);


        Task<IGenericPostResult> SetAsUnseen(UserTokenDto userAccount, int tvDbId, string imdbId, string title,
            int year, int season, int episode);

        Task<IRateResult> RateEpisode(UserTokenDto userAccount, bool rate, string title, int tvDbId, int year, int season, int episode);

        Task<ICheckinResult> CheckIn(UserTokenDto userAccount, string title, string imdbId, int tvDbId, int year,
            int season, int episode);

        Task<IGenericPostResult> AddEpisodeToWatchlist(UserTokenDto userAccount, int tvDbId, string imdbId,
            string title, int year, List<EpisodeDto> episodes);

        Task<IGenericPostResult> RemoveEpisodeFromWatchlist(UserTokenDto userAccount, int tvDbId, string imdbId,
            string title, int year, List<EpisodeDto> episodes);

        Task<IGenericPostResult> SetAsUnseen(UserTokenDto userAccount, int tvDbId, string imdbId, string title, int year, List<EpisodeDto> toList);

        Task<IRateResult> RateEpisodes(UserTokenDto userAccount, bool rate, int year, int tvdbId, string title,
            List<EpisodeDto> episodes);

        Task<IMediaStream> GetLinks(string imdbId, int season, int episode, string eng);
        Task<IEpisode> GetFullEpisodeInfo(UserTokenDto userAccount, int imdbId, int season, int episode);
        Task<int?> GetAbsoluteNumberFromTvDb(int showTvDbId, DateTime episodeAirDate);
        Task<List<IEpisode>> GetEpisodeBySeasonAzure(int tvDbId, int season, string title);
        Task<ICheckinResult> CancelCheckIn(UserTokenDto getDto);
        Task<IMediaStream> GetSubtitlesFromAzure(string imdbId, int season, int episode, string subtitlesLanguages);
        Task<IGenericPostResult> SetAsSeen(UserTokenDto userAccount, int tvDbId, string imdbId, string title, int year, int season);
        Task<ISetAsSeenResult> SetAsSeen(UserTokenDto userAccount, MiniShowDto showDto, EpisodeDto episodeDto);
        Task<ISetAsSeenResult> SetAsUnseen(UserTokenDto userAccount, ShowDto tvDbId, EpisodeDto getDto);
        Task<IRateResult> RateEpisode(UserTokenDto userAccount, int newValue, EpisodeDto getDto, ShowDto tvDbId);
        Task<List<IRateResult>> RateEpisodes(UserTokenDto userAccount, int value, List<EpisodeDto> toList, ShowDto getDto);
        Task<List<ISetAsSeenResult>> SetAsSeen(UserTokenDto userAccount, List<EpisodeDto> episodeDtos, MiniShowDto showDto);
        Task<List<ISetAsSeenResult>> SetAsUnseen(UserTokenDto userAccount, List<EpisodeDto> episodeDtos, ShowDto showDto);
    }
}