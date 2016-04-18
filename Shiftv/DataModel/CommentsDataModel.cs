using System;
using Shiftv.Contracts.Domain.Comments;
using Shiftv.Helpers;

namespace Shiftv.DataModel
{
    public class CommentsDataModel
    {
        private IComment _model;

        public CommentsDataModel(IComment comment)
        {
            _model = comment;
            User = new NewUserDataModel(comment.User);
        }

        public NewUserDataModel User { get; set; }

        public string CommentDate
        {
            get
            {
                if (_model.CreatedAtDate != null) return "@ " + _model.CreatedAtDate.Value.ToString("G");
                return null;
            }
        }

        public string Comment { get { return _model.CommentText; } }
        public bool IsSpoiler { get { return _model.Spoiler; } }
        public DateTime? CommentDateTime
        {
            get
            {
                return _model.CreatedAtDate != null ? _model.CreatedAtDate.Value : (DateTime?) null;
            }
        }
    }
}