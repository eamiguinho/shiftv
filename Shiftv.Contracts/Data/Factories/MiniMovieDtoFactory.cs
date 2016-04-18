using Autofac;
using Shiftv.Contracts.Data.Movies;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class MiniMovieDtoFactory
    {
        public static IMiniMovie Create(MiniMovieDto dto)
        {
            if (dto == null) return null;
            var movie = Ioc.Container.Resolve<IMiniMovie>();
            movie.Fanart = dto.Fanart;
            movie.Genres = dto.Genres;
            movie.Ids = IdsDtoFactory.Create(dto.Ids);
            movie.Rating = dto.Rating;
            movie.Runtime = dto.Runtime;
            movie.Title = dto.Title;
            movie.Votes = dto.Votes;
            movie.Released = dto.Released;
            movie.UserRating = dto.UserRating;
            movie.Watched = dto.Watched;
            movie.InWatchlist = dto.InWatchlist;
            return movie;
        }
    }
}