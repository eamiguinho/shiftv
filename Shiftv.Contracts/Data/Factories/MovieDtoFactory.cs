using System.Linq;
using Autofac;
using Shiftv.Contracts.Data.Movies;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class MovieDtoFactory
    {
        public static IMovie Create(MovieDto dto)
        {
            if (dto == null) return null;
            var movie = Ioc.Container.Resolve<IMovie>();
            movie.Certification = dto.Certification;
            movie.Genres = dto.Genres;
            movie.Overview = dto.Overview;
            movie.Released = dto.Released;
            movie.Runtime = dto.Runtime;
            movie.Tagline = dto.Tagline;
            movie.Title = dto.Title;
            movie.Trailer = dto.Trailer;
            movie.Year = dto.Year;
            movie.Ids = IdsDtoFactory.Create(dto.Ids);
            movie.Homepage = dto.Homepage;
            movie.Images = ImageDtoFactory.Create(dto.Images);
            movie.Language = dto.Language;
            movie.Rating = dto.Rating;
            movie.UpdatedAt = dto.UpdatedAt;
            movie.Votes = dto.Votes;
            movie.UserRating = dto.UserRating;
            movie.Watched = dto.Watched;
            movie.InWatchlist = dto.InWatchlist;
            return movie;
        }

        public static IMovie CreateClean(MovieDto dto)
        {
            var movie = Create(dto);
            return movie;
        }

        public static MovieDto GetDto(IMovie movie)
        {
            if (movie == null) return null;
            var dto = new MovieDto();
            dto.Certification = movie.Certification;
            dto.Genres = movie.Genres;
            dto.Overview = movie.Overview;
            dto.Released = movie.Released;
            dto.Runtime = movie.Runtime;
            dto.Tagline = movie.Tagline;
            dto.Title = movie.Title;
            dto.Trailer = movie.Trailer;
            dto.Year = movie.Year;
            dto.Ids = IdsDtoFactory.GetDto(movie.Ids);
            dto.Homepage = dto.Homepage;
            dto.Images = ImageDtoFactory.GetDto(movie.Images);
            dto.Language = movie.Language;
            dto.Rating = movie.Rating;
            dto.UpdatedAt = movie.UpdatedAt;
            dto.Votes = movie.Votes;
            dto.UserRating = movie.UserRating;
            dto.Watched = movie.Watched;
            dto.InWatchlist = movie.InWatchlist;
            return dto;
        }
    }
}