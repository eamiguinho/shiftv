using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Data.PostObjects;
using ShiftvAPI.Contracts.Data.Sync;
using ShiftvAPI.Contracts.Helpers;
using ShiftvAPI.Contracts.Infrastucture.Shiftv;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Shows;
using ShiftvAPI.Contracts.Services.Shows;

namespace ShiftvAPI.Services.Implementation.Shows
{
    public class ShowService : IShowService
    {
        private IShowTraktDataService _showTraktDataService;
        private IShowShiftvDataService _showShiftvDataService;
        private ISyncShiftvDataService _syncShiftvDataService;

        public ShowService(IShowTraktDataService showTraktDataService, IShowShiftvDataService showShiftvDataService, ISyncShiftvDataService syncShiftvDataService)
        {
            _showTraktDataService = showTraktDataService;
            _showShiftvDataService = showShiftvDataService;
            _syncShiftvDataService = syncShiftvDataService;
        }

        public async Task<DataResult<Show>> GetShowById(int id, string token)
        {
            if (id < 0) return new DataResult<Show>(StandardResults.Error);
            var showData = await _showShiftvDataService.GetShowById(id);
            if (showData == null)
            {
                showData = await _showTraktDataService.GetShowById(id);
            }
            if (showData == null)
            {
                return new DataResult<Show>(StandardResults.Error);
            }
            if (showData.Seasons == null || showData.Seasons.Count == 0 || showData.Seasons.Any(x=>x.Episodes == null))
            {
                var seasons = await GetSeasons(id, token);
                if (seasons.IsOk && seasons.Data != null)
                {
                    showData.Seasons = seasons.Data;
                }
                await SaveShowDb(showData);
            }
            CheckUserRating(token, showData);
            CheckUserRatingEpisodes(id, showData.Seasons, token);
            CheckUserWatchedEpisodes(id, showData.Seasons, token);
            return new DataResult<Show>(showData);
        }



        public async Task<DataResult<List<MiniShow>>> GetTrending(int page, int limit, string token)
        {
            var showData = await _showShiftvDataService.GetTrending(page, limit);
            if (showData == null)
            {
                var fullshowData = await _showTraktDataService.GetTrending(page, limit);
                if (fullshowData == null) return new DataResult<List<MiniShow>>(StandardResults.Error);
                var listMini = fullshowData.Select(show => new MiniShow
                {
                    Fanart = show.Show.Images.Fanart,
                    Ids = show.Show.Ids,
                    Network = show.Show.Network,
                    Rating = show.Show.Rating,
                    Title = show.Show.Title,
                    Votes = show.Show.Votes,
                    FirstAired = show.Show.FirstAired,
                    Status =  show.Show.Status,
                    Year = show.Show.Year,
                    Genres = show.Show.Genres
                }).ToList();
                showData = listMini;
                await _showShiftvDataService.SaveTrending(listMini);
                //foreach (var showTrending in fullshowData)
                //{
                //    await ForceShowUpdate(showTrending.Show.Ids.TraktId.Value, token);

                //   //s await SaveShowDb(showTrending.Show);
                //}
            }
            CheckUserRatings(token, showData);
            return new DataResult<List<MiniShow>>(showData);
        }

        private void CheckUserRatings(string token, List<MiniShow> showData)
        {
            if (string.IsNullOrEmpty(token)) return;
            var userRatings = _syncShiftvDataService.GetShowRatingsByUser(token);
            if (userRatings == null) return;
            foreach (var miniShow in showData)
            {
                if (userRatings.Any(x => miniShow.Ids.TraktId != null && x.TraktId == miniShow.Ids.TraktId.Value))
                {
                    var userRating =
                        userRatings.FirstOrDefault(x => miniShow.Ids.TraktId != null && x.TraktId == miniShow.Ids.TraktId.Value);
                    if (userRating != null) {
                        miniShow.UserRating = userRating.Rating;
                    }
                }
            }
          
        }

        private void CheckUserRating(string token, Show showData)
        {
            if (string.IsNullOrEmpty(token)) return;
            var userRatings = _syncShiftvDataService.GetShowRatingsByUser(token);
            if (userRatings == null) return;
            var userRatingForThisShow = userRatings.FirstOrDefault(x => showData.Ids.TraktId != null && x.TraktId == showData.Ids.TraktId.Value);
            if (userRatingForThisShow != null)
            {
                showData.UserRating = userRatingForThisShow.Rating;
            }
        }

        public async Task<DataResult<List<MiniShow>>> GetPopular(int page, int limit, string token)
        {
            var showData = await _showShiftvDataService.GetPopular(page, limit);
            if (showData == null)
            {
                var fullshowData = await _showTraktDataService.GetPopular(page, limit);
                if (fullshowData == null) return new DataResult<List<MiniShow>>(StandardResults.Error);
                var listMini = fullshowData.Select(show => new MiniShow 
                {
                    Fanart = show.Images.Fanart,
                    Ids = show.Ids,
                    Network = show.Network,
                    Rating = show.Rating,
                    Title = show.Title,
                    Votes = show.Votes,
                    FirstAired = show.FirstAired,Year = show.Year, Genres = show.Genres,Status = show.Status
                }).ToList();
                showData = listMini;
                await _showShiftvDataService.SavePopular(listMini);
                foreach (var show in fullshowData)
                {
                    await SaveShowDb(show);
                }
            }
            CheckUserRatings(token, showData);
            return new DataResult<List<MiniShow>>(showData);
        }

        public async void UpdateData()
        {
            try
            {
                var page = 1;
                var lastUpdate = _showShiftvDataService.GetLastUpdate() ?? DateTime.Now.AddDays(-5);
                while (true)
                {
                    var showData = await _showTraktDataService.GetUpdates(page, 100, lastUpdate);
                    if (showData == null || showData.Count == 0)
                    {
                        _showShiftvDataService.SaveLastUpdate(DateTime.Now);
                        return;
                    }
                    foreach (var showUpdate in showData)
                    {
                        if (!string.IsNullOrEmpty(showUpdate.Show.Ids.ImdbId)) await SaveShowDb(showUpdate.Show);
                    }
                    page++;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task<DataResult<People>> GetPeople(int showId)
        {
            var peopleData = await _showShiftvDataService.GetPeople(showId);
            if (peopleData == null)
            {
                peopleData = await _showTraktDataService.GetPeople(showId);
                if (peopleData == null) return new DataResult<People>(StandardResults.Error);
                await _showShiftvDataService.SavePeople(peopleData, showId);
            }
            return new DataResult<People>(peopleData);
        }

        public async Task<DataResult<List<MiniShow>>> Search(string key)
        {
            var traktSearch = await _showTraktDataService.Search(key);
            if (traktSearch == null || traktSearch.Count <= 0)
            {
                var searchData = await _showShiftvDataService.Search(key);
                return searchData != null ? new DataResult<List<MiniShow>>(searchData) : new DataResult<List<MiniShow>>(StandardResults.Error);
            }
            var list = traktSearch.OrderByDescending(x => x.Score).Select(show => new MiniShow
            {
                Fanart = show.Show.Images.Fanart,
                Ids = show.Show.Ids,
                Network = show.Show.Network,
                Rating = show.Show.Rating,
                Title = show.Show.Title,
                Votes = show.Show.Votes,
                FirstAired = show.Show.FirstAired,
                Status = show.Show.Status,
                Genres = show.Show.Genres,
                Year = show.Show.Year,

            }).ToList();
            return new DataResult<List<MiniShow>>(list);
        }

        public async Task<DataResult<List<Season>>> GetSeasons(int id, string token)
        {
            var seasons =  _showShiftvDataService.GetSeasons(id);
            if (seasons == null || seasons.Any(x => x.Episodes == null) || seasons.Count == 0)
            {
                var x = await _showTraktDataService.GetSeasons(id);
                if (x != null)
                {
                    foreach (var season in x.Where(season => season.Number != null))
                    {
                        var episodes = await _showTraktDataService.GetEpisodesBySeason(id, season.Number.Value);
                        if (episodes != null)
                        {
                            season.Episodes = episodes;
                        }
                    }
                    if (x.All(a => a.Episodes != null))  _showShiftvDataService.SaveSeason(id, x);
                }
                seasons = x;
            }
            
            return seasons != null && seasons.Count > 0
                  ? new DataResult<List<Season>>(seasons)
                  : new DataResult<List<Season>>(StandardResults.Error);
        }

        private void CheckUserWatchedEpisodes(int showId, List<Season> seasons, string token, List<WatchedEpisodes> userShowsIds = null)
        {
            if (string.IsNullOrEmpty(token) || seasons == null || seasons.Count == 0) return;
            var userEpisodesRatings = userShowsIds ?? _syncShiftvDataService.GetEpisodeWatchedByUser(token);
            if (userEpisodesRatings == null) return;
            var episodesSeenOfCurrentShow = userEpisodesRatings.Where(x => x.TraktShowId == showId);
            foreach (var season in seasons)
            {
                foreach (var episodes in season.Episodes.Where(episodes => episodesSeenOfCurrentShow.Any(
                    x => x.EpisodeNumber == episodes.Number && x.SeasonNumber == episodes.Season)))
                {
                    episodes.Watched = true;
                }
            }
        }

        private void CheckUserRatingEpisodes(int showId, List<Season> seasons, string token)
        {
            if (string.IsNullOrEmpty(token) || seasons == null || seasons.Count == 0) return;
            var userEpisodesRatings = _syncShiftvDataService.GetEpisodeRatingsByUser(token);
            if (userEpisodesRatings == null) return;
            foreach (var userEpisodesRating in userEpisodesRatings.Where(x=>x.TraktId == showId))
            {
                foreach (var episode in seasons.Select(season => season.Episodes.Any(x => x.Number == userEpisodesRating.Episode)
                    ? season.Episodes.FirstOrDefault(x => x.Number == userEpisodesRating.Episode)
                    : null).Where(episode => episode != null))
                {
                    episode.UserRating = userEpisodesRating.Rating;
                }
            }
        }

        public async Task<DataResult<List<Comment>>> GetComments(int showId, int page, int limit)
        {
            //get comments from trakt
            var x = await _showTraktDataService.GetComments(page, limit, showId);
            //get comments from shiftv
            var shiftvComments = await _showShiftvDataService.GetComments(showId);
            var list = new List<Comment>();
            if(x != null && x.Count > 0)list.AddRange(x);
            if(shiftvComments != null && shiftvComments.Count > 0) list.AddRange(shiftvComments);
            return new DataResult<List<Comment>>(list);
        }

        public async Task<DataResult<List<Comment>>> GetCommentsEpisode(int showId, int season, int episode, int page, int limit)
        {
            var x = await _showTraktDataService.GetCommentsEpisode(page, limit, showId, season, episode);
            return x != null ? new DataResult<List<Comment>>(x) : new DataResult<List<Comment>>(StandardResults.Error);
        }

        public async Task<DataResult<Season>> GetLastSeason(int showId, string token)
        {
            var seasons = _showShiftvDataService.GetLastSeason(showId);
            if (seasons == null)
            {
                var x = await _showTraktDataService.GetSeasons(showId);
                if (x != null)
                {
                    foreach (var season in x.Where(season => season.Number != null))
                    {
                        var episodes = await _showTraktDataService.GetEpisodesBySeason(showId, season.Number.Value);
                        if (episodes != null)
                        {
                            season.Episodes = episodes;
                        }
                    }
                    if (x.All(a => a.Episodes != null)) _showShiftvDataService.SaveSeason(showId, x);
                }
                if (x != null) seasons = x.OrderByDescending(a=>a.Number).LastOrDefault();
            }

            return seasons != null
                  ? new DataResult<Season>(seasons)
                  : new DataResult<Season>(StandardResults.Error);
        }

        public async Task<DataResult<List<ShowProgress>>> GetProgress(string token)
        {
            var userShowsIds = _syncShiftvDataService.GetEpisodeWatchedByUser(token);
            var userShows = await _showShiftvDataService.GetUserShows(token, userShowsIds);
            if (userShows == null || userShows.Count == 0)
                return new DataResult<List<ShowProgress>>(StandardResults.Error);
            var list = new List<ShowProgress>();
            foreach (var show in userShows)
            {
                if (show.Ids.TraktId == null) continue;
                var seasons = await GetSeasons(show.Ids.TraktId.Value, token);
                if (!seasons.IsOk | seasons.Data == null) continue;
                var showCalendar = new ShowProgress();
                showCalendar.EpisodesLeft = new List<Episode>();
                CheckUserWatchedEpisodes(show.Ids.TraktId.Value, seasons.Data, token, userShowsIds);
                foreach (var season in seasons.Data.Where(x=>x.Number != 0))
                {
                    foreach (var episode in season.Episodes.Where(x=>!x.Watched))
                    {
                        if (episode.FirstAired != null)
                        {
                            var date = DateTime.Parse(episode.FirstAired);
                            var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            if (date <= today)
                            {
                                if (showCalendar.EpisodesLeft != null) showCalendar.EpisodesLeft.Add(episode);
                            }
                        }
                    }
                }
                if (showCalendar.EpisodesLeft == null || showCalendar.EpisodesLeft.Count <= 0) continue;
                showCalendar.Show = show;
                showCalendar.TotalEpisodes = seasons.Data.Where(x=>x.Number != 0).Sum(x => x.EpisodeCount);
                list.Add(showCalendar);
            }
            return new DataResult<List<ShowProgress>>(list);
        }

        public async Task<DataResult<Show>> ForceShowUpdate(int id, string token)
        {
            if (id < 0) return new DataResult<Show>(StandardResults.Error);
            var  showData = await _showTraktDataService.GetShowById(id);
            if (showData == null)
            {
                return new DataResult<Show>(StandardResults.Error);
            }
            if (showData.Seasons == null || showData.Seasons.Count == 0 || showData.Seasons.Any(x => x.Episodes == null))
            {
                var x = await _showTraktDataService.GetSeasons(id);
                if (x != null)
                {
                    foreach (var season in x.Where(season => season.Number != null))
                    {
                        var episodes = await _showTraktDataService.GetEpisodesBySeason(id, season.Number.Value);
                        if (episodes != null)
                        {
                            season.Episodes = episodes;
                        }
                    }
                    if (x.All(a => a.Episodes != null)) _showShiftvDataService.SaveSeason(id, x);
                }
                var seasons = x;
                if (seasons != null)
                {
                    showData.Seasons = seasons;
                }
                await SaveShowDb(showData);
            }
            CheckUserRating(token, showData);
            CheckUserRatingEpisodes(id, showData.Seasons, token);
            CheckUserWatchedEpisodes(id, showData.Seasons, token);
            return new DataResult<Show>(showData);
        }


        private async Task SaveShowDb(Show showData)
        {
            try
            {
                if (showData.Ids.TraktId != null)
                {
                    var x = await _showTraktDataService.GetSeasons(showData.Ids.TraktId.Value);
                    if (x != null)
                    {
                        foreach (var season in x.Where(season => season.Number != null))
                        {
                            var episodes = await _showTraktDataService.GetEpisodesBySeason(showData.Ids.TraktId.Value, season.Number.Value);
                            if (episodes != null)
                            {
                                season.Episodes = episodes;
                            }
                        }
                        if (x.All(a => a.Episodes != null)) _showShiftvDataService.SaveSeason(showData.Ids.TraktId.Value, x);
                    }
                    var seasons = x;
                    if (seasons != null)
                    {
                        showData.Seasons = seasons;
                    }
                }
                if(showData.Seasons != null &&  showData.Ids.TraktId != null) _showShiftvDataService.SaveSeason(showData.Ids.TraktId.Value, showData.Seasons);
                _showShiftvDataService.SaveShow(showData);
            }
            catch (Exception e)
            {
                
            }
        }
    }

}
