using System;
using Shiftv.Contracts.Domain.Comments;
using Shiftv.Contracts.Domain.Users;

namespace Shiftv.Core.Models.Comments
{
    class Comment : IComment
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string CreatedAt { get; set; }
        public DateTime? CreatedAtDate {
            get  {
                if (string.IsNullOrEmpty(CreatedAt)) return null;
                DateTime dateComm;
                var b = DateTime.TryParse(CreatedAt, out dateComm);
                return b ? DateTime.Parse(CreatedAt) : DateTime.Now;
            }
        }
        public string CommentText { get; set; }
        public bool Spoiler { get; set; }
        public bool Review { get; set; }
        public int Replies { get; set; }
        public int Likes { get; set; }
        public IUser User { get; set; }
    }
}
