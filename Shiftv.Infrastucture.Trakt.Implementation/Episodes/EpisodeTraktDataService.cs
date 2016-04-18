using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Comments;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Data.JsonTrakt;
using Shiftv.Contracts.Data.Media;
using Shiftv.Contracts.Data.Results;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.DataServices.Episodes;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Contracts.PlatformSpecificServices;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Episodes
{
    public class EpisodeTraktDataService : IEpisodeTraktDataService
    {
        private IEpisodeTraktQueryService _queryService;
        private IDataBackupService _backupService;

        public EpisodeTraktDataService(IEpisodeTraktQueryService queryService, IDataBackupService backupService)
        {
            _queryService = queryService;
            _backupService = backupService;
        }

        public Task<List<IEpisode>> GetEpisodeBySeason(UserTokenDto userAccount, int tvDbId, int season, string title)
        {
            return Task.Run(async () =>
            {
                try
                {
                    //throw new Exception("BOOM");
                    if (tvDbId < 0 || season < 0) return null;
                    var url = await _queryService.GetEpisodeBySeason(tvDbId, season);
                    List<EpisodeDto> x;
                    if (userAccount != null)
                    {
                        x = await TraktDataServiceHelper.GetObjectWithCredentials<List<EpisodeDto>>(url, userAccount);
                    }
                    else
                    {
                        x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<EpisodeDto>>(url);
                    }
                    if (x != null)
                    {
                        TraktDataServiceHelper.SaveToAzure(x, string.Format("{0}_{1}", tvDbId, season), BackupContainerTypes.Seasons, _backupService);
                        return x.Select(a => EpisodeDtoFactory.Create(a, title)).ToList();
                    }
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<IEpisode>> GetEpisodeBySeasonAzure(int tvDbId, int season, string title)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var trendingShows = await _backupService.GetFileFromAzure(string.Format("{0}_{1}", tvDbId, season), BackupContainerTypes.Seasons);
                    if (trendingShows == null) return null;
                    var objectReceived = JsonConvert.DeserializeObject<List<EpisodeDto>>(trendingShows);
                    return objectReceived.Select(a => EpisodeDtoFactory.CreateClean(a, title)).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }



        public Task<List<IUser>> GetWatchingNow(int tvDbId, int season, int episode)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (episode < 0 || tvDbId < 0 || season < 0) return null;
                    var url = await _queryService.GetWatchingNow(tvDbId, season, episode);
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<UserDto>>(url);
                    return x.Select(UserDtoFactory.Create).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<IGenericPostResult> SetAsSeen(UserTokenDto userAccount, int tvDbId, string imdbId, string title, int year, int season, int episode)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(imdbId) || episode < 0 || tvDbId < 0 || season < 0 || year < 0) return null;
                    var url = await _queryService.GetSetAsSeen();
                    var listEpisodes = new List<SetEpisodeAsSeenEpiInfoRequestJsonDto>
                    {
                        new SetEpisodeAsSeenEpiInfoRequestJsonDto
                        {
                            Episode = episode,
                            Season = season,
                            LastPlayed = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString()
                        }
                    };
                    var episodeAsSeenRequest = new SetEpisodeAsSeenRequestJsonDto
                    {
                        Episodes = listEpisodes,
                        TvDbId = tvDbId,
                        ImdbId = imdbId,
                        //Password = userAccount.PasswordEnc,
                        Title = title,
                        //Username = userAccount.Username,
                        Year = year
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(episodeAsSeenRequest));
                    var x = await TraktDataServiceHelper.PostWithCredentials<GenericPostResultDto>(url, myContent);
                    return GenericPostResultDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<IGenericPostResult> SetAsSeen(UserTokenDto userAccount, int tvDbId, string imdbId, string title, int year, List<EpisodeDto> episodes)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(imdbId) || tvDbId < 0 || year < 0) return null;
                    var url = await _queryService.GetSetAsSeen();
                    var listEpisodes = new List<SetEpisodeAsSeenEpiInfoRequestJsonDto>();
                    foreach (var episodeDto in episodes)
                    {
                        listEpisodes.Add(new SetEpisodeAsSeenEpiInfoRequestJsonDto
                        {
                            Episode = episodeDto.Number,
                            Season = episodeDto.Season,
                            LastPlayed = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString()
                        });
                    }
                    var episodeAsSeenRequest = new SetEpisodeAsSeenRequestJsonDto
                    {
                        Episodes = listEpisodes,
                        TvDbId = tvDbId,
                        ImdbId = imdbId,
                        //Password = userAccount.PasswordEnc,
                        Title = title,
                        //Username = userAccount.Username,
                        Year = year
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(episodeAsSeenRequest));
                    var x = await TraktDataServiceHelper.PostWithCredentials<GenericPostResultDto>(url, myContent);
                    return GenericPostResultDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<IGenericPostResult> SetAsUnseen(UserTokenDto userAccount, int tvDbId, string imdbId, string title, int year, List<EpisodeDto> episodes)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(imdbId) || tvDbId < 0 || year < 0) return null;
                    var url = await _queryService.GetSetAsUnseen();
                    var listEpisodes = new List<SetEpisodeAsSeenEpiInfoRequestJsonDto>();
                    foreach (var episodeDto in episodes)
                    {
                        listEpisodes.Add(new SetEpisodeAsSeenEpiInfoRequestJsonDto
                        {
                            Episode = episodeDto.Number,
                            Season = episodeDto.Season,
                            LastPlayed = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString()
                        });
                    }
                    var episodeAsSeenRequest = new SetEpisodeAsSeenRequestJsonDto
                    {
                        Episodes = listEpisodes,
                        TvDbId = tvDbId,
                        ImdbId = imdbId,
                        //Password = userAccount.PasswordEnc,
                        Title = title,
                        //Username = userAccount.Username,
                        Year = year
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(episodeAsSeenRequest));
                    var x = await TraktDataServiceHelper.PostWithCredentials<GenericPostResultDto>(url, myContent);
                    return GenericPostResultDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<IGenericPostResult> SetAsUnseen(UserTokenDto userAccount, int tvDbId, string imdbId, string title, int year, int season, int episode)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(imdbId) || episode < 0 || tvDbId < 0 || season < 0 || year < 0) return null;
                    var url = await _queryService.GetSetAsUnseen();
                    var listEpisodes = new List<SetEpisodeAsSeenEpiInfoRequestJsonDto>
                    {
                        new SetEpisodeAsSeenEpiInfoRequestJsonDto
                        {
                            Episode = episode,
                            Season = season,
                            LastPlayed = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString()
                        }
                    };
                    var episodeAsSeenRequest = new SetEpisodeAsSeenRequestJsonDto
                    {
                        Episodes = listEpisodes,
                        TvDbId = tvDbId,
                        ImdbId = imdbId,
                        //Password = userAccount.PasswordEnc,
                        Title = title,
                        //Username = userAccount.Username,
                        Year = year
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(episodeAsSeenRequest));
                    var x = await TraktDataServiceHelper.PostWithCredentials<GenericPostResultDto>(url, myContent);
                    return GenericPostResultDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }


        public Task<IRateResult> RateEpisode(UserTokenDto userAccount, bool rate, string title, int tvDbId, int year, int season, int episode)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null || string.IsNullOrEmpty(title) || tvDbId <= -1 || year <= -1 || season <= -1 || episode <= -1) return null;
                    var url = await _queryService.GetRateEpisode();
                    var rateReq = new RateEpisodeRequestJsonDto
                    {
                        //Username = userAccount.Username.Trim(),
                        //Password = userAccount.PasswordEnc,
                        Title = title,
                        TvDbId = tvDbId,
                        Year = year,
                        Rating = rate ? "love" : "hate",
                        Season = season,
                        Episode = episode
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(rateReq));
                    var x = await TraktDataServiceHelper.PostWithCredentials<RateResultDto>(url, myContent);
                    return RateResultDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<IRateResult> RateEpisodes(UserTokenDto userAccount, bool rate, int year, int tvdbId, string title, List<EpisodeDto> episodes)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null || episodes == null || year <= -1 || tvdbId <= -1 || string.IsNullOrEmpty(title)) return null;
                    var url = await _queryService.GetRateEpisodes();
                    var listEpisodes = episodes.Select(episodeDto => new RateEpisodeRequestJsonDto
                    {
                        Title = title,
                        TvDbId = tvdbId,
                        Year = year,
                        Rating = rate ? "love" : "hate",
                        Season = episodeDto.Season,
                        Episode = episodeDto.Number
                    }).ToList();
                    var rateReq = new RateEpisodesRequestJsonDto
                    {
                        //Username = userAccount.Username.Trim(),
                        //Password = userAccount.PasswordEnc,
                        Episodes = listEpisodes
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(rateReq));
                    var x = await TraktDataServiceHelper.PostWithCredentials<RateResultDto>(url, myContent);
                    return RateResultDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<IMediaStream> GetLinks(string imdbId, int season, int episode, string lang)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (string.IsNullOrEmpty(imdbId) || episode < 0 || season < 0) return null;
                    var url = await _queryService.GetLinks(imdbId, season, episode, lang);
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<MediaStreamDto>(url, 30);
                    if (x != null && x.Subtitles != null)
                    {
                        SendSubtitlesToAzure(x, lang, imdbId, season, episode);
                    }
                    return MediaStreamDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }




        public async Task<IMediaStream> GetSubtitlesFromAzure(string imdbId, int season, int episode, string subtitlesLanguages)
        {
            try
            {
                var listM = new List<MediaStreamDto>();
                var languagesplit = subtitlesLanguages.Split(',');
                foreach (var s in languagesplit)
                {
                    var trendingShows = await _backupService.GetFileFromAzure(imdbId + s + season + episode + "subtitles", BackupContainerTypes.Subtitles);
                    if (trendingShows == null) return MediaStreamDtoFactory.Create(null);
                    var objectReceived = JsonConvert.DeserializeObject<MediaStreamDto>(trendingShows);
                    listM.Add(objectReceived);
                }
                if (listM.Count > 0)
                {
                    var t = new MediaStreamDto { Subtitles = new List<SubtitlesInfoDto>() };
                    foreach (var mediaStreamDto in listM)
                    {
                        t.Subtitles.AddRange(mediaStreamDto.Subtitles);
                    }
                    return MediaStreamDtoFactory.Create(t);
                }
                return MediaStreamDtoFactory.Create(null);
            }
            catch (Exception)
            {
                return MediaStreamDtoFactory.Create(null);
            }
        }

        public Task<IGenericPostResult> SetAsSeen(UserTokenDto userAccount, int tvDbId, string imdbId, string title, int year, int season)
        {
            throw new NotImplementedException();
        }

        public Task<ISetAsSeenResult> SetAsSeen(UserTokenDto userAccount, MiniShowDto showDto, EpisodeDto episodeDto)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null || showDto == null || episodeDto == null) return null;
                    var url = await _queryService.GetSetAsSeen();
                    var listEpisodes = new List<SetAsSeenJsonDto>
                    {
                        new SetAsSeenJsonDto()
                        {
                            Episode = new EpisodeRequestJsonDto
                            {
                                Title = episodeDto.Title,
                                 Ids = episodeDto.Ids,
                                 Number = episodeDto.Number,
                                 Season = episodeDto.Season
                            },
                            Show = new ShowRequestJsonDto
                            {
                                Title = showDto.Title,
                                Ids = showDto.Ids,
                                Year = showDto.Year
                            },
                            Watched = true,
                            WatchedAt = DateTime.Now
                        }
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(listEpisodes));
                    var x = await TraktDataServiceHelper.PostWithCredentials<List<SetAsSeenResultJsonDto>>(url, myContent,userAccount.AccessToken);
                    if (x != null && x.Count == 1) return SetAsSeenResultJsonDtoFactory.Create(x[0]);
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }


        public Task<List<ISetAsSeenResult>> SetAsSeen(UserTokenDto userAccount, List<EpisodeDto> episodeDtos, MiniShowDto showDto)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null || showDto == null || episodeDtos == null) return null;
                    var url = await _queryService.GetSetAsSeen();
                    var listEpisodes = new List<SetAsSeenJsonDto>();
                    foreach (var episodeDto in episodeDtos)
                    {
                        var episodeSeen = new SetAsSeenJsonDto()
                        {
                            Episode = new EpisodeRequestJsonDto
                            {
                                Title = episodeDto.Title,
                                Ids = episodeDto.Ids,
                                Number = episodeDto.Number,
                                Season = episodeDto.Season
                            },
                            Show = new ShowRequestJsonDto
                            {
                                Title = showDto.Title,
                                Ids = showDto.Ids,
                                Year = showDto.Year
                            },
                            Watched = true,
                            WatchedAt = DateTime.Now
                        };
                        listEpisodes.Add(episodeSeen);
                    }
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(listEpisodes));
                    var x = await TraktDataServiceHelper.PostWithCredentials<List<SetAsSeenResultJsonDto>>(url, myContent, userAccount.AccessToken);
                    return x != null ? x.Select(SetAsSeenResultJsonDtoFactory.Create).ToList() : null;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<ISetAsSeenResult>> SetAsUnseen(UserTokenDto userAccount, List<EpisodeDto> episodeDtos, ShowDto showDto)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null || showDto == null || episodeDtos == null) return null;
                    var url = await _queryService.GetSetAsSeen();
                    var listEpisodes = new List<SetAsSeenJsonDto>();
                    foreach (var episodeDto in episodeDtos)
                    {
                        var episodeSeen = new SetAsSeenJsonDto()
                        {
                            Episode = new EpisodeRequestJsonDto
                            {
                                Title = episodeDto.Title,
                                Ids = episodeDto.Ids,
                                Number = episodeDto.Number,
                                Season = episodeDto.Season
                            },
                            Show = new ShowRequestJsonDto
                            {
                                Title = showDto.Title,
                                Ids = showDto.Ids,
                                Year = showDto.Year
                            },
                            Watched = false,
                            WatchedAt = DateTime.Now
                        };
                        listEpisodes.Add(episodeSeen);
                    }
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(listEpisodes));
                    var x = await TraktDataServiceHelper.PostWithCredentials<List<SetAsSeenResultJsonDto>>(url, myContent, userAccount.AccessToken);
                    return x != null ? x.Select(SetAsSeenResultJsonDtoFactory.Create).ToList() : null;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }


        public Task<ISetAsSeenResult> SetAsUnseen(UserTokenDto userAccount, ShowDto showDto, EpisodeDto episodeDto)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null || showDto == null || episodeDto == null) return null;
                    var url = await _queryService.GetSetAsSeen();
                    var listEpisodes = new List<SetAsSeenJsonDto>
                    {
                        new SetAsSeenJsonDto()
                        {
                            Episode = new EpisodeRequestJsonDto
                            {
                                Title = episodeDto.Title,
                                 Ids = episodeDto.Ids,
                                 Number = episodeDto.Number,
                                 Season = episodeDto.Season
                            },
                            Show = new ShowRequestJsonDto
                            {
                                Title = showDto.Title,
                                Ids = showDto.Ids,
                                Year = showDto.Year
                            },
                            Watched = false,
                            WatchedAt = DateTime.Now
                        }
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(listEpisodes));
                    var x = await TraktDataServiceHelper.PostWithCredentials<SetAsSeenResultJsonDto>(url, myContent, userAccount.AccessToken);
                    if (x != null) return SetAsSeenResultJsonDtoFactory.Create(x);
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<IRateResult> RateEpisode(UserTokenDto userAccount, int rate, EpisodeDto episodeDto, ShowDto showDto)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null || episodeDto == null || showDto == null) return null;
                    var url = await _queryService.GetRateEpisode();
                    var listEpisodes = new List<RateRequestJsonDto>
                    {
                        new RateRequestJsonDto()
                        {
                            Episode = new EpisodeRequestJsonDto
                            {
                                Title = episodeDto.Title,
                                 Ids = episodeDto.Ids,
                                 Number = episodeDto.Number,
                                 Season = episodeDto.Season
                            },
                            Show = new ShowRequestJsonDto
                            {
                                Title = showDto.Title,
                                Ids = showDto.Ids,
                                Year = showDto.Year
                            },
                            Rating = rate
                        }
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(listEpisodes));
                    var x = await TraktDataServiceHelper.PostWithCredentials<List<RateResultDto>>(url, myContent, userAccount.AccessToken);
                    if (x != null && x.Count == 1) return RateResultDtoFactory.Create(x[0]);
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<IRateResult>> RateEpisodes(UserTokenDto userAccount, int rate, List<EpisodeDto> episodeDtos, ShowDto showDto)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null || episodeDtos == null || showDto == null) return null;
                    var url = await _queryService.GetRateEpisode();
                    var listEpisodes = new List<RateRequestJsonDto>();
                    foreach (var episodeDto in episodeDtos)
                    {
                        var episodeRate = new RateRequestJsonDto()
                        {
                            Episode = new EpisodeRequestJsonDto
                            {
                                Title = episodeDto.Title,
                                Ids = episodeDto.Ids,
                                Number = episodeDto.Number,
                                Season = episodeDto.Season
                            },
                            Show = new ShowRequestJsonDto
                            {
                                Title = showDto.Title,
                                Ids = showDto.Ids,
                                Year = showDto.Year
                            },
                            Rating = rate
                        };
                        listEpisodes.Add(episodeRate);
                    }
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(listEpisodes));
                    var x = await TraktDataServiceHelper.PostWithCredentials<List<RateResultDto>>(url, myContent, userAccount.AccessToken);
                    if (x != null) return x.Select(RateResultDtoFactory.Create).ToList();
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

     

        private void SendSubtitlesToAzure(MediaStreamDto mediaStreamDto, string language, string imdbId, int season, int episode)
        {
            var languagesplit = language.Split(',');
            foreach (var s in languagesplit)
            {
                var t = new MediaStreamDto { Subtitles = new List<SubtitlesInfoDto>() };
                var s1 = s;
                t.Subtitles.AddRange(mediaStreamDto.Subtitles.Where(x => x.LanguageId == s1));
                TraktDataServiceHelper.SaveToAzure(t, imdbId + s1 + season + episode + "subtitles", BackupContainerTypes.Subtitles, _backupService, true);
            }
        }

        public Task<IEpisode> GetFullEpisodeInfo(UserTokenDto userAccount, int tvDbId, int season, int episode)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (tvDbId <= -1 || season <= -1 || episode <= -1) return null;
                    var url = await _queryService.GetFullEpisodeInfo(tvDbId, season, episode);
                    FullEpisodeDto x;
                    if (userAccount != null)
                    {
                        x = await TraktDataServiceHelper.GetObjectWithCredentials<FullEpisodeDto>(url, userAccount);
                    }
                    else
                    {
                        x = await TraktDataServiceHelper.GetObjectWithoutCredentials<FullEpisodeDto>(url);
                    }
                    if (x == null)
                    {
                        return null;
                    }
                    return EpisodeDtoFactory.Create(x.Episode, x.Show.Title);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }



        public Task<int?> GetAbsoluteNumberFromTvDb(int showTvDbId, DateTime episodeAirDate)
        {
            return Task.Run(async () =>
            {
                int maxTry = 0;
                while (maxTry != 2)
                {
                    episodeAirDate = episodeAirDate.AddDays(maxTry);
                    try
                    {
                        if (showTvDbId <= -1) return null;
                        var url = await _queryService.GetAbsoluteNumberFromTvDb(showTvDbId, episodeAirDate);
                        var x = await TraktDataServiceHelper.GetStremString(url);
                        var res = Regex.Split(x, "<absolute_number>");
                        var res2 = Regex.Split(res[1], "</absolute_number>");
                        int episodeN;
                        var canParse = int.TryParse(res2[0], out episodeN);
                        return canParse ? (int?)episodeN : null;
                    }
                    catch (Exception)
                    {
                        maxTry++;
                    }
                }

                return null;
            });
        }


        public Task<ICheckinResult> CheckIn(UserTokenDto userAccount, string title, string imdbId, int tvDbId, int year, int season, int episode)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(imdbId) || tvDbId <= -1 || year <= -1 || season <= -1 || episode <= -1) return null;
                    var url = await _queryService.GetCheckIn();
                    var rateReq = new CheckInEpisodeRequestJsonDto
                    {
                        //Username = userAccount.Username.Trim(),
                        //Password = userAccount.PasswordEnc,
                        Title = title,
                        TvDbId = tvDbId,
                        ImdbId = imdbId,
                        Year = year,
                        Season = season,
                        Episode = episode
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(rateReq));
                    var x = await TraktDataServiceHelper.PostWithCredentials<CheckInResultDto>(url, myContent);
                    return CheckInResultDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }
        public Task<ICheckinResult> CancelCheckIn(UserTokenDto userAccount)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null) return null;
                    var url = await _queryService.GetCancelCheckIn();
                    var rateReq = new CheckInEpisodeRequestJsonDto
                    {
                        //Username = userAccount.Username.Trim(),
                        //Password = userAccount.PasswordEnc
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(rateReq));
                    var x = await TraktDataServiceHelper.PostWithCredentials<CheckInResultDto>(url, myContent);
                    return CheckInResultDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<IGenericPostResult> AddEpisodeToWatchlist(UserTokenDto userAccount, int tvDbId, string imdbId, string title, int year, List<EpisodeDto> episodes)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null || string.IsNullOrEmpty(title) || tvDbId <= -1 || year <= -1 || episodes == null) return null;
                    var url = await _queryService.AddEpisodeToWatchList();
                    var list = episodes.Select(episodeDto => new EpisodeData { Season = episodeDto.Season, Episode = episodeDto.Number }).ToList();
                    var rateReq = new AddEpisodeToWatchListRequestJsonDto
                    {
                        //Username = userAccount.Username.Trim(),
                        //Password = userAccount.PasswordEnc,
                        Episodes = list,
                        Title = title,
                        TvdbId = tvDbId,
                        ImdbId = imdbId
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(rateReq));
                    var x = await TraktDataServiceHelper.PostWithCredentials<GenericPostResultDto>(url, myContent);
                    return GenericPostResultDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<IGenericPostResult> RemoveEpisodeFromWatchlist(UserTokenDto userAccount, int tvDbId, string imdbId, string title, int year, List<EpisodeDto> episodes)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null || string.IsNullOrEmpty(title) || tvDbId <= -1 || year <= -1 || episodes == null) return null;
                    var url = await _queryService.RemoveEpisodeFromWatchlist();
                    var list = episodes.Select(episodeDto => new EpisodeData { Season = episodeDto.Season, Episode = episodeDto.Number }).ToList();
                    var rateReq = new AddEpisodeToWatchListRequestJsonDto
                    {
                        //Username = userAccount.Username.Trim(),
                        //Password = userAccount.PasswordEnc,
                        Episodes = list,
                        Title = title,
                        TvdbId = tvDbId,
                        ImdbId = imdbId
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(rateReq));
                    var x = await TraktDataServiceHelper.PostWithCredentials<GenericPostResultDto>(url, myContent);
                    return GenericPostResultDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }


    }

}
