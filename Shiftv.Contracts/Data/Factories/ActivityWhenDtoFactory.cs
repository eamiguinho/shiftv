using Autofac;
using Shiftv.Contracts.Data.Activity;
using Shiftv.Contracts.Domain.Activity;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public class ActivityWhenDtoFactory
    {
        public static IActivityWhen Create(ActivityWhenDto whenDto)
        {
            var when = Ioc.Container.Resolve<IActivityWhen>();
            when.Day = whenDto.Day;
            when.Time = whenDto.Time;
            return when;
        }
    }
}