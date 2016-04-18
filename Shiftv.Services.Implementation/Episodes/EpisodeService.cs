using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.DataServices.Episodes;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Contracts.Services.Accounts;
using Shiftv.Contracts.Services.Crawler;
using Shiftv.Contracts.Services.Episodes;
using Shiftv.Contracts.Services.Shows;
using Shiftv.Global;

namespace Shiftv.Services.Implementation.Episodes
{
    public class EpisodeService : ServiceHelper, IEpisodeService
    {
        private readonly IEpisodeTraktDataService _dataService;
        private readonly IUserService _userService;
        private readonly IShowService _showService;
        private ICrawlerService _crawler;

        public EpisodeService(IEpisodeTraktDataService dataService, IUserService userService, IShowService showService, ICrawlerService crawler)
        {
            _dataService = dataService;
            _userService = userService;
            _showService = showService;
            _crawler = crawler;
        }
        public IEpisode GetNextEpisode(IShow show)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResult<List<IUser>>> GetWatchingNow(int season, int episode)
        {
            if (season <= -1 || episode <= -1) return new DataResult<List<IUser>>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<List<IUserProfile>>(StandardResults.Offline);
            var show = _showService.GetCurrentShow();
            if (show == null) return new DataResult<List<IUser>>(StandardResults.Error);
            var req = await _dataService.GetWatchingNow(show.Ids.TvDbId.Value, season, episode);
            return req == null ? new DataResult<List<IUser>>(StandardResults.Error) : new DataResult<List<IUser>>(req);
        }

        public async Task<DataResult<IGenericPostResult>> SetAsSeen(int season, int episode)
        {
            if (season <= -1) return new DataResult<IGenericPostResult>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<IGenericPostResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            var show = _showService.GetCurrentShow();
            if (show == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            var req = await _dataService.SetAsSeen(UserTokenDtoFactory.GetDto(user), show.Ids.TvDbId.Value, show.Ids.ImdbId, show.Title, show.Year.Value, season, episode);
            if (req == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            else
            {
                _showService.UpdateCurrentShow();
                return new DataResult<IGenericPostResult>(req);
            }
        }

        public async Task<DataResult<IGenericPostResult>> SetAsSeen(int season, int episode, int tvdbId, string imdbid, string title, int year)
        {
            if (season <= -1 || year <= -1 || tvdbId <= -1 || string.IsNullOrEmpty(imdbid) || string.IsNullOrEmpty(title)) return new DataResult<IGenericPostResult>(StandardResults.Error);
            var user = _userService.GetCurrentUser();
            var req = await _dataService.SetAsSeen(UserTokenDtoFactory.GetDto(user), tvdbId, imdbid, title, year, season, episode);
            if (req == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            else
            {
                _showService.UpdateCurrentShow();
                _showService.UpdateProgress();
                return new DataResult<IGenericPostResult>(req);
            }
        }

        public async Task<DataResult<List<ISetAsSeenResult>>> SetAsSeen(List<IEpisode> episodes, IMiniShow selectedShow = null)
        {
            if (episodes == null || episodes.Count <= 0) return new DataResult<List<ISetAsSeenResult>>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<IGenericPostResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<List<ISetAsSeenResult>>(StandardResults.Error);
            var req = await _dataService.SetAsSeen(UserTokenDtoFactory.GetDto(user), episodes.Select(EpisodeDtoFactory.GetDto).ToList(), selectedShow == null ? ShowDtoFactory.GetMiniDto(_showService.GetCurrentShow()) : MiniShowDtoFactory.GetDto(selectedShow));
            if (req == null) return new DataResult<List<ISetAsSeenResult>>(StandardResults.Error);
            else
            {
                foreach (var setAsSeenResult in req.Where(x=>x.Success))
                {
                    var epi = episodes.FirstOrDefault(x => x.Ids.TraktId == setAsSeenResult.Episode.Ids.TraktId);
                    if (epi != null) epi.Watched = true;
                }
                return new DataResult<List<ISetAsSeenResult>>(req);
            }
        }


        public async Task<DataResult<ISetAsSeenResult>> SetAsSeen(IEpisode episode, IMiniShow miniShow = null)
        {
            if (episode == null) return new DataResult<ISetAsSeenResult>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<IGenericPostResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<ISetAsSeenResult>(StandardResults.Error);
            var req = await _dataService.SetAsSeen(UserTokenDtoFactory.GetDto(user), miniShow == null ? ShowDtoFactory.GetMiniDto(_showService.GetCurrentShow()) : MiniShowDtoFactory.GetDto(miniShow), EpisodeDtoFactory.GetDto(episode));
            if (req == null) return new DataResult<ISetAsSeenResult>(StandardResults.Error);
            //_showService.UpdateCurrentShow();
            episode.Watched = true;
            return new DataResult<ISetAsSeenResult>(req);
        }


        public async Task<DataResult<ISetAsSeenResult>> SetAsUnseen(IEpisode episode)   
        {
            if (episode == null) return new DataResult<ISetAsSeenResult>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<IGenericPostResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<ISetAsSeenResult>(StandardResults.Error);
            var show = _showService.GetCurrentShow();
            if (show == null) return new DataResult<ISetAsSeenResult>(StandardResults.Error);
            var req = await _dataService.SetAsUnseen(UserTokenDtoFactory.GetDto(user), ShowDtoFactory.GetDto(show), EpisodeDtoFactory.GetDto(episode));
            if (req == null) return new DataResult<ISetAsSeenResult>(StandardResults.Error);
            else
            {
                episode.Watched = false;
                //_showService.UpdateCurrentShow();
                return new DataResult<ISetAsSeenResult>(req);
            }
        }

        public async Task<DataResult<IGenericPostResult>> SetAsSeen(List<IEpisode> episodes, int tvdbId, string imdbid, string title, int year)
        {
            if (episodes == null || episodes.Count <= 0) return new DataResult<IGenericPostResult>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<IGenericPostResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            var req = await _dataService.SetAsSeen(UserTokenDtoFactory.GetDto(user), tvdbId, imdbid, title, year, episodes.Select(EpisodeDtoFactory.GetDto).ToList());
            if (req == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            else
            {
                _showService.UpdateCurrentShow();
                _showService.UpdateProgress();
                return new DataResult<IGenericPostResult>(req);
            }
        }

        public async Task<DataResult<IRateResult>> RateEpisode(int newValue, IEpisode episode)
        {
            if (newValue <= -1 || episode == null) return new DataResult<IRateResult>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<IRateResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<IRateResult>(StandardResults.Error);
            var currentShow = _showService.GetCurrentShow();
            if (currentShow == null) return new DataResult<IRateResult>(StandardResults.Error);
            var res = await _dataService.RateEpisode(UserTokenDtoFactory.GetDto(user), newValue, EpisodeDtoFactory.GetDto(episode), ShowDtoFactory.GetDto(currentShow));
            if (res == null || !res.Status) 
                return new DataResult<IRateResult>(StandardResults.Error);
            episode.RatedValue = newValue;
            return new DataResult<IRateResult>(res);
        }

        public async Task<DataResult<IGenericPostResult>> SetAsUnseen(int season, int episode)
        {
            if (season <= -1) return new DataResult<IGenericPostResult>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<IGenericPostResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            var show = _showService.GetCurrentShow();
            if (show == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            var req = await _dataService.SetAsUnseen(UserTokenDtoFactory.GetDto(user), show.Ids.TvDbId.Value, show.Ids.ImdbId, show.Title, show.Year.Value, season, episode);
            if (req == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            else
            {
                _showService.UpdateCurrentShow();
                return new DataResult<IGenericPostResult>(req);
            }
        }

        public async Task<DataResult<List<ISetAsSeenResult>>> SetAsUnseen(List<IEpisode> episodes)
        {
            if (episodes == null || episodes.Count <= 0) return new DataResult<List<ISetAsSeenResult>>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<IGenericPostResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<List<ISetAsSeenResult>>(StandardResults.Error);
            var show = _showService.GetCurrentShow();
            if (show == null) return new DataResult<List<ISetAsSeenResult>>(StandardResults.Error);
            var req = await _dataService.SetAsUnseen(UserTokenDtoFactory.GetDto(user), episodes.Select(EpisodeDtoFactory.GetDto).ToList(), ShowDtoFactory.GetDto(show));
            if (req == null) return new DataResult<List<ISetAsSeenResult>>(StandardResults.Error);
            foreach (var setAsSeenResult in req.Where(x => x.Success))
            {
                var epi = episodes.FirstOrDefault(x => x.Ids.TraktId == setAsSeenResult.Episode.Ids.TraktId);
                if (epi != null) epi.Watched = false;
            }
            return new DataResult<List<ISetAsSeenResult>>(req);
        }

        public async Task<DataResult<IRateResult>> RateEpisode(bool love, int season, int episode)
        {
            if (season <= -1 || episode <= -1) return new DataResult<IRateResult>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<IRateResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<IRateResult>(StandardResults.Error);
            var currentShow = _showService.GetCurrentShow();
            if (currentShow == null) return new DataResult<IRateResult>(StandardResults.Error);
            var res = await _dataService.RateEpisode(UserTokenDtoFactory.GetDto(user), love, currentShow.Title, currentShow.Ids.TvDbId.Value, currentShow.Year.Value, season, episode);
            if (res == null || !res.Status)
                return new DataResult<IRateResult>(StandardResults.Error);
            _showService.UpdateCurrentShow();
            return new DataResult<IRateResult>(res);
        }

        public async Task<DataResult<List<IRateResult>>> RateEpisodes(int value, List<IEpisode> episodes)
        {
            if (episodes == null) return new DataResult<List<IRateResult>>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<IRateResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<List<IRateResult>>(StandardResults.Error);
            var currentShow = _showService.GetCurrentShow();
            if (currentShow == null) return new DataResult<List<IRateResult>>(StandardResults.Error);
            var res = await _dataService.RateEpisodes(UserTokenDtoFactory.GetDto(user), value, episodes.Select(EpisodeDtoFactory.GetDto).ToList(), ShowDtoFactory.GetDto(currentShow));
            if (res == null)
                return new DataResult<List<IRateResult>>(StandardResults.Error);
            foreach (var rateResult in res.Where(x=>x.Status))
            {
                var episode = episodes.FirstOrDefault(x => x.Ids.TraktId == rateResult.Episode.Ids.TraktId);
                if (episode != null) episode.RatedValue = value;
            }
            return new DataResult<List<IRateResult>>(res);
        }

        public async Task<DataResult<ICheckinResult>> CheckIn(int season, int episode)
        {
            if (season <= -1 || episode <= -1) return new DataResult<ICheckinResult>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<ICheckinResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<ICheckinResult>(StandardResults.Error);
            var currentShow = _showService.GetCurrentShow();
            if (currentShow == null) return new DataResult<ICheckinResult>(StandardResults.Error);
            var res = await _dataService.CheckIn(UserTokenDtoFactory.GetDto(user), currentShow.Title, currentShow.Ids.ImdbId, currentShow.Ids.TvDbId.Value, currentShow.Year.Value, season, episode);
            if (res == null)
                return new DataResult<ICheckinResult>(StandardResults.Error);
            _showService.UpdateCurrentShow();
            return new DataResult<ICheckinResult>(res);
        }

        public async Task<DataResult<ICheckinResult>> CancelCheckIn()
        {
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<ICheckinResult>(StandardResults.Error);
            var res = await _dataService.CancelCheckIn(UserTokenDtoFactory.GetDto(user));
            if (res == null || res.Status == RequestResults.Failure)
                return new DataResult<ICheckinResult>(StandardResults.Error);
            return new DataResult<ICheckinResult>(res);
        }

        public async Task<DataResult<IMediaStream>> GetEpisodeSubtitles(string subtitlesLanguages, int season, int episode)
        {
            var currentShow = _showService.GetCurrentShow();
            if (currentShow == null) return new DataResult<IMediaStream>(StandardResults.Error);
            var res2 = await _dataService.GetLinks(currentShow.Ids.ImdbId, season, episode, subtitlesLanguages);
            if (res2 == null)
            {
                res2 = await _dataService.GetSubtitlesFromAzure(currentShow.Ids.ImdbId, season, episode, subtitlesLanguages);
                if (res2 == null) return new DataResult<IMediaStream>(StandardResults.Error);
            }
            return new DataResult<IMediaStream>(res2);
        }

        public async Task<DataResult<IMediaStream>> GetEpisodeLink(int season, int episode, DateTime episodeAirDate, string subtitlesLanguages, int? numberAbs)
        {
            try
            {
                if (season <= -1 || episode <= -1) return new DataResult<IMediaStream>(StandardResults.Error);
                //if (!await IsInternet()) return new DataResult<IMediaStream>(StandardResults.Offline);
                var currentShow = _showService.GetCurrentShow();
                if (currentShow == null) return new DataResult<IMediaStream>(StandardResults.Error);
                var absoluteEpisodeNumber = 0;
                if (currentShow.IsAnime)
                {
                    if (numberAbs != null)
                    {
                        absoluteEpisodeNumber = numberAbs.Value;
                    }
                    else
                    {
                        var x = await GetAbsoluteNumberFromTvDb(currentShow.Ids.TvDbId.Value, episodeAirDate);
                        if (x.IsOk && x.Data != null)
                        {
                            absoluteEpisodeNumber = x.Data.Value;
                        }
                        else
                        {
                            absoluteEpisodeNumber = episode;
                        }
                        
                    }
                 
                }
                var res =
                    await
                    _crawler.GetLinks(currentShow.Title, season, episode, currentShow.Runtime.Value, currentShow.Year.Value, currentShow.IsAnime ? CrawlerType.Anime : CrawlerType.Episode, absoluteEpisodeNumber, currentShow.Ids.ImdbId);
                if (res == null) return new DataResult<IMediaStream>(StandardResults.Error);
                var linksModel = Ioc.Container.Resolve<IMediaStream>();
                linksModel.Links = res;
                return new DataResult<IMediaStream>(linksModel);
            }
            catch (Exception)
            {
                return new DataResult<IMediaStream>(StandardResults.Error);
            }
        }

        public async Task<DataResult<IGenericPostResult>> AddToWatchlist(IEpisode episode)
        {
            if (episode == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<IGenericPostResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            var show = _showService.GetCurrentShow();
            if (show == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            var req = await _dataService.AddEpisodeToWatchlist(UserTokenDtoFactory.GetDto(user), show.Ids.TvDbId.Value, show.Ids.ImdbId, show.Title, show.Year.Value, new List<EpisodeDto> { EpisodeDtoFactory.GetDto(episode) });
            return req == null ? new DataResult<IGenericPostResult>(StandardResults.Error) : new DataResult<IGenericPostResult>(req);
        }

        public async Task<DataResult<IGenericPostResult>> AddEpisodesToWatchlist(List<IEpisode> episodes)
        {
            if (episodes == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<IGenericPostResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            var show = _showService.GetCurrentShow();
            if (show == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            var req = await _dataService.AddEpisodeToWatchlist(UserTokenDtoFactory.GetDto(user), show.Ids.TvDbId.Value, show.Ids.ImdbId, show.Title, show.Year.Value, episodes.Select(EpisodeDtoFactory.GetDto).ToList());
            return req == null ? new DataResult<IGenericPostResult>(StandardResults.Error) : new DataResult<IGenericPostResult>(req);
        }

        public async Task<DataResult<IGenericPostResult>> RemoveFromWatchlist(IEpisode episode)
        {
            if (episode == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<IGenericPostResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            var show = _showService.GetCurrentShow();
            if (show == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            var req = await _dataService.RemoveEpisodeFromWatchlist(UserTokenDtoFactory.GetDto(user), show.Ids.TvDbId.Value, show.Ids.ImdbId, show.Title, show.Year.Value, new List<EpisodeDto> { EpisodeDtoFactory.GetDto(episode) });
            return req == null ? new DataResult<IGenericPostResult>(StandardResults.Error) : new DataResult<IGenericPostResult>(req);
        }

        public async Task<DataResult<IGenericPostResult>> RemoveAllFromWatchlist(List<IEpisode> episodes)
        {
            if (episodes == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<IGenericPostResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            var show = _showService.GetCurrentShow();
            if (show == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            var req = await _dataService.RemoveEpisodeFromWatchlist(UserTokenDtoFactory.GetDto(user), show.Ids.TvDbId.Value, show.Ids.ImdbId, show.Title, show.Year.Value, episodes.Select(EpisodeDtoFactory.GetDto).ToList());
            return req == null ? new DataResult<IGenericPostResult>(StandardResults.Error) : new DataResult<IGenericPostResult>(req);
        }

        public async Task<DataResult<IEpisode>> GetFullEpisodeInfo(int tvdbId, int season, int episode)
        {
            if (tvdbId <= 0) return new DataResult<IEpisode>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<IGenericPostResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            var req = await _dataService.GetFullEpisodeInfo(UserTokenDtoFactory.GetDto(user), tvdbId, season, episode);
            if (req == null)
            {
                return new DataResult<IEpisode>(StandardResults.Error);
            }
            return new DataResult<IEpisode>(req);
        }

        public async Task<DataResult<int?>> GetAbsoluteNumberFromTvDb(int showTvDbId, DateTime episodeAirDate)
        {
            //if (!await IsInternet()) return new DataResult<IGenericPostResult>(StandardResults.Offline);
            var show = _showService.GetCurrentShow();
            if (show == null) return new DataResult<int?>(StandardResults.Error);
            var req = await _dataService.GetAbsoluteNumberFromTvDb(showTvDbId, episodeAirDate);
            return req == null ? new DataResult<int?>(StandardResults.Error) : new DataResult<int?>(req);
        }



        public List<ISubtitlesLanguage> GetListSubtitlesLanguage()
        {
            var list = new List<ISubtitlesLanguage>();
            var sub0 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub0.Language = "Disabled";
            sub0.LanguageId = null;
            list.Add(sub0);
            var sub1 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub1.Language = "English";
            sub1.LanguageId = "eng";
            list.Add(sub1);
            var sub2 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub2.Language = "Portuguese (pt-PT)";
            sub2.LanguageId = "por";
            list.Add(sub2);
            var sub16 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub16.Language = "Portuguese (pt-BR)";
            sub16.LanguageId = "pob";
            list.Add(sub16);
            var sub3 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub3.Language = "German";
            sub3.LanguageId = "ger";
            list.Add(sub3);
            var sub4 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub4.Language = "Spanish";
            sub4.LanguageId = "spa";
            list.Add(sub4);
            var sub5 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub5.Language = "Italian";
            sub5.LanguageId = "ita";
            list.Add(sub5);
            var sub6 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub6.Language = "French";
            sub6.LanguageId = "fra";
            list.Add(sub6);
            var sub7 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub7.Language = "Polish";
            sub7.LanguageId = "pol";
            list.Add(sub7);
            var sub8 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub8.Language = "Russian";
            sub8.LanguageId = "rus";
            list.Add(sub8);
            var sub10 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub10.Language = "Dutch";
            sub10.LanguageId = "dut";
            list.Add(sub10);
            var sub11 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub11.Language = "Swedish";
            sub11.LanguageId = "swe";
            list.Add(sub11);
            var sub12 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub12.Language = "Greek";
            sub12.LanguageId = "gre";
            list.Add(sub12);
            var sub13 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub13.Language = "Czech";
            sub13.LanguageId = "cze";
            list.Add(sub13);
            var sub14 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub14.Language = "Slovak";
            sub14.LanguageId = "slo";
            list.Add(sub14);
            var sub15 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub15.Language = "Icelandic";
            sub15.LanguageId = "ice";
            list.Add(sub15);
            return list;
        }
    }

}
