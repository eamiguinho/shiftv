using Autofac;
using Shiftv.Contracts.Data.Results;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public class SetAsSeenResultJsonDtoFactory
    {
        public static ISetAsSeenResult Create(SetAsSeenResultJsonDto dto)
        {
            if (dto == null) return null;
            var setAsSeen = Ioc.Container.Resolve<ISetAsSeenResult>();
            if (dto.Request.Episode != null)
            {
                setAsSeen.Episode = Ioc.Container.Resolve<IEpisode>();
                setAsSeen.Episode.Ids = IdsDtoFactory.Create(dto.Request.Episode.Ids);
                setAsSeen.Episode.Number = dto.Request.Episode.Number;
                setAsSeen.Episode.Season = dto.Request.Episode.Season;
            }
            if (dto.Request.Show != null)
            {
                setAsSeen.Show = Ioc.Container.Resolve<IShow>();
                setAsSeen.Show.Ids = IdsDtoFactory.Create(dto.Request.Show.Ids);
                setAsSeen.Show.Title = dto.Request.Show.Title;
                setAsSeen.Show.Year = dto.Request.Show.Year;
            }
            if (dto.Request.Movie != null)
            {
                setAsSeen.Movie = Ioc.Container.Resolve<IMovie>();
                setAsSeen.Movie.Ids = IdsDtoFactory.Create(dto.Request.Movie.Ids);
                setAsSeen.Movie.Title = dto.Request.Movie.Title;
                setAsSeen.Movie.Year = dto.Request.Movie.Year;
            }
            setAsSeen.Success = dto.Success;
            return setAsSeen;
        }


    }
}