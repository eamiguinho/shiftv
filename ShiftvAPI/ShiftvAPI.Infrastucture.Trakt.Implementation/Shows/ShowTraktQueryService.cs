using System;
using System.Threading.Tasks;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Shows;
using ShiftvAPI.Infrastucture.Trakt.Implementation.Helpers.Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace ShiftvAPI.Infrastucture.Trakt.Implementation.Shows
{
    class ShowTraktQueryService : IShowTraktQueryService
    {
        public Task<string> GetShowById(int id)
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}?{3}",
                TraktConstants.BaseApiUrl,
                TraktConstants.ShowsResource,
                id,
                TraktConstants.ExtendedImagesData));
        }

        public Task<string> GetPopular(int page, int nItems)
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}?{3}={4}&{5}={6}&{7}",
                TraktConstants.BaseApiUrl,
                TraktConstants.ShowsResource,
                TraktConstants.PopularAction,
                TraktConstants.Page,
                page,
                TraktConstants.Limit,
                nItems,
                TraktConstants.ExtendedImagesData
                ));
        }

        public Task<string> GetTrending(int page, int nItems)
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}?{3}={4}&{5}={6}&{7}",
                TraktConstants.BaseApiUrl,
                TraktConstants.ShowsResource,
                TraktConstants.TredingAction,
                TraktConstants.Page,
                page,
                TraktConstants.Limit,
                nItems,
                TraktConstants.ExtendedImagesData
                ));
        }

        public Task<string> GetUpdates(DateTime startDate, int page, int nItems)
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}?{4}={5}&{6}={7}&{8}",
                  TraktConstants.BaseApiUrl,
                  TraktConstants.ShowsResource,
                  TraktConstants.Updates,
                  startDate.Date.ToString("yyyy-MM-dd"),
                  TraktConstants.Page,
                  page,
                  TraktConstants.Limit,
                  nItems,
                  TraktConstants.ExtendedImagesData
                  ));
        }

        public Task<string> GetPeople(int showId)
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}?{4}",
                  TraktConstants.BaseApiUrl,
                  TraktConstants.ShowsResource,
                 showId,
                  TraktConstants.People,
                  TraktConstants.ExtendedImagesData
                  ));
        }

        public Task<string> GetComments(int showId)
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}?{4}",
                  TraktConstants.BaseApiUrl,
                  TraktConstants.ShowsResource,
                 showId,
                  TraktConstants.People,
                  TraktConstants.ExtendedImagesData
                  ));
        }

        public Task<string> GetComments(int showId, int page, int nItems)
        {

            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}?{4}&{5}={6}&{7}={8}",
                  TraktConstants.BaseApiUrl,
                  TraktConstants.ShowsResource,
                  showId,
                  TraktConstants.CommentsResource,
                  TraktConstants.ExtendedImagesData,
                  TraktConstants.Page,
                  page,
                  TraktConstants.Limit,
                  nItems
                  ));
        }

        public Task<string> GetSeasons(int showId)
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}?{4}",
               TraktConstants.BaseApiUrl,
               TraktConstants.ShowsResource,
              showId,
               TraktConstants.SeasonsAction,
               TraktConstants.ExtendedImagesData
               ));
        }

        public Task<string> GetEpisodesBySeason(int showId, int season)
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}/{4}?{5}",
            TraktConstants.BaseApiUrl,
            TraktConstants.ShowsResource,
           showId,
            TraktConstants.SeasonsAction,
            season,
            TraktConstants.ExtendedImagesData
            ));
        }

        public Task<string> Search(string key)
        {//https://api.trakt.tv/search?query=batman&type=type
            return Task.Run(() => string.Format("{0}/{1}?query={2}&type=show&{3}",
           TraktConstants.BaseApiUrl,
           TraktConstants.SearchResource,
          key,
          TraktConstants.ExtendedImagesData
           ));
        }

        public Task<string> GetCommentsEpisode(int showId, int season, int episode, int page, int limit)
        {
            //https://api.trakt.tv/shows/id/seasons/season/episodes/episode/comments
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}?{8}&{9}={10}&{11}={12}",
               TraktConstants.BaseApiUrl,
               TraktConstants.ShowsResource,
               showId,
               TraktConstants.SeasonsAction,
               season,
               TraktConstants.EpisodesAction,
               episode,
               TraktConstants.CommentsResource,
               TraktConstants.ExtendedImagesData,
               TraktConstants.Page,
               page,
               TraktConstants.Limit,
               limit
               ));
        }

        public Task<string> GetCalendar(DateTime date, int numDays)
        {
            //https://api.trakt.tv/shows/id/seasons/season/episodes/episode/comments
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}/{4}?{5}",
               TraktConstants.BaseApiUrl,
               TraktConstants.CalendarsResource,
               TraktConstants.ShowsResource,
               date.ToString("yyyy-MM-dd"),
               numDays,
               TraktConstants.ExtendedImagesData
               ));
        }

     
    }
}