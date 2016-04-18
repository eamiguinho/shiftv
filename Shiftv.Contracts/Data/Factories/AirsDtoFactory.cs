using Autofac;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class AirsDtoFactory
    {
        public static IAirs Create(AirsDto dto)
        {
            if (dto == null) return null;
            var airs = Ioc.Container.Resolve<IAirs>();
            airs.Day = dto.Day;
            airs.Time = dto.Time;
            airs.Timezone = dto.Timezone;
            return airs;
        }

        public static AirsDto GetDto(IAirs data)
        {
            if (data == null) return null;
            var dto = new AirsDto
            {
                Day = data.Day,
                Time = data.Time,
                Timezone = data.Timezone
            };
            return dto;
        }
    }
}