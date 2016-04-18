using Autofac;
using Shiftv.Contracts.Data.Comments;
using Shiftv.Contracts.Domain.Comments;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class CommentResultDtoFactory
    {
        public static ICommentResult Create(CommentResultDto commentResultDto)
        {
            var commentres = Ioc.Container.Resolve<ICommentResult>();
            switch (commentResultDto.Status)
            {
                case "success":
                    commentres.Status = RequestResults.Success;
                    break;
                default:
                    commentres.Status = RequestResults.Failure;
                    break;
            }
            return commentres;
        }
    }
}