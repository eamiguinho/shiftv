using Autofac;
using Shiftv.Contracts.Data.Activity;
using Shiftv.Contracts.Domain.Activity;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public class ActivityElapsedDtoFactory
    {
        public static IActivityElapsed Create(ActivityElapsedDto activityElapsedDto)
        {
            var elapsed = Ioc.Container.Resolve<IActivityElapsed>();
            elapsed.Full = activityElapsedDto.Full;
            elapsed.Short = activityElapsedDto.Short;
            return elapsed;
        }
    }
}