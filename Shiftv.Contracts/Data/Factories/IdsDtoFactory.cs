using Autofac;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class IdsDtoFactory
    {
        public static IIds Create(IdsDto dto)
        {
            if (dto == null) return null;
            var ids = Ioc.Container.Resolve<IIds>();
            ids.ImdbId = dto.ImdbId;
            ids.Slug = dto.Slug;
            ids.TmDbId = dto.TmDbId;
            ids.TraktId = dto.TraktId;
            ids.TvDbId = dto.TvDbId;
            ids.TvRageId = dto.TvRageId;
            return ids;
        }

        public static IdsDto GetDto(IIds data)
        {
            if (data == null) return null;
            var dto = new IdsDto
            {
                ImdbId = data.ImdbId,
                Slug = data.Slug,
                TmDbId = data.TmDbId,
                TraktId = data.TraktId,
                TvDbId = data.TvDbId,
                TvRageId = data.TvRageId
            };
            return dto;
        }
    }
}