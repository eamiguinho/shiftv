using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.DataServices.Comments;
using Shiftv.Contracts.Domain.Comments;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Services;
using Shiftv.Contracts.Services.Accounts;
using Shiftv.Contracts.Services.Comments;
using Shiftv.Contracts.Services.Movies;
using Shiftv.Contracts.Services.Shows;
using Shiftv.Global;

namespace Shiftv.Services.Implementation.Comments
{
    public class CommentService : ServiceHelper, ICommentService
    {
        private ICommentTraktDataService _dataService;
        private IUserService _userService;
        private IShowService _showService;
        private IMovieService _movieService;

        public CommentService(ICommentTraktDataService dataService, IUserService userService, IShowService showService = null, IMovieService movieService = null)
        {
            _dataService = dataService;
            _userService = userService;
            _showService = showService;
            _movieService = movieService;
        }

        public async Task<DataResult<List<IComment>>> GetCommentsShowById()
        {
            //if (!await IsInternet()) return new DataResult<List<IComment>>(StandardResults.Offline);
            if (_showService == null) return new DataResult<List<IComment>>(StandardResults.Error);
            var show = _showService.GetCurrentShow();
            if (show == null) return new DataResult<List<IComment>>(StandardResults.Error);
            var res = await _dataService.GetCommentsShowById(IdsDtoFactory.GetDto(show.Ids)) ?? new List<IComment>();
            if (res.Count == 0) return new DataResult<List<IComment>>(StandardResults.Error);
            return new DataResult<List<IComment>>(res);
        }

        public async Task<DataResult<ICommentResult>> CommentsShow(string comment, bool isSpoiler, bool isReview)
        {
            if (string.IsNullOrEmpty(comment)) return new DataResult<ICommentResult>(StandardResults.Error);
            comment = AddShiftvBrandHastag(comment);
            //if (!await IsInternet()) return new DataResult<ICommentResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<ICommentResult>(StandardResults.Error);
            var show = _showService.GetCurrentShow();
            if (show == null) return new DataResult<ICommentResult>(StandardResults.Error);
            var res = await _dataService.CommentShow(UserTokenDtoFactory.GetDto(user), comment, ShowDtoFactory.GetDto(show), isSpoiler, isReview);
            if (res == null) return new DataResult<ICommentResult>(StandardResults.Error);
            else
            {
                return new DataResult<ICommentResult>(res);
            }
        }

        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }

        private string AddShiftvBrandHastag(string comment)
        {
            return string.Format("{0} {1}", comment, "#ShiftvW8");
        }

        public async Task<DataResult<List<IComment>>> GetEpisodeComments(IShow show, int season, int episode)
        {
            if (show == null || season < 0 || episode < 0) return new DataResult<List<IComment>>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<List<IComment>>(StandardResults.Offline);
            var res = await _dataService.GetCommentsByEpisode(show.Ids.TvDbId.Value, season, episode) ?? new List<IComment>();
            if (res.Count == 0) return new DataResult<List<IComment>>(StandardResults.Error);
            return new DataResult<List<IComment>>(res);
        }

        public async Task<DataResult<ICommentResult>> CommentEpisode(string comment, int season, int episode, bool isSpoiler, bool isReview)
        {
            if (string.IsNullOrEmpty(comment) || season < 0 || episode < 0) return new DataResult<ICommentResult>(StandardResults.Error);
            comment = AddShiftvBrandHastag(comment);
            //if (!await IsInternet()) return new DataResult<ICommentResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<ICommentResult>(StandardResults.Error);
            var show = _showService.GetCurrentShow();
            if (show == null) return new DataResult<ICommentResult>(StandardResults.Error);
            var res = await _dataService.CommentEpisode(UserTokenDtoFactory.GetDto(user), comment, show.Title, show.Ids.TvDbId.Value, show.Year.Value, season, episode, isSpoiler, isReview);
            if (res == null) return new DataResult<ICommentResult>(StandardResults.Error);
            //var test = Ioc.Container.Resolve<IComment>();
            //test.Inserted = (long)DateTimeToUnixTimestamp(DateTime.Now);
            //test.Text = comment;
            //test.TextHtml = comment;
            //test.IsSpoiler = isSpoiler;
            //test.ImdbId = show.Ids.ImdbId;
            //test.User = user.UserProfile;
            //test.Episode = episode;
            //test.Season = season;
            //_dataService.SaveCommentLocally(test);
            return new DataResult<ICommentResult>(res);
        }

        public async Task<DataResult<List<IComment>>> GetCommentsMovie()
        {
            //if (!await IsInternet()) return new DataResult<List<IComment>>(StandardResults.Offline);
            var movie = _movieService.GetCurrentMovie();
            if (movie == null) return new DataResult<List<IComment>>(StandardResults.Error);
            var res = await _dataService.GetCommentsMovieById(IdsDtoFactory.GetDto(movie.Ids)) ?? new List<IComment>(); ;
            if (res.Count == 0) return new DataResult<List<IComment>>(StandardResults.Error);
            return new DataResult<List<IComment>>(res);
        }
        public async Task<DataResult<ICommentResult>> CommentsMovie(string comment, bool isSpoiler, bool isReview)
        {
            if (string.IsNullOrEmpty(comment)) return new DataResult<ICommentResult>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<ICommentResult>(StandardResults.Offline);
            comment = AddShiftvBrandHastag(comment);
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<ICommentResult>(StandardResults.Error);
            var movie = _movieService.GetCurrentMovie();
            if (movie == null) return new DataResult<ICommentResult>(StandardResults.Error);
            var res = await _dataService.CommentMovie(UserTokenDtoFactory.GetDto(user), comment, MovieDtoFactory.GetDto(movie), isSpoiler, isReview);
            if (res == null) return new DataResult<ICommentResult>(StandardResults.Error);
            //var test = Ioc.Container.Resolve<IComment>();
            //test.Inserted = (long)DateTimeToUnixTimestamp(DateTime.Now);
            //test.Text = comment;
            //test.TextHtml = comment;
            //test.IsSpoiler = isSpoiler;
            //test.ImdbId = movie.Ids.ImdbId;
            //test.User = user.UserProfile;
            //_dataService.SaveCommentLocally(test);
            return new DataResult<ICommentResult>(res);
        }


    }
}
