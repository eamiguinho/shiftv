using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Shows;
using ShiftvAPI.Infrastucture.Trakt.Implementation.Helpers;

namespace ShiftvAPI.Infrastucture.Trakt.Implementation.Shows
{
    public class ShowTraktDataService : IShowTraktDataService
    {
        private IShowTraktQueryService _queryService
            ;

        public ShowTraktDataService(IShowTraktQueryService queryService)
        {
            _queryService = queryService;
        }
        public Task<Show> GetShowById(int id)
        {
            return Task.Run(async () =>
            {
                try
                {
                    //throw new Exception("BOOM");
                    var url = await _queryService.GetShowById(id);
                    Show x = await TraktDataServiceHelper.GetObjectWithoutCredentials<Show>(url);
                    return x;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<Show>> GetPopular(int page, int nItems)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetPopular(page, nItems);
                    List<Show> x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<Show>>(url);
                    return x;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<ShowTrending>> GetTrending(int page, int nItems)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetTrending(page, nItems);
                    List<ShowTrending> x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<ShowTrending>>(url);
                    return x;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<ShowUpdate>> GetUpdates(int page, int nItems, DateTime startDate)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetUpdates(startDate, page, nItems);
                    List<ShowUpdate> x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<ShowUpdate>>(url);
                    return x;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<People> GetPeople(int showId)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetPeople(showId);
                    People x = await TraktDataServiceHelper.GetObjectWithoutCredentials<People>(url);
                    return x;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<Comment>> GetComments(int page, int nItems, int showId)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetComments(showId, page, nItems);
                    List<Comment> x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<Comment>>(url);
                    return x;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<Season>> GetSeasons(int showId)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetSeasons(showId);
                    List<Season> x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<Season>>(url);
                    return x;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<Episode>> GetEpisodesBySeason(int showId, int season)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetEpisodesBySeason(showId, season);
                    List<Episode> x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<Episode>>(url);
                    return x;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<ShowSearchResult>> Search(string key)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.Search(key);
                    List<ShowSearchResult> x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<ShowSearchResult>>(url);
                    return x;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<Comment>> GetCommentsEpisode(int page, int limit, int showId, int season, int episode)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetCommentsEpisode(showId, season, episode, page, limit);
                    List<Comment> x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<Comment>>(url);
                    return x;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<Calendar>> GetCalendar(string token)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var date = DateTime.Now.AddDays(-6);
                    var numDays = 60;
                    var url = await _queryService.GetCalendar(date, numDays);
                    Dictionary<string, List<Calendar>> x = await TraktDataServiceHelper.GetObjectWithCredentials<Dictionary<string, List<Calendar>>>(url, token);
                    var calendarList = new List<Calendar>();
                    foreach (var calendar in x)
                    {
                        foreach (var s in x.Values)
                        {
                            foreach (var s1 in s)
                            {
                                calendarList.Add(new Calendar
                                {
                                    ShowName = s1.Show.Title,
                                    ShowIds = s1.Show.Ids,
                                    AirsAt = s1.AirsAt,
                                    Episode = s1.Episode,
                                    GroupKey = calendar.Key
                                });
                            }

                        }
                    }
                    return calendarList;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

       
    }
}
