using Autofac;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public class UserProfileWatchingDtoFactory
    {
        public static IUserProfileWatching Create(UserProfileWatchingDto watchingDto)
        {
            var watching = Ioc.Container.Resolve<IUserProfileWatching>();
            watching.Action = watchingDto.Action;
            if (watchingDto.Episode != null && watchingDto.Show != null) watching.Episode = EpisodeDtoFactory.Create(watchingDto.Episode, watchingDto.Show.Title);
            if (watchingDto.Show != null) watching.Show = ShowDtoFactory.CreateShow(watchingDto.Show);
            if (watchingDto.Movie != null) watching.Movie = MovieDtoFactory.Create(watchingDto.Movie);
            watchingDto.Type = watchingDto.Type;
            return watching;
        }
    }
}