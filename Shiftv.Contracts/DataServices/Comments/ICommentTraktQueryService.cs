using System.Threading.Tasks;

namespace Shiftv.Contracts.DataServices.Comments
{
    public interface ICommentTraktQueryService
    {
        Task<string> GetCommentsShowById(int? imdbId);
        Task<string> CommentShow();
        Task<string> EditCommentShow();
        Task<string> GetCommentsByEpisode(int tvdbId, int season, int episodeNumber);
        Task<string> CommentEpisode();
        Task<string> GetCommentsMovieById(int? imdbId);
        Task<string> CommentMovie();
    }
}