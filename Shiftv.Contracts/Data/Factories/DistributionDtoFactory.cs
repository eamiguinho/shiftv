using Autofac;
using Shiftv.Contracts.Data.Stats;
using Shiftv.Contracts.Domain.Stats;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class DistributionDtoFactory
    {
        public static IDistribution Create(DistributionDto distributionDto)
        {
            var distribution = Ioc.Container.Resolve<IDistribution>();
            distribution.Star1 = distributionDto.Star1;
            distribution.Star2 = distributionDto.Star2;
            distribution.Star3 = distributionDto.Star3;
            distribution.Star4 = distributionDto.Star4;
            distribution.Star5 = distributionDto.Star5;
            distribution.Star6 = distributionDto.Star6;
            distribution.Star7 = distributionDto.Star7;
            distribution.Star8 = distributionDto.Star8;
            distribution.Star9 = distributionDto.Star9;
            distribution.Star10 = distributionDto.Star10;
            return distribution;
        }

        public static DistributionDto GetDto(IDistribution distribution)
        {
            var dto = new DistributionDto();
            dto.Star1 = distribution.Star1;
            dto.Star2 = distribution.Star2;
            dto.Star3 = distribution.Star3;
            dto.Star4 = distribution.Star4;
            dto.Star5 = distribution.Star5;
            dto.Star6 = distribution.Star6;
            dto.Star7 = distribution.Star7;
            dto.Star8 = distribution.Star8;
            dto.Star9 = distribution.Star9;
            dto.Star10 = distribution.Star10;
            return dto;
        }
    }
}