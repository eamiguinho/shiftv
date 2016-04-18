using Autofac;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public class UserProfileWatchedDtoFactory
    {
        public static IUserProfileWatched Create(UserProfileWatchedDto userProfileWatchedDto)
        {
            var userProfileWatched = Ioc.Container.Resolve<IUserProfileWatched>();
            userProfileWatched.Action = userProfileWatchedDto.Action;
            userProfileWatched.Watched = userProfileWatchedDto.Watched;
            userProfileWatched.Type = userProfileWatchedDto.Type;
            if (userProfileWatchedDto.Episode != null && userProfileWatchedDto.Show != null) userProfileWatched.Episode = EpisodeDtoFactory.Create(userProfileWatchedDto.Episode, userProfileWatchedDto.Show.Title);
            if (userProfileWatchedDto.Show != null) userProfileWatched.Show = ShowDtoFactory.CreateShow(userProfileWatchedDto.Show);
            if (userProfileWatchedDto.Movie != null) userProfileWatched.Movie = MovieDtoFactory.Create(userProfileWatchedDto.Movie);
            return userProfileWatched;
        }
    }
}