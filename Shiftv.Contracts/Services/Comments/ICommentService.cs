using System.Collections.Generic;
using System.Threading.Tasks;
using Shiftv.Contracts.Domain.Comments;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Global;

namespace Shiftv.Contracts.Services.Comments
{
    public interface ICommentService
    {
        Task<DataResult<List<IComment>>> GetCommentsShowById();
        Task<DataResult<ICommentResult>> CommentsShow(string comment, bool isSpoiler, bool isReview);

        Task<DataResult<ICommentResult>> CommentEpisode(string comment, int season, int episode, bool isSpoiler,
            bool isReview);

        Task<DataResult<List<IComment>>> GetCommentsMovie();
        Task<DataResult<ICommentResult>> CommentsMovie(string comment, bool isSpoiler, bool isReview);
        Task<DataResult<List<IComment>>> GetEpisodeComments(IShow show, int season, int episode);
    }
}