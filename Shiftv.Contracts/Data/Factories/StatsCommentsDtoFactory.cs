using Autofac;
using Shiftv.Contracts.Data.Stats;
using Shiftv.Contracts.Domain.Stats;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class StatsCommentsDtoFactory
    {
        public static IStatsComments Create(StatsCommentsDto commentsDto)
        {
            var comments = Ioc.Container.Resolve<IStatsComments>();
            comments.All = commentsDto.All;
            comments.Reviews = commentsDto.Reviews;
            comments.Shouts = comments.Shouts;
            return comments;
        }
    }
}