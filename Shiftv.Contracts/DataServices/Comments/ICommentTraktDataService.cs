using System.Collections.Generic;
using System.Threading.Tasks;
using Shiftv.Contracts.Data.Movies;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.Domain.Comments;

namespace Shiftv.Contracts.DataServices.Comments
{
    public interface ICommentTraktDataService
    {
        Task<List<IComment>> GetCommentsShowById(IdsDto ids);

        Task<ICommentResult> CommentShow(UserTokenDto userAccount, string comment, ShowDto show, bool isSpoiler,
            bool isReview);

        Task<List<IComment>> GetCommentsByEpisode(int tvdbId, int season, int episodeNumber);
        Task<ICommentResult> CommentEpisode(UserTokenDto getDto, string comment, string title, int tvDbId, int year, int season, int episode, bool isSpoiler, bool isReview);
        Task<List<IComment>> GetCommentsMovieById(IdsDto ids);

        Task<ICommentResult> CommentMovie(UserTokenDto userAccount, string comment, MovieDto movie, bool isSpoiler,
            bool isReview);

        void SaveCommentLocally(IComment test);
        Task<List<IComment>> GetCommentsLocally();  
    }
}