using Autofac;
using Shiftv.Contracts.Data.Stats;
using Shiftv.Contracts.Domain.Stats;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class RatingDtoFactory
    {
        public static IRating Create(RatingDto dto)
        {
            if (dto == null) return null;
            var rating = Ioc.Container.Resolve<IRating>();
            rating.Hated = dto.Hated;
            rating.Loved = dto.Loved;
            rating.Percentage = dto.Percentage;
            rating.Votes = dto.Votes;
            rating.Distribution = dto.Distribution != null ? DistributionDtoFactory.Create(dto.Distribution) : null;
            return rating;
        }

        public static RatingDto GetDto(IRating rating)
        {
            if (rating == null) return null;
            var dto = new RatingDto();
            dto.Hated = rating.Hated;
            dto.Loved = rating.Loved;
            dto.Percentage = rating.Percentage;
            dto.Votes = rating.Votes;
            dto.Distribution = rating.Distribution != null ? DistributionDtoFactory.GetDto(rating.Distribution) : null;
            return dto;
        }
    }
}