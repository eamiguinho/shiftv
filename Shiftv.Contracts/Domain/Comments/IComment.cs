using System;
using Shiftv.Contracts.Domain.Users;

namespace Shiftv.Contracts.Domain.Comments
{
    public interface IComment
    {
        int Id { get; set; }
        int? ParentId { get; set; }
        string CreatedAt { get; set; }
        DateTime? CreatedAtDate { get; }
        string CommentText { get; set; }
        bool Spoiler { get; set; }
        bool Review { get; set; }
        int Replies { get; set; }
        int Likes { get; set; }
        IUser User { get; set; }
    }
}
