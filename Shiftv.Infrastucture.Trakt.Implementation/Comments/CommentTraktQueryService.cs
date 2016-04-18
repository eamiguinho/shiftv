using System.Threading.Tasks;
using Shiftv.Contracts.DataServices.Comments;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Comments
{
    class CommentTraktQueryService : ICommentTraktQueryService
    {
        public Task<string> GetCommentsShowById(int? imdbId)
        {
            //http://api.trakt.tv/show/comments.json/73a66219d4b25eba8b2ef444c2405352/the-walking-dead
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
             TraktConstants.ShiftvBaseApiUrl,
             TraktConstants.ShowsResource,
             TraktConstants.CommentsAction,
             imdbId));
        }

        public Task<string> CommentShow()
        {
            //http://api.trakt.tv/comment/show/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}",
        TraktConstants.ShiftvBaseApiUrl,
        TraktConstants.ShowsAction,
        TraktConstants.CommentResource));
        }

        public Task<string> EditCommentShow()
        {
            //http://trakt.tv/api/shout/edit/trakt
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}/{4}",
        TraktConstants.CheaterApiUrl,
        TraktConstants.ShoutResource,
        TraktConstants.EditAction,
        TraktConstants.TraktAction,
        TraktConstants.TraktKey));
        }

        public Task<string> GetCommentsByEpisode(int tvdbId, int season, int episodeNumber)
        {
            //http://api.trakt.tv/show/episode/comments.format/apikey/title/season/episode/type
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}{4}/{5}/{6}/{7}/{8}",
          TraktConstants.BaseApiUrl,
          TraktConstants.ShowResource,
          TraktConstants.EpisodeAction,
          TraktConstants.CommentsAction,
          TraktConstants.QueryType,
          TraktConstants.TraktKey,
          tvdbId,
          season,
          episodeNumber));
        }

        public Task<string> CommentEpisode()
        {
            //http://api.trakt.tv/comment/episode/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
        TraktConstants.BaseApiUrl,
        TraktConstants.CommentResource,
        TraktConstants.EpisodeAction,
        TraktConstants.TraktKey));
        }

        public Task<string> GetCommentsMovieById(int? imdbId)
        {
            //http://api.trakt.tv/movie/comments.json/73a66219d4b25eba8b2ef444c2405352/tt2294629
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
                        TraktConstants.ShiftvBaseApiUrl,
                        TraktConstants.MoviesResource,
                        TraktConstants.CommentsAction,
                        imdbId));
        }

        public Task<string> CommentMovie()
        {
            //http://api.trakt.tv/comment/movie/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}/",
        TraktConstants.ShiftvBaseApiUrl,
        TraktConstants.MoviesResource,
        TraktConstants.CommentResource
        ));
        }
    }
}
