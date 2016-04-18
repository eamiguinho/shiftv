using Shiftv.Contracts.Domain.Comments;
using Shiftv.Contracts.Domain.Results;

namespace Shiftv.Core.Models.Comments
{
    class CommentResult : ICommentResult
    {
        public RequestResults Status { get; set; }
        public string Message { get; set; }
    }
}
