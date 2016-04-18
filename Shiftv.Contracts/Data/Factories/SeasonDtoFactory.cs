using System.Linq;
using Autofac;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class SeasonDtoFactory   
    {
        public static ISeason Create(SeasonDto dto, string showImdbId, string showTitle)
        {   
            if (dto == null) return null;
            var season = Ioc.Container.Resolve<ISeason>();
            season.Episodes = dto.Episodes != null
                ? dto.Episodes.Select(x => EpisodeDtoFactory.Create(x, showTitle)).ToList()
                : null;
            season.Images = ImageDtoFactory.Create(dto.Images);
            season.Number = dto.Number;
            season.Overview = dto.Overview;
            season.Rating = dto.Rating;
            season.EpisodeCount = dto.EpisodeCount;
            return season;
        }
    }
}