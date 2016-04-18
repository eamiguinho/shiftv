using Autofac;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public class UserProfileStatsMoviesDtoFactory
    {
        public static IUserProfileStatsMovies Create(UserProfileStatsMoviesDto moviesDto)
        {
            var movies = Ioc.Container.Resolve<IUserProfileStatsMovies>();
            movies.Hated = moviesDto.Hated;
            movies.Loved = moviesDto.Loved;
            movies.Shouts = moviesDto.Shouts;
            movies.Watched = moviesDto.Watched;
            movies.Checkins = moviesDto.Checkins;
            movies.Scrobbles = moviesDto.Scrobbles;
            movies.Seen = moviesDto.Seen;
            return movies;
        }
    }
}