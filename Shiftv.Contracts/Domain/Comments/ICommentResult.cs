using Shiftv.Contracts.Domain.Results;

namespace Shiftv.Contracts.Domain.Comments
{
    public interface ICommentResult
    {
        RequestResults Status { get; set; }
    }
}
